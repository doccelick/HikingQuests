using HikingQuests.Server.Application;
using HikingQuests.Server.Domain.Entities;
using HikingQuests.Server.Domain.ValueObjects;
using HikingQuests.Server.Infrastructure;
using HikingQuests.Server.Infrastructure.Persistence;

namespace HikingQuests.Tests.Infrastructure.PersistanceTests
{
    public class QuestWorkflowServiceIntegrationTests : IClassFixture<SqliteQuestTestFixture>, IAsyncLifetime
    {
        private readonly SqliteQuestTestFixture _fixture;
        
        public QuestWorkflowServiceIntegrationTests(SqliteQuestTestFixture fixture)
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
        public async Task StartQuestAsync_Should_Update_Status_In_Database()
        {
            // ARRANGE
            await using var context = _fixture.CreateContext();
            var unitOfWork = new UnitOfWork(context);
            var repository = new QuestRepository(context);
            var service = new QuestWorkflowService(repository);

            var plannedQuest = new QuestItem("Quest Title", "Quest Description");
            await repository.AddAsync(plannedQuest);
            await unitOfWork.SaveChangesAsync();

            // ACT
            await service.StartQuestAsync(plannedQuest.Id);
            await unitOfWork.SaveChangesAsync();

            // ASSERT
            await using var assertContext = _fixture.CreateContext();
            var assertRepository = new QuestRepository(assertContext);

            var retrievedQuest = await assertRepository.GetByIdAsync(plannedQuest.Id);

            Assert.Equal(QuestStatus.InProgress, retrievedQuest.Status);
        }

        [Fact]
        public async Task StartQuestAsync_Should_Propagate_KeyNotFoundException_When_Quest_Missing()
        {
            // ARRANGE
            await using var context = _fixture.CreateContext();
            var repository = new QuestRepository(context);
            var service = new QuestWorkflowService(repository);
            var nonExistentId = Guid.NewGuid();

            // ACT & ASSERT:
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await service.StartQuestAsync(nonExistentId));

            Assert.Contains("Quest not found", exception.Message);
        }

        [Fact]
        public async Task CompleteQuestAsync_Should_Update_Status_In_Database()
        {
            // ARRANGE
            await using var context = _fixture.CreateContext();
            var unitOfWork = new UnitOfWork(context);
            var repository = new QuestRepository(context);
            var service = new QuestWorkflowService(repository);

            var questToComplete = new QuestItem("Quest Title", "Quest Description");
            questToComplete.StartQuest();

            await repository.AddAsync(questToComplete);
            await unitOfWork.SaveChangesAsync();

            // ACT
            await service.CompleteQuestAsync(questToComplete.Id);
            await unitOfWork.SaveChangesAsync();

            // ASSERT
            await using var assertContext = _fixture.CreateContext();
            var assertRepository = new QuestRepository(assertContext);
            var retrievedQuest = await assertRepository.GetByIdAsync(questToComplete.Id);

            Assert.Equal(QuestStatus.Completed, retrievedQuest.Status);
        }

        [Fact]
        public async Task CompleteQuestAsync_Should_Throw_InvalidOperationException_And_Not_Update_DB()
        {
            // ARRANGE
            await using var context = _fixture.CreateContext();
            var unitOfWork = new UnitOfWork(context);
            var repository = new QuestRepository(context);
            var service = new QuestWorkflowService(repository);

            var plannedQuest = new QuestItem("Quest Title", "Quest Description");
            await repository.AddAsync(plannedQuest);
            await unitOfWork.SaveChangesAsync();

            var originalStatus = plannedQuest.Status;

            // ACT & ASSERT
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await service.CompleteQuestAsync(plannedQuest.Id));

            // ASSERT:
            await using var assertContext = _fixture.CreateContext();
            var assertRepository = new QuestRepository(assertContext);

            var retrievedQuest = await assertRepository.GetByIdAsync(plannedQuest.Id);

            Assert.Equal(originalStatus, retrievedQuest.Status);
        }
    }
}
