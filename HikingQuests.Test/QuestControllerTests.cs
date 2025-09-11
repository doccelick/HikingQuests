using HikingQuests.Server.Controllers;
using HikingQuests.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HikingQuests.Test
{
    public class QuestControllerTests
    {
        [Fact]
        public void GetQuests_Returns_All_Quests()
        {
            var mockQuestLog = new Mock<IQuestLog>();

            mockQuestLog.Setup(q => q.GetAllQuestItems()).Returns(new List<QuestItem>
            {
                new QuestItem("Quest A", "Description A"),
                new QuestItem("Quest B", "Description B"),
                new QuestItem("Quest C", "Description C")
            });

            var controller = new QuestController(mockQuestLog.Object);

            var result = controller.GetQuests();

            var actionResult = Assert.IsType<ActionResult<IEnumerable<QuestItem>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            var quests = Assert.IsAssignableFrom<IEnumerable<QuestItem>>(okResult.Value);
            
            Assert.Equal(3, quests.Count());

            mockQuestLog.Verify(q => q.GetAllQuestItems(), Times.Once);
        }

        [Fact]
        public void GetQuests_Returns_Empty_List_When_No_Quests()
        {
            var mockQuestLog = new Mock<IQuestLog>();

            mockQuestLog.Setup(q => q.GetAllQuestItems()).Returns(new List<QuestItem>());

            var controller = new QuestController(mockQuestLog.Object);
            var result = controller.GetQuests();
            var actionResult = Assert.IsType<ActionResult<IEnumerable<QuestItem>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var quests = Assert.IsAssignableFrom<IEnumerable<QuestItem>>(okResult.Value);
            Assert.Empty(quests);
        }

        [Fact]
        public void GetQuests_Throws_Exception_When_QuestLog_Fails()
        {
            var mockQuestLog = new Mock<IQuestLog>();

            mockQuestLog.Setup(q => q.GetAllQuestItems()).Throws(new Exception("Database error"));
            var controller = new QuestController(mockQuestLog.Object);
            var exception = Assert.Throws<Exception>(() => controller.GetQuests());
            Assert.Equal("Database error", exception.Message);
        }

        [Fact]
        public void GetQuests_Calls_GetAllQuestItems_Once()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            mockQuestLog.Setup(q => q.GetAllQuestItems()).Returns(new List<QuestItem>());
            var controller = new QuestController(mockQuestLog.Object);
            var result = controller.GetQuests();
            mockQuestLog.Verify(q => q.GetAllQuestItems(), Times.Once);
        }

        [Fact]
        public void GetQuests_Returns_Correct_Quest_Titles()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            mockQuestLog.Setup(q => q.GetAllQuestItems()).Returns(new List<QuestItem>
            {
                new QuestItem("Quest A", "Description A"),
                new QuestItem("Quest B", "Description B"),
                new QuestItem("Quest C", "Description C")
            });
            var controller = new QuestController(mockQuestLog.Object);
            var result = controller.GetQuests();
            var actionResult = Assert.IsType<ActionResult<IEnumerable<QuestItem>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var quests = Assert.IsAssignableFrom<IEnumerable<QuestItem>>(okResult.Value);
            var titles = quests.Select(q => q.Title).ToList();
            Assert.Contains("Quest A", titles);
            Assert.Contains("Quest B", titles);
            Assert.Contains("Quest C", titles);
        }

        [Fact]
        public void GetQuestById_Returns_Correct_Quest_Title()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var mockedQuestItem = new QuestItem("Quest A", "Description A");

            mockQuestLog.Setup(q => q.GetQuestById(mockedQuestItem.Id)).Returns(mockedQuestItem);
            var controller = new QuestController(mockQuestLog.Object);

            var result = controller.GetQuestItemById(mockedQuestItem.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var questItem = Assert.IsType<QuestItem>(okResult.Value);

            Assert.Equal(mockedQuestItem.Id, questItem.Id);
            Assert.Equal("Quest A", questItem.Title);
            Assert.Equal("Description A", questItem.Description);

            mockQuestLog.Verify(q => q.GetQuestById(questItem.Id), Times.Once);
        }

        [Fact]
        public void GetQuestById_Returns_NotFound_For_Invalid_Id()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var invalidId = Guid.NewGuid();
            mockQuestLog.Setup(q => q.GetQuestById(invalidId)).Throws(new KeyNotFoundException());
            var controller = new QuestController(mockQuestLog.Object);
            var result = controller.GetQuestItemById(invalidId);
            Assert.IsType<NotFoundResult>(result);
            mockQuestLog.Verify(q => q.GetQuestById(invalidId), Times.Once);
        }
    }
}
