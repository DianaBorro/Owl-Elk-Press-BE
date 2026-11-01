using Microsoft.EntityFrameworkCore;
using owl_and_elk_press_BE.Models;

namespace owl_and_elk_press_BE.BusinessLogic.NovelNestNovember;

    public class WordTrackingService
    {
        private readonly NovelNestDbContext _context;

        public WordTrackingService(NovelNestDbContext context)
        {
            _context = context;
        }

        public async Task<List<WordLog>> GetUserLogsAsync(string userId)
        {
            return await _context.WordLogs
                .Where(w => w.UserId == userId)
                .OrderBy(w => w.LogDate)
                .ToListAsync();
        }

        public async Task<WordLog> LogWordsAsync(string userId, DateTime date, int wordCount)
        {
            if (wordCount < 0) throw new ArgumentException("The wordcount cannot be negative.");
            
            var midnightDate = date.Date; 
            var existingLog = await _context.WordLogs
                .FirstOrDefaultAsync(w => w.UserId == userId && w.LogDate == midnightDate);

            if (existingLog != null)
            {
                existingLog.WordCount = wordCount; 
            }
            else
            {
                existingLog = new WordLog
                {
                    UserId = userId,
                    LogDate = midnightDate,
                    WordCount = wordCount
                };
                await _context.WordLogs.AddAsync(existingLog);
            }

            await _context.SaveChangesAsync();
            return existingLog;
        }
    }
