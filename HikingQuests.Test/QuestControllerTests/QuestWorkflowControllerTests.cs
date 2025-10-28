using HikingQuests.Server.Controllers;
using HikingQuests.Server.Dtos;
using HikingQuests.Server.Models;
using HikingQuests.Server.Constants;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace HikingQuests.Tests.QuestControllerTests
{
    public class QuestWorkflowControllerTests
    {
        [Fact]
        public void StartQuest_Calls_QuestLog_StartQuest_And_Returns_UpdatedQuest()
        {
            var mockQuestLog = new Mock<IQuestWorkflowService>();
            var questId = Guid.NewGuid();
            var updatedQuest = new QuestItem("Quest 1", "Description 1");

            mockQuestLog.Setup(q => q.StartQuest(questId))
                         .Returns(updatedQuest);

            var controller = new QuestWorkflowController(mockQuestLog.Object);

            var result = controller.StartQuest(questId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuest = Assert.IsType<QuestItem>(okResult.Value);

            Assert.Equal(updatedQuest.Id, returnedQuest.Id);
            mockQuestLog.Verify(q => q.StartQuest(questId), Times.Once());
        }

        [Fact]
        public void StartQuest_Throws_KeyNotFoundException_When_Quest_Does_Not_Exist()
        {
            var mockQuestLog = new Mock<IQuestWorkflowService>();
            var questId = Guid.NewGuid();

            mockQuestLog.Setup(q => q.StartQuest(questId))
                .Throws(new KeyNotFoundException(QuestMessages.QuestNotFound));

            var controller = new QuestWorkflowController(mockQuestLog.Object);

            var exception = Assert.Throws<KeyNotFoundException>(() => controller.StartQuest(questId));
            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);

            mockQuestLog.Verify(q => q.StartQuest(questId), Times.Once);
        }

        [Fact]
        public void StartQuest_Throws_InvalidOperationException_When_Quest_Already_In_Progress()
        {
            var mockQuestLog = new Mock<IQuestWorkflowService>();
            var questId = Guid.NewGuid();

            mockQuestLog.Setup(q => q.StartQuest(questId))
                .Throws(new InvalidOperationException(QuestMessages.QuestAlreadyInProgress));

            var controller = new QuestWorkflowController(mockQuestLog.Object);

            var exception = Assert.Throws<InvalidOperationException>(() => controller.StartQuest(questId));
            Assert.Equal(QuestMessages.QuestAlreadyInProgress, exception.Message);

            mockQuestLog.Verify(q => q.StartQuest(questId), Times.Once);
        }

        [Fact]
        public void StartQuest_Throws_InvalidOperationException_When_Quest_Already_Completed()
        {
            var mockQuestLog = new Mock<IQuestWorkflowService>();
            var questId = Guid.NewGuid();

            mockQuestLog
                .Setup(q => q.StartQuest(questId))
                .Throws(new InvalidOperationException(QuestMessages.QuestAlreadyCompleted));

            var controller = new QuestWorkflowController(mockQuestLog.Object);

            var exception = Assert.Throws<InvalidOperationException>(() => controller.StartQuest(questId));
            Assert.Equal(QuestMessages.QuestAlreadyCompleted, exception.Message);

            mockQuestLog.Verify(q => q.StartQuest(questId), Times.Once);
        }


        [Fact]
        public void CompleteQuest_Calls_QuestLog_CompleteQuest_And_Returns_NoContent()
        {
            var mockQuestLog = new Mock<IQuestWorkflowService>();
            var questId = Guid.NewGuid();
            var controller = new QuestWorkflowController(mockQuestLog.Object);

            var result = controller.CompleteQuest(questId);

            var noContentResult = Assert.IsType<NoContentResult>(result);

            mockQuestLog.Verify(q => q.CompleteQuest(questId), Times.Once());
        }

        [Fact]
        public void CompleteQuest_Returns_KeyNotFoundException_When_Quest_Does_Not_Exist()
        {
            var mockQuestLog = new Mock<IQuestWorkflowService>();
            var questId = Guid.NewGuid();

            mockQuestLog
                .Setup(q => q.CompleteQuest(questId))
                .Throws(new InvalidOperationException(QuestMessages.QuestNotFound));

            var controller = new QuestWorkflowController(mockQuestLog.Object);

            var exception = Assert.Throws<InvalidOperationException>(() => controller.CompleteQuest(questId));
            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);

            mockQuestLog.Verify(q => q.CompleteQuest(questId), Times.Once);
        }

        [Fact]
        public void CompleteQuest_Throws_InvalidOperationException_When_Quest_Not_In_Progress()
        {
            var mockQuestLog = new Mock<IQuestWorkflowService>();
            var questId = Guid.NewGuid();

            mockQuestLog
                .Setup(q => q.CompleteQuest(questId))
                .Throws(new InvalidOperationException(QuestMessages.QuestNotInProgress));

            var controller = new QuestWorkflowController(mockQuestLog.Object);

            var exception = Assert.Throws<InvalidOperationException>(() => controller.CompleteQuest(questId));
            Assert.Equal(QuestMessages.QuestNotInProgress, exception.Message);

            mockQuestLog.Verify(q => q.CompleteQuest(questId), Times.Once);
        }

        [Fact]
        public void CompleteQuest_Throws_InvalidOperationException_When_Quest_Already_Completed()
        {
            var mockQuestLog = new Mock<IQuestWorkflowService>();
            var questId = Guid.NewGuid();

            mockQuestLog
                .Setup(q => q.CompleteQuest(questId))
                .Throws(new InvalidOperationException(QuestMessages.QuestAlreadyCompleted));

            var controller = new QuestWorkflowController(mockQuestLog.Object);

            var exception = Assert.Throws<InvalidOperationException>(() => controller.CompleteQuest(questId));
            Assert.Equal(QuestMessages.QuestAlreadyCompleted, exception.Message);

            mockQuestLog.Verify(q => q.CompleteQuest(questId), Times.Once);
        }
    }
}
