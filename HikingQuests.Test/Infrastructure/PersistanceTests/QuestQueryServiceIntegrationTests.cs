using HikingQuests.Server.Application;
using HikingQuests.Server.Domain.ValueObjects;
using HikingQuests.Server.Infrastructure;
using HikingQuests.Server.Infrastructure.Entities;
using HikingQuests.Server.Infrastructure.Persistence;

namespace HikingQuests.Tests.Infrastructure.PersistanceTests
{
    public class QuestQueryServiceIntegrationTests : IClassFixture<SqliteQuestTestFixture>, IAsyncLifetime
    {
        private readonly SqliteQuestTestFixture _fixture;

        public QuestQueryServiceIntegrationTests(SqliteQuestTestFixture fixture)
        {
            _fixture = fixture;
        }

        public async Task InitializeAsync()
        {
            await using var context = _fixture.CreateContext();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task GetAllQuestsAsync_Should_Return_All_Persisted_Quests_When_Quests_Exist()
        {
            // ARRANGE
            await using var context = _fixture.CreateContext();
            var unitOfWork = new UnitOfWork(context);
            var repository = new QuestRepository(context);
            var service = new QuestQueryService(repository);

            var entitiesToAdd = new List<QuestEntity>()
            {
               new QuestEntity { Id = Guid.NewGuid(), Title = "Quest 1", Description = "Description 1", Status = QuestStatus.Planned },
               new QuestEntity { Id = Guid.NewGuid(), Title = "Quest 2", Description = "Description 2", Status = QuestStatus.Planned },
               new QuestEntity { Id = Guid.NewGuid(), Title = "Quest 3", Description = "Description 3", Status = QuestStatus.Planned },
            };

            // ACT
            context.QuestItems.AddRange(entitiesToAdd);
            await unitOfWork.SaveChangesAsync();

            // ASSERT
            var retrievedQuests = await service.GetAllQuestsAsync();
            Assert.NotNull(retrievedQuests);
            Assert.Equal(entitiesToAdd.Count(), retrievedQuests.Count());
            var retrievedTitles = retrievedQuests.Select(q => q.Title).ToList();
            Assert.Contains("Quest 1", retrievedTitles);
            Assert.Contains("Quest 2", retrievedTitles);
            Assert.Contains("Quest 3", retrievedTitles);
        }

        [Fact]
        public async Task GetQuestByIdAsync_Should_Return_Persisted_Quest_When_Quest_Exists()
        {
            // ARRANGE
            await using var context = _fixture.CreateContext();
            var unitOfWork = new UnitOfWork(context);
            var repository = new QuestRepository(context);
            var service = new QuestQueryService(repository);

            var entityToAdd = new QuestEntity()
            {
                Id = Guid.NewGuid(), 
                Title = "Quest 1", 
                Description = "Description 1", 
                Status = QuestStatus.Planned
            };            

            // ACT
            context.QuestItems.Add(entityToAdd);
            await unitOfWork.SaveChangesAsync();

            // ASSERT
            var retrievedQuest = await service.GetQuestByIdAsync(entityToAdd.Id);
            Assert.NotNull(retrievedQuest);
            Assert.Equal(entityToAdd.Id, retrievedQuest.Id);
            Assert.Equal(entityToAdd.Title, retrievedQuest.Title);
        }

        [Fact]
        public async Task GetQuestByTitleAsync_Should_Return_Persisted_Quest_When_Quest_Exists()
        {
            // ARRANGE
            await using var context = _fixture.CreateContext();
            var unitOfwork = new UnitOfWork(context);
            var repository = new QuestRepository(context);
            var service = new QuestQueryService(repository);

            var entityToAdd = new QuestEntity()
            {
                Id = Guid.NewGuid(),
                Title = "Quest 1",
                Description = "Description 1",
                Status = QuestStatus.Planned
            };

            // ACT
            context.QuestItems.Add(entityToAdd);
            await unitOfwork.SaveChangesAsync();

            // ASSERT
            var retrievedQuest = await service.GetQuestByTitleAsync(entityToAdd.Title);
            Assert.NotNull(retrievedQuest);
            Assert.Equal(entityToAdd.Id, retrievedQuest.Id);
            Assert.Equal(entityToAdd.Title, retrievedQuest.Title);
        }
    }
}
