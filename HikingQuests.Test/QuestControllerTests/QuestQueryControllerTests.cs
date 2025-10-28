using HikingQuests.Server.Constants;
using HikingQuests.Server.Controllers;
using HikingQuests.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HikingQuests.Tests.QuestControllerTests
{
    public class QuestQueryControllerTests
    {
        private IEnumerable<QuestItem> GetQuestsFromActionResult(IActionResult actionResult)
        {
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            return Assert.IsAssignableFrom<IEnumerable<QuestItem>>(okResult.Value);
        }

        [Fact]
        public void GetQuests_Returns_All_Quests()
        {
            var mockQuestLog = new Mock<IQuestQueryService>();

            mockQuestLog
                .Setup(q => q.GetAllQuestItems())
                .Returns(new List<QuestItem>{
                new QuestItem("Quest A", "Description A"),
                new QuestItem("Quest B", "Description B"),
                new QuestItem("Quest C", "Description C")
            });

            var controller = new QuestQueryController(mockQuestLog.Object);
            var result = controller.GetQuests();

            var quests = GetQuestsFromActionResult(result);
            Assert.Equal(3, quests.Count());
            mockQuestLog.Verify(q => q.GetAllQuestItems(), Times.Once);
        }

        [Fact]
        public void GetQuests_Returns_Empty_List_When_No_Quests()
        {
            var mockQuestLog = new Mock<IQuestQueryService>();

            mockQuestLog
                .Setup(q => q.GetAllQuestItems())
                .Returns(new List<QuestItem>());

            var controller = new QuestQueryController(mockQuestLog.Object);
            var result = controller.GetQuests();

            var quests = GetQuestsFromActionResult(result);
            Assert.Empty(quests);
        }

        [Fact]
        public void GetQuests_Throws_Exception_When_QuestLog_Fails()
        {
            var mockQuestLog = new Mock<IQuestQueryService>();
            mockQuestLog
                .Setup(q => q.GetAllQuestItems())
                .Throws(new Exception("Database error"));
            var controller = new QuestQueryController(mockQuestLog.Object);

            var exception = Assert.Throws<Exception>(() => controller.GetQuests());
            Assert.Equal("Database error", exception.Message);
        }

        [Fact]
        public void GetQuests_Calls_GetAllQuestItems_Once()
        {
            var mockQuestLog = new Mock<IQuestQueryService>();
            mockQuestLog
                .Setup(q => q.GetAllQuestItems())
                .Returns(new List<QuestItem>());

            var controller = new QuestQueryController(mockQuestLog.Object);
            var result = controller.GetQuests();
            mockQuestLog.Verify(q => q.GetAllQuestItems(), Times.Once);
        }

        [Fact]
        public void GetQuests_Returns_Correct_Quest_Titles()
        {
            var mockQuestLog = new Mock<IQuestQueryService>();
            mockQuestLog
                .Setup(q => q.GetAllQuestItems())
                .Returns(new List<QuestItem>{
                    new QuestItem("Quest A", "Description A"),
                    new QuestItem("Quest B", "Description B"),
                    new QuestItem("Quest C", "Description C")
                });


            var controller = new QuestQueryController(mockQuestLog.Object);
            var result = controller.GetQuests();
            var quests = GetQuestsFromActionResult(result);
            var titles = quests.Select(q => q.Title).ToList();

            Assert.Contains("Quest A", titles);
            Assert.Contains("Quest B", titles);
            Assert.Contains("Quest C", titles);
        }

        [Fact]
        public void GetQuestById_Returns_Correct_Quest()
        {
            var mockQuestLog = new Mock<IQuestQueryService>();
            var mockedQuestItem = new QuestItem("Quest A", "Description A");
            var controller = new QuestQueryController(mockQuestLog.Object);

            mockQuestLog
                .Setup(q => q.GetQuestById(mockedQuestItem.Id))
                .Returns(mockedQuestItem);


            var result = controller.GetQuestItemById(mockedQuestItem.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var questItem = Assert.IsType<QuestItem>(okResult.Value);

            Assert.Equal(mockedQuestItem.Id, questItem.Id);
            Assert.Equal("Quest A", questItem.Title);
            Assert.Equal("Description A", questItem.Description);

            mockQuestLog.Verify(q => q.GetQuestById(questItem.Id), Times.Once);
        }

        [Fact]
        public void GetQuestById_Throws_KeyNotFoundException_For_Invalid_Id()
        {
            var mockQuestLog = new Mock<IQuestQueryService>();
            var invalidId = Guid.NewGuid();
            var controller = new QuestQueryController(mockQuestLog.Object);

            mockQuestLog
                .Setup(q => q.GetQuestById(invalidId))
                .Throws(new KeyNotFoundException(QuestMessages.QuestNotFound));

            var exception = Assert.Throws<KeyNotFoundException>(() => controller.GetQuestItemById(invalidId));
            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);

            mockQuestLog.Verify(q => q.GetQuestById(invalidId), Times.Once);
        }
    }
}
