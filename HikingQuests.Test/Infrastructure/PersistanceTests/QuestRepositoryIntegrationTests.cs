using HikingQuests.Server.Domain.ValueObjects;
using HikingQuests.Server.Infrastructure;
using HikingQuests.Server.Infrastructure.Entities;
using HikingQuests.Server.Domain.Entities;
using HikingQuests.Server.Constants;
using HikingQuests.Server.Infrastructure.Persistence;

namespace HikingQuests.Tests.Infrastructure.PersistanceTests
{
    public class QuestRepositoryIntegrationTests : IClassFixture<SqliteQuestTestFixture>, IAsyncLifetime
    {
        private readonly SqliteQuestTestFixture _fixture;

        public QuestRepositoryIntegrationTests(SqliteQuestTestFixture fixture)
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
        public async Task AddAsync_Should_Persist_QuestEntity_When_Called()
        {
            //Arrange
            await using var context = _fixture.CreateContext();
            var unitOfWork = new UnitOfWork(context);
            var repository = new QuestRepository(context);
            var entityToAdd = new QuestItem("First integrated Quest", "Test description");
            
            //Act
            await repository.AddAsync(entityToAdd);
            await unitOfWork.SaveChangesAsync();

            //Assert
            await using var verificationContext = _fixture.CreateContext();
            var verificationRepository = new QuestRepository(verificationContext);
            var retrievedEntry = await verificationRepository.GetByIdAsync(entityToAdd.Id);

            Assert.NotNull(retrievedEntry);
            Assert.Equal(entityToAdd.Title, retrievedEntry.Title);
            Assert.Equal(entityToAdd.Status, retrievedEntry.Status);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Persisted_Quests_When_Quests_Exist()
        {
            // ARRANGE
            await using var context = _fixture.CreateContext();
            var unitOfWork = new UnitOfWork(context);
            var repository = new QuestRepository(context);
            var entitiesToAdd = new List<QuestEntity>()
            {
               new QuestEntity { Id = Guid.NewGuid(), Title = "Quest 1", Description = "Description 1", Status = QuestStatus.Planned },
               new QuestEntity { Id = Guid.NewGuid(), Title = "Quest 2", Description = "Description 2", Status = QuestStatus.Planned },
               new QuestEntity { Id = Guid.NewGuid(), Title = "Quest 3", Description = "Description 3", Status = QuestStatus.Planned },
            };

            // ACT
            context.QuestItems.AddRange(entitiesToAdd);
            await unitOfWork.SaveChangesAsync();

            var retrievedQuests = await repository.GetAllQuestsAsync();

            // ASSERT
            Assert.NotNull(retrievedQuests);
            Assert.Equal(entitiesToAdd.Count(), retrievedQuests.Count());
            var retrievedTitles = retrievedQuests.Select(q => q.Title).ToList();
            Assert.Contains("Quest 1", retrievedTitles);
            Assert.Contains("Quest 2", retrievedTitles);
            Assert.Contains("Quest 3", retrievedTitles);

        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_A_Persisted_Quest_When_Quest_Exist()
        {
            // ARRANGE
            await using var context = _fixture.CreateContext();
            var unitOfWork = new UnitOfWork(context);
            var repository = new QuestRepository(context);
            var entityToAdd = new QuestEntity()
            {
                Id = Guid.NewGuid(),
                Title = "Added Quest",
                Description = "Quest Desctription",
                Status = QuestStatus.Planned
            };

            // ACT
            context.QuestItems.Add(entityToAdd);
            await unitOfWork.SaveChangesAsync();

            var retrievedQuest = await repository.GetByIdAsync(entityToAdd.Id);

            // ASSERT
            Assert.NotNull(retrievedQuest);
            Assert.Equal(entityToAdd.Id, retrievedQuest.Id);
        }

        [Fact]
        public async Task GetByTitleAsync_Should_Return_A_Persisted_Quest_When_Quest_Exist()
        {
            // ARRANGE
            await using var context = _fixture.CreateContext();
            var unitOfWork = new UnitOfWork(context);
            var repository = new QuestRepository(context);
            var entityToAdd = new QuestEntity()
            {
                Id = Guid.NewGuid(),
                Title = "Added Quest",
                Description = "Quest Desctription",
                Status = QuestStatus.Planned
            };

            // ACT
            context.QuestItems.Add(entityToAdd);
            await unitOfWork.SaveChangesAsync();

            var retrievedQuest = await repository.GetByTitleAsync(entityToAdd.Title);

            // ASSERT
            Assert.NotNull(retrievedQuest);
            Assert.Equal(entityToAdd.Id, retrievedQuest.Id);
            Assert.Equal(entityToAdd.Title, retrievedQuest.Title);
        }


        [Fact]
        public async Task UpdateAsync_Should_UpdateTitleAndDescription_When_QuestExists()
        {
            // ARRANGE
            await using var context = _fixture.CreateContext();
            var unitOfWork = new UnitOfWork(context);
            var arrangeRepository = new QuestRepository(context);
            var initialQuest = new QuestItem("Original Title", "Original Description");
            await arrangeRepository.AddAsync(initialQuest);
            await unitOfWork.SaveChangesAsync();
            var itemToUpdate = new QuestItem(initialQuest.Id, "New Updated Title", "New Description", initialQuest.Status);

            // ACT
            await arrangeRepository.UpdateAsync(itemToUpdate);
            await unitOfWork.SaveChangesAsync();

            // ASSERT
            await using var assertContext = _fixture.CreateContext();
            var assertRepository = new QuestRepository(assertContext);

            var retrievedQuest = await assertRepository.GetByIdAsync(initialQuest.Id);

            Assert.NotNull(retrievedQuest);
            Assert.Equal(itemToUpdate.Title, retrievedQuest.Title);
            Assert.Equal(itemToUpdate.Description, retrievedQuest.Description);

            Assert.Equal(initialQuest.Status, retrievedQuest.Status);
        }

        [Fact]
        public async Task DeleteAsync_Should_RemoveQuestEntity_When_QuestExists()
        {
            // ARRANGE
            await using var context = _fixture.CreateContext();
            var unitOfWork = new UnitOfWork(context);
            var arrangeRepository = new QuestRepository(context);
            var questToDelete = new QuestItem("Quest to Delete", "Description to Delete");
            await arrangeRepository.AddAsync(questToDelete);
            await unitOfWork.SaveChangesAsync();

            // ACT
            await arrangeRepository.DeleteAsync(questToDelete.Id);
            await unitOfWork.SaveChangesAsync();

            // ASSERT
            await using var assertContext = _fixture.CreateContext();
            var assertRepository = new QuestRepository(assertContext);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await assertRepository.GetByIdAsync(questToDelete.Id));

            Assert.Contains(QuestMessages.QuestNotFound, exception.Message);
        }

    }
}
