using HikingQuests.Server.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HikingQuests.Tests.Infrastructure.PersistanceTests
{
    public class SqliteQuestTestFixture : IDisposable
    {
        private readonly SqliteConnection _connection;

        public DbContextOptions<QuestDbContext> ContextOptions { get; }

        public SqliteQuestTestFixture()
        {
            _connection = new SqliteConnection("Datasource=:memory:");
            _connection.Open();

            ContextOptions = new DbContextOptionsBuilder<QuestDbContext>()
                .UseSqlite(_connection)
                .Options;

            using (var context = new QuestDbContext(ContextOptions))
            {
                context.Database.EnsureCreated();
            }
        }

        public QuestDbContext CreateContext() => new QuestDbContext(ContextOptions);

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
