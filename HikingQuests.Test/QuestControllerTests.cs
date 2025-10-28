using HikingQuests.Server.Controllers;
using HikingQuests.Server.Dtos;
using HikingQuests.Server.Models;
using HikingQuests.Server.Constants;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace HikingQuests.Tests
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
        public void GetQuestById_Throws_KeyNotFoundException_For_Invalid_Id()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var invalidId = Guid.NewGuid();
            var controller = new QuestController(mockQuestLog.Object);

            mockQuestLog
                .Setup(q => q.GetQuestById(invalidId))
                .Throws(new KeyNotFoundException(QuestMessages.QuestNotFound));

            var exception = Assert.Throws<KeyNotFoundException>(() => controller.GetQuestItemById(invalidId));
            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);

            mockQuestLog.Verify(q => q.GetQuestById(invalidId), Times.Once);
        }

        [Fact]
        public void AddQuest_Adds_Quest_Successfully()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var controller = new QuestController(mockQuestLog.Object);

            var newQuest = new QuestItem("New Quest", "New Description");

            mockQuestLog
                .Setup(q => q.AddQuest(It.IsAny<QuestItem>()))
                .Returns((QuestItem q) => q);

            var result = controller.AddQuest(newQuest);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedQuest = Assert.IsType<QuestItem>(createdAtActionResult.Value);
            Assert.Equal(newQuest.Id, returnedQuest.Id);

            mockQuestLog.Verify(q => q.AddQuest(It.IsAny<QuestItem>()), Times.Once);
        }

        [Fact]
        public void AddQuest_Returns_ArgumentNullException_When_Quest_Is_Null()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var controller = new QuestController(mockQuestLog.Object);

            mockQuestLog
                .Setup(q => q.AddQuest(It.IsAny<QuestItem>()))
                .Throws(new ArgumentNullException(nameof(QuestItem), QuestMessages.QuestItemCannotBeNull));

            var exception = Assert.Throws<ArgumentNullException>(() => controller.AddQuest(null!));

            Assert.StartsWith(QuestMessages.QuestItemCannotBeNull, exception.Message);
            Assert.Equal(nameof(QuestItem), exception.ParamName);

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
                .Throws(new InvalidOperationException(QuestMessages.QuestAlreadyExistsInLog)
                );

            var exception = Assert.Throws<InvalidOperationException>(() => controller.AddQuest(existingQuest));
            Assert.Equal(QuestMessages.QuestAlreadyExistsInLog, exception.Message);

            mockQuestLog.Verify(q => q.AddQuest(existingQuest), Times.Once);
        }


        [Fact]
        public void UpdateQuest_Updating_Title_Only_Throws_KeyNotFoundException_For_Invalid_Id()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var invalidId = Guid.NewGuid();
            var controller = new QuestController(mockQuestLog.Object);

            var updateQuestDto = new UpdateQuestDto { Title = "Updated Title" };

            mockQuestLog
                .Setup(q => q.UpdateQuestTitle(invalidId, It.IsAny<string>()))
                .Throws(new KeyNotFoundException(QuestMessages.QuestNotFound));

            var exception = Assert.Throws<KeyNotFoundException>(() =>
                controller.UpdateQuest(invalidId, updateQuestDto));

            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);

            mockQuestLog.Verify(q => q.UpdateQuestTitle(invalidId, updateQuestDto.Title), Times.Once);
        }

        [Fact]
        public void UpdateQuest_Throws_KeyNotFoundException_For_Invalid_Id()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var invalidId = Guid.NewGuid();
            var controller = new QuestController(mockQuestLog.Object);

            var updateQuestDto = new UpdateQuestDto { Description = "Updated Description" };

            mockQuestLog
                .Setup(q => q.UpdateQuestDescription(invalidId, It.IsAny<string>()))
                .Throws(new KeyNotFoundException(QuestMessages.QuestNotFound));

            var exception = Assert.Throws<KeyNotFoundException>(() =>
                controller.UpdateQuest(invalidId, updateQuestDto));

            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);

            mockQuestLog.Verify(q => q.UpdateQuestDescription(invalidId, updateQuestDto.Description), Times.Once);
        }


        [Fact]
        public void UpdateQuest_Throws_ArgumentException_When_No_Fields_To_Update()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var existingId = Guid.NewGuid();
            var controller = new QuestController(mockQuestLog.Object);

            var updateQuestDto = new UpdateQuestDto { };

            var exception = Assert.Throws<ArgumentException>(() =>
                controller.UpdateQuest(existingId, updateQuestDto));

            Assert.Equal(QuestMessages.NothingToUpdate, exception.Message);
        }


        [Fact]
        public void UpdateQuest_Title_Longer_Than_100_Characters_Throws_ArgumentException()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var existingId = Guid.NewGuid();
            var controller = new QuestController(mockQuestLog.Object);

            var longTitle = new string('A', 101);
            var updateQuestDto = new UpdateQuestDto { Title = longTitle };

            var validationContext = new ValidationContext(updateQuestDto);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(updateQuestDto, validationContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(
                    validationResult.MemberNames.FirstOrDefault() ?? string.Empty,
                    validationResult.ErrorMessage ?? string.Empty
                );
            }

            var exception = Assert.Throws<ArgumentException>(() =>
                controller.UpdateQuest(existingId, updateQuestDto));

            Assert.Contains(QuestMessages.TitleTooLong, exception.Message);
        }


        [Fact]
        public void UpdateQuest_Description_Longer_Than_500_Characters_Throws_ArgumentException()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var existingId = Guid.NewGuid();
            var controller = new QuestController(mockQuestLog.Object);

            var longDescription = new string('A', 501);
            var updateQuestDto = new UpdateQuestDto { Description = longDescription };

            var validationContext = new ValidationContext(updateQuestDto);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(updateQuestDto, validationContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(
                    validationResult.MemberNames.FirstOrDefault() ?? string.Empty,
                    validationResult.ErrorMessage ?? string.Empty
                );
            }

            var exception = Assert.Throws<ArgumentException>(() =>
                controller.UpdateQuest(existingId, updateQuestDto));

            Assert.Contains(QuestMessages.DescriptionTooLong, exception.Message);
        }


        [Fact]
        public void StartQuest_Calls_QuestLog_StartQuest_And_Returns_UpdatedQuest()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var questId = Guid.NewGuid();
            var updatedQuest = new QuestItem("Quest 1", "Description 1");

            mockQuestLog.Setup(q => q.StartQuest(questId))
                         .Returns(updatedQuest);

            var controller = new QuestController(mockQuestLog.Object);

            var result = controller.StartQuest(questId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedQuest = Assert.IsType<QuestItem>(okResult.Value);

            Assert.Equal(updatedQuest.Id, returnedQuest.Id);
            mockQuestLog.Verify(q => q.StartQuest(questId), Times.Once());
        }

        [Fact]
        public void StartQuest_Throws_KeyNotFoundException_When_Quest_Does_Not_Exist()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var questId = Guid.NewGuid();

            mockQuestLog.Setup(q => q.StartQuest(questId))
                .Throws(new KeyNotFoundException(QuestMessages.QuestNotFound));

            var controller = new QuestController(mockQuestLog.Object);

            var exception = Assert.Throws<KeyNotFoundException>(() => controller.StartQuest(questId));
            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);

            mockQuestLog.Verify(q => q.StartQuest(questId), Times.Once);
        }

        [Fact]
        public void StartQuest_Throws_InvalidOperationException_When_Quest_Already_In_Progress()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var questId = Guid.NewGuid();

            mockQuestLog.Setup(q => q.StartQuest(questId))
                .Throws(new InvalidOperationException(QuestMessages.QuestAlreadyInProgress));

            var controller = new QuestController(mockQuestLog.Object);

            var exception = Assert.Throws<InvalidOperationException>(() => controller.StartQuest(questId));
            Assert.Equal(QuestMessages.QuestAlreadyInProgress, exception.Message);

            mockQuestLog.Verify(q => q.StartQuest(questId), Times.Once);
        }

        [Fact]
        public void StartQuest_Throws_InvalidOperationException_When_Quest_Already_Completed()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var questId = Guid.NewGuid();

            mockQuestLog
                .Setup(q => q.StartQuest(questId))
                .Throws(new InvalidOperationException(QuestMessages.QuestAlreadyCompleted));

            var controller = new QuestController(mockQuestLog.Object);

            var exception = Assert.Throws<InvalidOperationException>(() => controller.StartQuest(questId));
            Assert.Equal(QuestMessages.QuestAlreadyCompleted, exception.Message);

            mockQuestLog.Verify(q => q.StartQuest(questId), Times.Once);
        }


        [Fact]
        public void CompleteQuest_Calls_QuestLog_CompleteQuest_And_Returns_NoContent()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var questId = Guid.NewGuid();
            var controller = new QuestController(mockQuestLog.Object);

            var result = controller.CompleteQuest(questId);

            var noContentResult = Assert.IsType<NoContentResult>(result);

            mockQuestLog.Verify(q => q.CompleteQuest(questId), Times.Once());
        }

        [Fact]
        public void CompleteQuest_Returns_KeyNotFoundException_When_Quest_Does_Not_Exist()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var questId = Guid.NewGuid();

            mockQuestLog
                .Setup(q => q.CompleteQuest(questId))
                .Throws(new InvalidOperationException(QuestMessages.QuestNotFound));

            var controller = new QuestController(mockQuestLog.Object);

            var exception = Assert.Throws<InvalidOperationException>(() => controller.CompleteQuest(questId));
            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);

            mockQuestLog.Verify(q => q.CompleteQuest(questId), Times.Once);
        }

        [Fact]
        public void CompleteQuest_Throws_InvalidOperationException_When_Quest_Not_In_Progress()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var questId = Guid.NewGuid();

            mockQuestLog
                .Setup(q => q.CompleteQuest(questId))
                .Throws(new InvalidOperationException(QuestMessages.QuestNotInProgress));

            var controller = new QuestController(mockQuestLog.Object);

            var exception = Assert.Throws<InvalidOperationException>(() => controller.CompleteQuest(questId));
            Assert.Equal(QuestMessages.QuestNotInProgress, exception.Message);

            mockQuestLog.Verify(q => q.CompleteQuest(questId), Times.Once);
        }

        [Fact]
        public void CompleteQuest_Throws_InvalidOperationException_When_Quest_Already_Completed()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var questId = Guid.NewGuid();

            mockQuestLog
                .Setup(q => q.CompleteQuest(questId))
                .Throws(new InvalidOperationException(QuestMessages.QuestAlreadyCompleted));

            var controller = new QuestController(mockQuestLog.Object);

            var exception = Assert.Throws<InvalidOperationException>(() => controller.CompleteQuest(questId));
            Assert.Equal(QuestMessages.QuestAlreadyCompleted, exception.Message);

            mockQuestLog.Verify(q => q.CompleteQuest(questId), Times.Once);
        }

        [Fact]
        public void DeleteQuest_Calls_QuestLog_DeleteQuest_And_Returns_NoContent()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var questId = Guid.NewGuid();
            var controller = new QuestController(mockQuestLog.Object);

            var result = controller.DeleteQuest(questId);

            var noContentResult = Assert.IsType<NoContentResult>(result);

            mockQuestLog.Verify(q => q.DeleteQuest(questId), Times.Once());
        }

        [Fact]
        public void DeleteQuest_Returns_NotFound_When_Quest_Does_Not_Exist()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var questId = Guid.NewGuid();

            mockQuestLog
                .Setup(q => q.DeleteQuest(questId))
                .Throws(new InvalidOperationException(QuestMessages.QuestNotFound));

            var controller = new QuestController(mockQuestLog.Object);

            var exception = Assert.Throws<InvalidOperationException>(() => controller.DeleteQuest(questId));
            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);

            mockQuestLog.Verify(q => q.DeleteQuest(questId), Times.Once);
        }
    }
}
