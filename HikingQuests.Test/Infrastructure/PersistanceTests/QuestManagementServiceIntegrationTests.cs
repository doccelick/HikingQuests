using HikingQuests.Server.Application;
using HikingQuests.Server.Application.Dtos;
using HikingQuests.Server.Constants;
using HikingQuests.Server.Domain.Entities;
using HikingQuests.Server.Infrastructure;

namespace HikingQuests.Tests.Infrastructure.PersistanceTests
{

    public class QuestManagementServiceIntegrationTests : IClassFixture<SqliteQuestTestFixture>, IAsyncLifetime
    {
        private readonly SqliteQuestTestFixture _fixture;

        public QuestManagementServiceIntegrationTests(SqliteQuestTestFixture fixture)
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
        public async Task AddQuestAsync_Should_Persist_Quest_When_Called()
        {
            // ARRANGE
            await using var arrangeContext = _fixture.CreateContext();
            var repository = new QuestRepository(arrangeContext);
            var service = new QuestManagementService(repository);

            var itemToAdd = new QuestItem("New Quest via Service", "Description via Service");
            var itemToAddDto = new AddQuestDto() { Title = itemToAdd.Title, Description = itemToAdd.Description};
            // ACT
            var addedItem = await service.AddQuestAsync(itemToAddDto);
            await arrangeContext.SaveChangesAsync();

            // ASSERT
            await using var assertContext = _fixture.CreateContext();
            var assertRepository = new QuestRepository(arrangeContext);

            var retrievedItem = await assertRepository.GetByIdAsync(addedItem.Id);

            Assert.NotNull(retrievedItem);
            Assert.Equal(itemToAdd.Title, retrievedItem.Title);
        }

        [Fact]
        public async Task UpdateQuestAsync_Should_Update_Title_And_Description_When_Quest_Exists()
        {
            // ARRANGE
            await using var arrangeContext = _fixture.CreateContext();
            var repository = new QuestRepository(arrangeContext);
            var service = new QuestManagementService(repository);

            var originalTitle = "Original title";
            var originalDescription = "Original Description";
            var itemToAddDto = new AddQuestDto() { Title = originalTitle, Description = originalDescription };

            var savedItem = await service.AddQuestAsync(itemToAddDto);
            await arrangeContext.SaveChangesAsync();

            var newTitle = "New Title";
            var newDescription = "New Description";

            var updateQuestDto = new UpdateQuestDto() 
            {
                Title = newTitle, 
                Description = newDescription
            };

            // ACT
            await service.UpdateQuestAsync(savedItem.Id, updateQuestDto);

            await arrangeContext.SaveChangesAsync();

            // ASSERT
            await using var assertContext = _fixture.CreateContext();
            var assertRepository = new QuestRepository(assertContext);

            var retrievedItem = await assertRepository.GetByIdAsync(savedItem.Id);

            Assert.NotNull(retrievedItem);
            Assert.Equal(newTitle, retrievedItem.Title);
            Assert.Equal(newDescription, retrievedItem.Description);
        }

        [Fact]
        public async Task DeleteQuestAsync_Should_Remove_Quest_From_DB_When_Quest_Exists()
        {
            // ARRANGE
            await using var arrangeContext = _fixture.CreateContext();
            var repository = new QuestRepository(arrangeContext);
            var service = new QuestManagementService(repository);

            var itemToDelete = new QuestItem("Quest to Delete", "Description");
            await repository.AddAsync(itemToDelete);
            await arrangeContext.SaveChangesAsync();

            // ACT
            await service.DeleteQuestAsync(itemToDelete.Id);
            await arrangeContext.SaveChangesAsync();

            // ASSERT
            await using var assertContext = _fixture.CreateContext();
            var assertRepository = new QuestRepository(assertContext);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await assertRepository.GetByIdAsync(itemToDelete.Id));

            Assert.Contains(QuestMessages.QuestNotFound, exception.Message);
        }
    }
}
