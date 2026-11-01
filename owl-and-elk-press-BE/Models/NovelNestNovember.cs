namespace owl_and_elk_press_BE.Models;

using System;
using Microsoft.EntityFrameworkCore;
    public class WordLog
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime LogDate { get; set; }
        public int WordCount { get; set; }
    }

    public class NovelNestDbContext : DbContext
    {
        public NovelNestDbContext(DbContextOptions<NovelNestDbContext> options) : base(options) { }
        public DbSet<WordLog> WordLogs => Set<WordLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WordLog>().HasIndex(w => new { w.UserId, w.LogDate }).IsUnique();
        }
    }
