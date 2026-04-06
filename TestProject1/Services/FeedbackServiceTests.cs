using StudySessionPlanner_App.Models;
using StudySessionPlanner_App.Services;
using StudySessionPlanner.Tests.Helpers;
using Xunit;

namespace StudySessionPlanner.Tests.Services
{
    public class FeedbackServiceTests
    {
        [Fact]
        public async Task AddFeedbackAsync_ShouldAddFeedback_WhenSessionExistsAndUserHasNotLeftFeedback()
        {
            // Arrange
            var context = TestDbContextFactory.Create();

            var topic = new Topic
            {
                Name = "Databases"
            };

            context.Topics.Add(topic);
            await context.SaveChangesAsync();

            var session = new StudySession
            {
                Title = "Test Session",
                Location = "Room 101",
                StartTime = DateTime.UtcNow.AddDays(1),
                DurationMinutes = 60,
                TopicId = topic.Id
            };

            context.StudySessions.Add(session);
            await context.SaveChangesAsync();

            var service = new FeedbackService(context);

            // Act
            var result = await service.AddFeedbackAsync(session.Id, "user1", "Very helpful session", 5);

            // Assert
            Assert.True(result);
            Assert.Equal(1, context.Feedbacks.Count());
        }

        [Fact]
        public async Task AddFeedbackAsync_ShouldNotAllowDuplicateFeedbackFromSameUser()
        {
            // Arrange
            var context = TestDbContextFactory.Create();

            var topic = new Topic
            {
                Name = "Databases"
            };

            context.Topics.Add(topic);
            await context.SaveChangesAsync();

            var session = new StudySession
            {
                Title = "Test Session",
                Location = "Room 101",
                StartTime = DateTime.UtcNow.AddDays(1),
                DurationMinutes = 60,
                TopicId = topic.Id
            };

            context.StudySessions.Add(session);
            await context.SaveChangesAsync();

            var service = new FeedbackService(context);

            // Act
            await service.AddFeedbackAsync(session.Id, "user1", "Very helpful session", 5);
            var result = await service.AddFeedbackAsync(session.Id, "user1", "Second feedback attempt", 4);

            // Assert
            Assert.False(result);
            Assert.Equal(1, context.Feedbacks.Count());
        }

        [Fact]
        public async Task GetFeedbackForSessionAsync_ShouldReturnAllFeedbackForGivenSession()
        {
            // Arrange
            var context = TestDbContextFactory.Create();

            var topic = new Topic
            {
                Name = "Databases"
            };

            context.Topics.Add(topic);
            await context.SaveChangesAsync();

            var session = new StudySession
            {
                Title = "Test Session",
                Location = "Room 101",
                StartTime = DateTime.UtcNow.AddDays(1),
                DurationMinutes = 60,
                TopicId = topic.Id
            };

            context.StudySessions.Add(session);
            await context.SaveChangesAsync();

            var user1 = new ApplicationUser
            {
                Id = "user1",
                UserName = "user1@test.com",
                Email = "user1@test.com"
            };

            var user2 = new ApplicationUser
            {
                Id = "user2",
                UserName = "user2@test.com",
                Email = "user2@test.com"
            };

            context.Users.AddRange(user1, user2);
            await context.SaveChangesAsync();

            context.Feedbacks.AddRange(
                new Feedback
                {
                    StudySessionId = session.Id,
                    UserId = user1.Id,
                    Comment = "Great session",
                    Rating = 5
                },
                new Feedback
                {
                    StudySessionId = session.Id,
                    UserId = user2.Id,
                    Comment = "Very useful",
                    Rating = 4
                });

            await context.SaveChangesAsync();

            var service = new FeedbackService(context);

            // Act
            var result = await service.GetFeedbackForSessionAsync(session.Id);

            // Assert
            Assert.Equal(2, result.Count);
        }
    }
}