using HikingQuests.Server.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace HikingQuests.Server.Infrastructure.Persistence
{
    public class QuestDbContext : DbContext
    {
        public DbSet<QuestEntity> QuestItems { get; set; }

        public QuestDbContext(DbContextOptions<QuestDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<QuestEntity>().HasKey(q => q.Id);
        }
    }
}
