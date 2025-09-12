using HikingQuests.Server.Controllers;
using HikingQuests.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HikingQuests.Test
{
    public class QuestControllerTests
    {
        private IEnumerable<QuestItem> GetQuestsFromActionResult(IActionResult actionResult)
        {
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            return Assert.IsAssignableFrom<IEnumerable<QuestItem>>(okResult.Value);
        }

        [Fact]
        public void GetQuests_Returns_All_Quests()
        {
            var mockQuestLog = new Mock<IQuestLog>();

            mockQuestLog
                .Setup(q => q.GetAllQuestItems())
                .Returns(new List<QuestItem>{
                new QuestItem("Quest A", "Description A"),
                new QuestItem("Quest B", "Description B"),
                new QuestItem("Quest C", "Description C")
            });

            var controller = new QuestController(mockQuestLog.Object);
            var result = controller.GetQuests();

            var quests = GetQuestsFromActionResult(result);
            Assert.Equal(3, quests.Count());
            mockQuestLog.Verify(q => q.GetAllQuestItems(), Times.Once);
        }

        [Fact]
        public void GetQuests_Returns_Empty_List_When_No_Quests()
        {
            var mockQuestLog = new Mock<IQuestLog>();

            mockQuestLog
                .Setup(q => q.GetAllQuestItems())
                .Returns(new List<QuestItem>());

            var controller = new QuestController(mockQuestLog.Object);
            var result = controller.GetQuests();

            var quests = GetQuestsFromActionResult(result);
            Assert.Empty(quests);
        }

        [Fact]
        public void GetQuests_Throws_Exception_When_QuestLog_Fails()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            mockQuestLog
                .Setup(q => q.GetAllQuestItems())
                .Throws(new Exception("Database error"));
            var controller = new QuestController(mockQuestLog.Object);

            var exception = Assert.Throws<Exception>(() => controller.GetQuests());
            Assert.Equal("Database error", exception.Message);
        }

        [Fact]
        public void GetQuests_Calls_GetAllQuestItems_Once()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            mockQuestLog
                .Setup(q => q.GetAllQuestItems())
                .Returns(new List<QuestItem>());

            var controller = new QuestController(mockQuestLog.Object);
            var result = controller.GetQuests();
            mockQuestLog.Verify(q => q.GetAllQuestItems(), Times.Once);
        }

        [Fact]
        public void GetQuests_Returns_Correct_Quest_Titles()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            mockQuestLog
                .Setup(q => q.GetAllQuestItems())
                .Returns(new List<QuestItem>{
                    new QuestItem("Quest A", "Description A"),
                    new QuestItem("Quest B", "Description B"),
                    new QuestItem("Quest C", "Description C")
                });


            var controller = new QuestController(mockQuestLog.Object);
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
            var mockQuestLog = new Mock<IQuestLog>();
            var mockedQuestItem = new QuestItem("Quest A", "Description A");
            var controller = new QuestController(mockQuestLog.Object);

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
        public void GetQuestById_Returns_NotFound_For_Invalid_Id()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var invalidId = Guid.NewGuid();
            var controller = new QuestController(mockQuestLog.Object);

            mockQuestLog
                .Setup(q => q.GetQuestById(invalidId))
                .Throws(new KeyNotFoundException());

            var result = controller.GetQuestItemById(invalidId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(QuestMessages.QuestNotFound, notFoundResult.Value);

            mockQuestLog.Verify(q => q.GetQuestById(invalidId), Times.Once);
        }

        [Fact]
        public void AddQuest_Adds_Quest_Successfully()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var controller = new QuestController(mockQuestLog.Object);

            var newQuest = new QuestItem("New Quest", "New Description");

            var result = controller.AddQuest(newQuest);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedQuest = Assert.IsType<QuestItem>(createdAtActionResult.Value);
            Assert.Equal(newQuest.Id, returnedQuest.Id);

            mockQuestLog.Verify(q => q.AddQuest(It.IsAny<QuestItem>()), Times.Once);
        }

        [Fact]
        public void AddQuest_Returns_BadRequest_When_Quest_Is_Null()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var controller = new QuestController(mockQuestLog.Object);

            mockQuestLog
                .Setup(q => q.AddQuest(It.IsAny<QuestItem>()))
                .Throws(
                new ArgumentNullException(nameof(QuestItem)
                , QuestMessages.QuestItemCannotBeNull
                ));
            
            var result = controller.AddQuest(null!);
            
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(QuestMessages.QuestItemCannotBeNull, badRequestResult.Value);
            
            mockQuestLog.Verify(q => q.AddQuest(It.IsAny<QuestItem>()), Times.Once);
        }

        [Fact]
        public void AddQuest_Returns_Conflict_When_Quest_Already_Exists()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var controller = new QuestController(mockQuestLog.Object);
            var existingQuest = new QuestItem("Existing Quest", "Existing Description");

            mockQuestLog
                .Setup(q => q.AddQuest(existingQuest))
                .Throws(new InvalidOperationException()
                );
            
            
            var result = controller.AddQuest(existingQuest);
            
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(QuestMessages.QuestAlreadyExistsInLog, conflictResult.Value);

            mockQuestLog.Verify(q => q.AddQuest(existingQuest), Times.Once);
        }
    }
}
