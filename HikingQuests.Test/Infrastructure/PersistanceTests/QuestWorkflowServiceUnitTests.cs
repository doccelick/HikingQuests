using HikingQuests.Server.Application;
using HikingQuests.Server.Constants;
using HikingQuests.Server.Domain.Entities;
using HikingQuests.Server.Domain.ValueObjects;
using HikingQuests.Server.Infrastructure;
using Moq;

namespace HikingQuests.Tests.Infrastructure.PersistanceTests
{
    public class QuestWorkflowServiceUnitTests
    {
        private readonly QuestWorkflowService _questWorkflowService;
        private readonly Mock<IQuestRepository> _mockQuestRepository;

        public QuestWorkflowServiceUnitTests()
        {
            _mockQuestRepository = new Mock<IQuestRepository>();
            _questWorkflowService = new QuestWorkflowService(_mockQuestRepository.Object);
        }

        [Fact]
        public async Task StartQuestAsync_Should_Call_StartQuestAsync_And_Persist_Update()
        {
            // ARRANGE
            var questId = Guid.NewGuid();
            var plannedQuest = new QuestItem("New Quest", "Desc");

            _mockQuestRepository.Setup(r => r.GetByIdAsync(questId))
                .ReturnsAsync(plannedQuest);

            // ACT
            await _questWorkflowService.StartQuestAsync(questId);

            // ASSERT
            _mockQuestRepository.Verify(r => r.GetByIdAsync(questId), Times.Once);

            Assert.Equal(QuestStatus.InProgress, plannedQuest.Status);

            _mockQuestRepository.Verify(r => r.UpdateAsync(plannedQuest), Times.Once);
        }

        [Fact]
        public async Task StartQuestAsync_Should_Propagate_KeyNotFoundException_When_Quest_Is_Missing()
        {
            // ARRANGE
            var questId = Guid.NewGuid();

            _mockQuestRepository.Setup(r => r.GetByIdAsync(questId))
                .ThrowsAsync(new KeyNotFoundException(QuestMessages.QuestNotFound));

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await _questWorkflowService.StartQuestAsync(questId));

            Assert.Contains(QuestMessages.QuestNotFound, exception.Message);

            _mockQuestRepository.Verify(r => r.UpdateAsync(It.IsAny<QuestItem>()), Times.Never);
        }

        [Fact]
        public async Task StartQuestAsync_Should_Propagate_InvalidOperationException_When_Already_InProgress()
        {
            // ARRANGE
            var questId = Guid.NewGuid();

            var inProgressQuest = new QuestItem("Existing", "Desc");
            inProgressQuest.StartQuest();

            _mockQuestRepository.Setup(r => r.GetByIdAsync(questId))
                .ReturnsAsync(inProgressQuest);

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _questWorkflowService.StartQuestAsync(questId));

            Assert.Contains(QuestMessages.QuestAlreadyInProgress, exception.Message);

            _mockQuestRepository.Verify(r => r.UpdateAsync(It.IsAny<QuestItem>()), Times.Never);
        }

        [Fact]
        public async Task StartQuestAsync_Should_Propagate_InvalidOperationException_When_Already_Completed()
        {
            // ARRANGE
            var questId = Guid.NewGuid();

            var completedQuest = new QuestItem("Completed", "Desc");
            completedQuest.StartQuest();
            completedQuest.CompleteQuest();

            _mockQuestRepository.Setup(r => r.GetByIdAsync(questId))
                .ReturnsAsync(completedQuest);

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _questWorkflowService.StartQuestAsync(questId));

            Assert.Contains(QuestMessages.QuestAlreadyCompleted, exception.Message);

            _mockQuestRepository.Verify(r => r.UpdateAsync(It.IsAny<QuestItem>()), Times.Never);
        }

        [Fact]
        public async Task CompleteQuestAsync_Should_Call_CompleteQuest_And_PersistUpdate()
        {
            // ARRANGE
            var questId = Guid.NewGuid();

            var StartedQuest = new QuestItem("Started Quest", "Desc");
            StartedQuest.StartQuest();

            _mockQuestRepository.Setup(r => r.GetByIdAsync(questId))
                .ReturnsAsync(StartedQuest);

            // ACT
            await _questWorkflowService.CompleteQuestAsync(questId);

            // ASSERT
                _mockQuestRepository.Verify(r => r.GetByIdAsync(questId), Times.Once);

            Assert.Equal(QuestStatus.Completed, StartedQuest.Status);

            _mockQuestRepository.Verify(r => r.UpdateAsync(StartedQuest), Times.Once);
        }

        [Fact]
        public async Task CompleteQuestAsync_Should_Propagate_KeyNotFoundException_When_Quest_Is_Missing()
        {
            // ARRANGE
            var questId = Guid.NewGuid();

            _mockQuestRepository.Setup(r => r.GetByIdAsync(questId))
                .ThrowsAsync(new KeyNotFoundException(QuestMessages.QuestNotFound));

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await _questWorkflowService.CompleteQuestAsync(questId));

            Assert.Contains(QuestMessages.QuestNotFound, exception.Message);

            _mockQuestRepository.Verify(r => r.UpdateAsync(It.IsAny<QuestItem>()), Times.Never);
        }

        [Fact]
        public async Task CompleteQuestAsync_Should_Propagate_InvalidOperationException_When_Not_InProgress()
        {
            // ARRANGE
            var questId = Guid.NewGuid();

            var plannedQuest = new QuestItem("Planned", "Desc");

            _mockQuestRepository.Setup(r => r.GetByIdAsync(questId))
                .ReturnsAsync(plannedQuest);

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _questWorkflowService.CompleteQuestAsync(questId));

            Assert.Contains(QuestMessages.QuestNotInProgress, exception.Message);

            _mockQuestRepository.Verify(r => r.UpdateAsync(It.IsAny<QuestItem>()), Times.Never);
        }

        [Fact]
        public async Task CompleteQuestAsync_Should_Propagate_InvalidOperationException_When_Already_Completed()
        {
            // ARRANGE
            var questId = Guid.NewGuid();

            var completedQuest = new QuestItem("Completed", "Desc");
            completedQuest.StartQuest();
            completedQuest.CompleteQuest();

            _mockQuestRepository.Setup(r => r.GetByIdAsync(questId))
                .ReturnsAsync(completedQuest);

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _questWorkflowService.CompleteQuestAsync(questId));

            Assert.Contains(QuestMessages.QuestAlreadyCompleted, exception.Message);

            _mockQuestRepository.Verify(r => r.UpdateAsync(It.IsAny<QuestItem>()), Times.Never);
        }
    }
}
