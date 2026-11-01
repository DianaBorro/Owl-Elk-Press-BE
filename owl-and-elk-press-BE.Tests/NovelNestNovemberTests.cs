using Microsoft.EntityFrameworkCore;
using owl_and_elk_press_BE.BusinessLogic.NovelNestNovember;
using owl_and_elk_press_BE.Models;

namespace owl_and_elk_press_BE.Tests;

    public class WordTrackingServiceTests
    {
        private NovelNestDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<NovelNestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;
            
            return new NovelNestDbContext(options);
        }

        [Fact]
        public async Task LogWordsAsync_ShouldAddNewLog_WhenDayDoesNotExist()
        {
            using var context = CreateInMemoryDbContext();
            var service = new WordTrackingService(context);
            var userId = "google-user-123";
            var testDate = new DateTime(2026, 11, 1);

            var result = await service.LogWordsAsync(userId, testDate, 1667);

            Assert.NotNull(result);
            Assert.Equal(1667, result.WordCount);
            var logsInDb = await context.WordLogs.CountAsync();
            Assert.Equal(1, logsInDb);
        }

        [Fact]
        public async Task LogWordsAsync_ShouldUpdateExistingLog_WhenDayAlreadyExists()
        {
            using var context = CreateInMemoryDbContext();
            var service = new WordTrackingService(context);
            var userId = "google-user-123";
            var testDate = new DateTime(2026, 11, 1);

            await service.LogWordsAsync(userId, testDate, 1000);

            var updatedResult = await service.LogWordsAsync(userId, testDate, 1800);

            Assert.Equal(1800, updatedResult.WordCount);
            var totalLogs = await context.WordLogs.CountAsync();
            Assert.Equal(1, totalLogs);
        }

        [Fact]
        public async Task LogWordsAsync_ShouldThrowException_WhenWordsAreNegative()
        {
            using var context = CreateInMemoryDbContext();
            var service = new WordTrackingService(context);

            await Assert.ThrowsAsync<ArgumentException>(() => 
                service.LogWordsAsync("user1", DateTime.UtcNow, -50)
            );
        }
    }
