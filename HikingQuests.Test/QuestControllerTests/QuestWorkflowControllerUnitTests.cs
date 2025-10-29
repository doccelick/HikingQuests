using HikingQuests.Server.Controllers;
using HikingQuests.Server.Constants;
using Microsoft.AspNetCore.Mvc;
using Moq;
using HikingQuests.Server.Application.Interfaces;

namespace HikingQuests.Tests.QuestControllerTests
{
    public class QuestWorkflowControllerUnitTests
    {
        private readonly Mock<IQuestWorkflowService> _mockQuestWorkflowService;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly QuestWorkflowController _controller;

        public QuestWorkflowControllerUnitTests() 
        {
            _mockQuestWorkflowService = new Mock<IQuestWorkflowService>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _controller = new QuestWorkflowController(_mockQuestWorkflowService.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task StartQuestAsync_And_Calls_QuestLog_StartQuest_And_Returns_NoContent()
        {
            // ARRANGE
            var questId = Guid.NewGuid();            
            _mockQuestWorkflowService
                .Setup(s => s.StartQuestAsync(questId))
                .Returns(Task.CompletedTask);

            // ACT
            var result = await _controller.StartQuestAsync(questId);

            // ASSERT
            Assert.IsType<NoContentResult>(result);
            _mockQuestWorkflowService.Verify(s => s.StartQuestAsync(questId), Times.Once);
        }

        [Fact]
        public async Task StartQuest_Throws_KeyNotFoundException_When_Quest_Does_Not_Exist()
        {
            var questId = Guid.NewGuid();

            // ARRANGE
            _mockQuestWorkflowService
                .Setup(s => s.StartQuestAsync(questId))
                .ThrowsAsync(new KeyNotFoundException(QuestMessages.QuestNotFound));

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await _controller.StartQuestAsync(questId));

            Assert.Contains(QuestMessages.QuestNotFound, exception.Message);
            _mockQuestWorkflowService.Verify(s => s.StartQuestAsync(questId), Times.Once);
        }

        [Fact]
        public async Task StartQuest_Throws_InvalidOperationException_When_Quest_Already_In_Progress()
        {
            var questId = Guid.NewGuid();

            _mockQuestWorkflowService
                .Setup(q => q.StartQuestAsync(questId))
                .ThrowsAsync(new InvalidOperationException(QuestMessages.QuestAlreadyInProgress));

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _controller.StartQuestAsync(questId));
            Assert.Equal(QuestMessages.QuestAlreadyInProgress, exception.Message);

            _mockQuestWorkflowService.Verify(q => q.StartQuestAsync(questId), Times.Once);
        }

        [Fact]
        public async Task StartQuest_Throws_InvalidOperationException_When_Quest_Already_Completed()
        {
            var questId = Guid.NewGuid();

            _mockQuestWorkflowService
                .Setup(q => q.StartQuestAsync(questId))
                .ThrowsAsync(new InvalidOperationException(QuestMessages.QuestAlreadyCompleted));

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _controller.StartQuestAsync(questId));
            Assert.Equal(QuestMessages.QuestAlreadyCompleted, exception.Message);

            _mockQuestWorkflowService.Verify(q => q.StartQuestAsync(questId), Times.Once);
        }


        [Fact]
        public async Task CompleteQuest_Calls_QuestLog_CompleteQuest_And_Returns_NoContent()
        {
            var questId = Guid.NewGuid();

            // ARRANGE
            _mockQuestWorkflowService
                .Setup(s => s.CompleteQuestAsync(questId))
                .Returns(Task.CompletedTask);

            var result = await _controller.CompleteQuestAsync(questId);

            var noContentResult = Assert.IsType<NoContentResult>(result);

            _mockQuestWorkflowService.Verify(q => q.CompleteQuestAsync(questId), Times.Once());
        }

        [Fact]
        public async Task CompleteQuest_Returns_KeyNotFoundException_When_Quest_Does_Not_Exist()
        {
            var questId = Guid.NewGuid();

            _mockQuestWorkflowService
                .Setup(q => q.CompleteQuestAsync(questId))
                .ThrowsAsync(new KeyNotFoundException(QuestMessages.QuestNotFound));

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await _controller.CompleteQuestAsync(questId));
            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);

            _mockQuestWorkflowService.Verify(q => q.CompleteQuestAsync(questId), Times.Once);
        }

        [Fact]
        public async Task CompleteQuest_Throws_InvalidOperationException_When_Quest_Not_In_Progress()
        {
            var questId = Guid.NewGuid();

            _mockQuestWorkflowService
                .Setup(q => q.CompleteQuestAsync(questId))
                .ThrowsAsync(new InvalidOperationException(QuestMessages.QuestNotInProgress));

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _controller.CompleteQuestAsync(questId));
            Assert.Equal(QuestMessages.QuestNotInProgress, exception.Message);

            _mockQuestWorkflowService.Verify(q => q.CompleteQuestAsync(questId), Times.Once);
        }

        [Fact]
        public async Task CompleteQuest_Throws_InvalidOperationException_When_Quest_Already_Completed()
        {
            var questId = Guid.NewGuid();

            _mockQuestWorkflowService
                .Setup(q => q.CompleteQuestAsync(questId))
                .ThrowsAsync(new InvalidOperationException(QuestMessages.QuestAlreadyCompleted));

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _controller.CompleteQuestAsync(questId));
            Assert.Equal(QuestMessages.QuestAlreadyCompleted, exception.Message);

            _mockQuestWorkflowService.Verify(q => q.CompleteQuestAsync(questId), Times.Once);
        }
    }
}
