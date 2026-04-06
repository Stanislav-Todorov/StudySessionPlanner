using StudySessionPlanner_App.Models;
using StudySessionPlanner_App.Services;
using StudySessionPlanner.Tests.Helpers;

using Xunit;

namespace StudySessionPlanner.Tests.Services
{
    public class EnrollmentServiceTests
    {
        [Fact]
        public async Task EnrollUserAsync_ShouldEnrollUser_WhenSessionExistsAndUserNotEnrolled()
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

            var service = new EnrollmentService(context);

            string userId = "user1";

            // Act
            var result = await service.EnrollUserAsync(session.Id, userId);

            // Assert
            Assert.True(result);
            Assert.Equal(1, context.Enrollments.Count());
        }



        [Fact]
        public async Task EnrollUserAsync_ShouldNotEnrollUserTwice()
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

            var service = new EnrollmentService(context);

            string userId = "user1";

            // Act
            await service.EnrollUserAsync(session.Id, userId);
            var result = await service.EnrollUserAsync(session.Id, userId);

            // Assert
            Assert.False(result);
            Assert.Equal(1, context.Enrollments.Count());
        }


        [Fact]
        public async Task EnrollUserAsync_ShouldReturnFalse_WhenSessionDoesNotExist()
        {
            // Arrange
            var context = TestDbContextFactory.Create();
            var service = new EnrollmentService(context);

            string userId = "user1";

            // Act
            var result = await service.EnrollUserAsync(999, userId);

            // Assert
            Assert.False(result);
        }
    }
}