using StudySessionPlanner_App.Models;
using StudySessionPlanner_App.Services;
using StudySessionPlanner.Tests.Helpers;
using Xunit;

namespace StudySessionPlanner.Tests.Services
{
    public class StudySessionServiceTests
    {
        [Fact]
        public async Task GetFilteredSessionsAsync_ShouldReturnAllSessions_WhenNoFiltersAreApplied()
        {
            // Arrange
            var context = TestDbContextFactory.Create();

            var topic = new Topic { Name = "Databases" };
            context.Topics.Add(topic);
            await context.SaveChangesAsync();

            context.StudySessions.AddRange(
                new StudySession
                {
                    Title = "SQL Basics",
                    Location = "Room 101",
                    StartTime = DateTime.UtcNow.AddDays(1),
                    DurationMinutes = 60,
                    TopicId = topic.Id
                },
                new StudySession
                {
                    Title = "LINQ Practice",
                    Location = "Room 102",
                    StartTime = DateTime.UtcNow.AddDays(2),
                    DurationMinutes = 90,
                    TopicId = topic.Id
                });

            await context.SaveChangesAsync();

            var service = new StudySessionService(context);

            // Act
            var result = await service.GetFilteredSessionsAsync(null, null);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetFilteredSessionsAsync_ShouldFilterBySearchTerm()
        {
            // Arrange
            var context = TestDbContextFactory.Create();

            var topic = new Topic { Name = "Databases" };
            context.Topics.Add(topic);
            await context.SaveChangesAsync();

            context.StudySessions.AddRange(
                new StudySession
                {
                    Title = "SQL Basics",
                    Location = "Room 101",
                    StartTime = DateTime.UtcNow.AddDays(1),
                    DurationMinutes = 60,
                    TopicId = topic.Id
                },
                new StudySession
                {
                    Title = "LINQ Practice",
                    Location = "Room 102",
                    StartTime = DateTime.UtcNow.AddDays(2),
                    DurationMinutes = 90,
                    TopicId = topic.Id
                });

            await context.SaveChangesAsync();

            var service = new StudySessionService(context);

            // Act
            var result = await service.GetFilteredSessionsAsync("SQL", null);

            // Assert
            Assert.Single(result);
            Assert.Equal("SQL Basics", result.First().Title);
        }

        [Fact]
        public async Task GetFilteredSessionsAsync_ShouldFilterByTopicId()
        {
            // Arrange
            var context = TestDbContextFactory.Create();

            var topic1 = new Topic { Name = "Databases" };
            var topic2 = new Topic { Name = "C#" };

            context.Topics.AddRange(topic1, topic2);
            await context.SaveChangesAsync();

            context.StudySessions.AddRange(
                new StudySession
                {
                    Title = "SQL Basics",
                    Location = "Room 101",
                    StartTime = DateTime.UtcNow.AddDays(1),
                    DurationMinutes = 60,
                    TopicId = topic1.Id
                },
                new StudySession
                {
                    Title = "OOP Principles",
                    Location = "Room 102",
                    StartTime = DateTime.UtcNow.AddDays(2),
                    DurationMinutes = 90,
                    TopicId = topic2.Id
                });

            await context.SaveChangesAsync();

            var service = new StudySessionService(context);

            // Act
            var result = await service.GetFilteredSessionsAsync(null, topic2.Id);

            // Assert
            Assert.Single(result);
            Assert.Equal("OOP Principles", result.First().Title);
        }
    }
}