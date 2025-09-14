using HikingQuests.Server.Controllers;
using HikingQuests.Server.Dtos;
using HikingQuests.Server.Models;
using HikingQuests.Server.Constants;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;

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

        [Fact]
        public void UpdateQuest_Correctly_Updates_Quest_Title_Only()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var existingId = Guid.NewGuid();
            var controller = new QuestController(mockQuestLog.Object);

            var updateQuestDto = new UpdateQuestDto { Title = "Updated Title" };

            var result = controller.UpdateQuest(existingId, updateQuestDto);


            var noContentResult = Assert.IsType<NoContentResult>(result);
            mockQuestLog.Verify(q => q.UpdateQuestTitle(existingId, updateQuestDto.Title), Times.Once);
            mockQuestLog.Verify(q => q.UpdateQuestDescription(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void UpdateQuest_Correctly_Updates_Quest_Description_Only()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var existingId = Guid.NewGuid();
            var controller = new QuestController(mockQuestLog.Object);

            var updateQuestDto = new UpdateQuestDto { Description = "Updated Description" };

            var result = controller.UpdateQuest(existingId, updateQuestDto);


            var noContentResult = Assert.IsType<NoContentResult>(result);
            mockQuestLog.Verify(q => q.UpdateQuestDescription(existingId, updateQuestDto.Description), Times.Once);
            mockQuestLog.Verify(q => q.UpdateQuestTitle(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void UpdateQuest_Correctly_Updates_Both_Quest_Title_And_Description()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var existingId = Guid.NewGuid();
            var controller = new QuestController(mockQuestLog.Object);

            var updateQuestDto = new UpdateQuestDto { Title = "Updated Title", Description = "Updated Description" };

            var result = controller.UpdateQuest(existingId, updateQuestDto);

            var noContentResult = Assert.IsType<NoContentResult>(result);
            mockQuestLog.Verify(q => q.UpdateQuestTitle(existingId, updateQuestDto.Title), Times.Once);
            mockQuestLog.Verify(q => q.UpdateQuestDescription(existingId, updateQuestDto.Description), Times.Once);
        }

        [Fact]
        public void UpdateQuest_Updating_Title_Only_Returns_NotFound_For_Invalid_Id()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var invalidId = Guid.NewGuid();
            var controller = new QuestController(mockQuestLog.Object);

            var updateQuestDto = new UpdateQuestDto { Title = "Updated Title" };

            mockQuestLog
                .Setup(q => q.UpdateQuestTitle(invalidId, It.IsAny<string>()))
                .Throws(new KeyNotFoundException());


            var result = controller.UpdateQuest(invalidId, updateQuestDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(QuestMessages.QuestNotFound, notFoundResult.Value);
            mockQuestLog.Verify(q => q.UpdateQuestTitle(invalidId, updateQuestDto.Title), Times.Once);
        }

        [Fact]
        public void UpdateQuest_Updating_Description_Only_Returns_NotFound_For_Invalid_Id()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var invalidId = Guid.NewGuid();
            var controller = new QuestController(mockQuestLog.Object);

            var updateQuestDto = new UpdateQuestDto { Description = "Updated Description" };

            mockQuestLog
                .Setup(q => q.UpdateQuestDescription(invalidId, It.IsAny<string>()))
                .Throws(new KeyNotFoundException());


            var result = controller.UpdateQuest(invalidId, updateQuestDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(QuestMessages.QuestNotFound, notFoundResult.Value);
            mockQuestLog.Verify(q => q.UpdateQuestDescription(invalidId, updateQuestDto.Description), Times.Once);
        }

        [Fact]
        public void UpdateQuest_Returns_BadRequest_When_No_Fields_To_Update()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var existingId = Guid.NewGuid();
            var controller = new QuestController(mockQuestLog.Object);

            var updateQuestDto = new UpdateQuestDto { };
            
            var result = controller.UpdateQuest(existingId, updateQuestDto);
            
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(QuestMessages.NothingToUpdate, badRequestResult.Value);
        }

        [Fact]
        public void UpdateQuest_Having_More_Than_100_Characters_In_Title_Returns_BadRequest()
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

            var result = controller.UpdateQuest(existingId, updateQuestDto);
            
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            var serializableError = badRequestResult.Value as SerializableError;
            Assert.NotNull(serializableError);

            var error = serializableError
                .SelectMany(kvp => kvp.Value as string[] ?? Array.Empty<string>())
                .FirstOrDefault();

            Assert.Equal(QuestMessages.TitleTooLong, error);
        }

        [Fact]
        public void UpdateQuest_Having_More_Than_500_Characters_In_Description_Returns_BadRequest()
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

            var result = controller.UpdateQuest(existingId, updateQuestDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            var serializableError = badRequestResult.Value as SerializableError;
            Assert.NotNull(serializableError);

            var error = serializableError
                .SelectMany(kvp => kvp.Value as string[] ?? Array.Empty<string>())
                .FirstOrDefault();

            Assert.Equal(QuestMessages.DescriptionTooLong, error);
        }

        [Fact]
        public void StartQuest_Calls_QuestLog_StartQuest_And_Returns_NoContent()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var questId = Guid.NewGuid();
            var controller = new QuestController(mockQuestLog.Object);

            var result = controller.StartQuest(questId);

            var noContentResult = Assert.IsType<NoContentResult>(result);
            mockQuestLog.Verify(q => q.StartQuest(questId), Times.Once());
        }

        [Fact]
        public void StartQuest_Returns_NotFound_When_Quest_Does_Not_Exist()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var invalidId = Guid.NewGuid();
            mockQuestLog.Setup(q => q.StartQuest(invalidId))
                         .Throws(new KeyNotFoundException());
            var controller = new QuestController(mockQuestLog.Object);

            var result = controller.StartQuest(invalidId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(QuestMessages.QuestNotFound, notFoundResult.Value);
        }

        [Fact]
        public void StartQuest_Returns_Conflict_When_Quest_Already_In_Progress()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var questId = Guid.NewGuid();

            mockQuestLog.Setup(q => q.StartQuest(questId))
                .Throws(new InvalidOperationException(QuestMessages.QuestAlreadyInProgress));

            var controller = new QuestController(mockQuestLog.Object);

            var result = controller.StartQuest(questId);

            var conflictResult = Assert.IsType<ConflictObjectResult>(result);

            Assert.Equal(QuestMessages.QuestAlreadyInProgress, conflictResult.Value);
        }

        [Fact]
        public void StartQuest_Returns_Conflict_When_Quest_Already_Completed()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var questId = Guid.NewGuid();

            mockQuestLog.Setup(q => q.StartQuest(questId))
                .Throws(new InvalidOperationException(QuestMessages.QuestAlreadyCompleted));

            var controller = new QuestController(mockQuestLog.Object);

            var result = controller.StartQuest(questId);

            var conflictResult = Assert.IsType<ConflictObjectResult>(result);

            Assert.Equal(QuestMessages.QuestAlreadyCompleted, conflictResult.Value);
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
        public void CompleteQuest_Returns_NotFound_When_Quest_Does_Not_Exist()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var invalidId = Guid.NewGuid();
            mockQuestLog.Setup(q => q.CompleteQuest(invalidId))
                         .Throws(new KeyNotFoundException());
            var controller = new QuestController(mockQuestLog.Object);

            var result = controller.CompleteQuest(invalidId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(QuestMessages.QuestNotFound, notFoundResult.Value);
        }

        [Fact]
        public void CompleteQuest_Returns_Conflict_When_Quest_Not_In_Progress()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var questId = Guid.NewGuid();

            mockQuestLog.Setup(q => q.CompleteQuest(questId))
                .Throws(new InvalidOperationException(QuestMessages.QuestNotInProgress));

            var controller = new QuestController(mockQuestLog.Object);

            var result = controller.CompleteQuest(questId);

            var conflictResult = Assert.IsType<ConflictObjectResult>(result);

            Assert.Equal(QuestMessages.QuestNotInProgress, conflictResult.Value);
        }

        [Fact]
        public void CompleteQuest_Returns_Conflict_When_Quest_Already_Completed()
        {
            var mockQuestLog = new Mock<IQuestLog>();
            var questId = Guid.NewGuid();

            mockQuestLog.Setup(q => q.CompleteQuest(questId))
                .Throws(new InvalidOperationException(QuestMessages.QuestAlreadyCompleted));

            var controller = new QuestController(mockQuestLog.Object);

            var result = controller.CompleteQuest(questId);

            var conflictResult = Assert.IsType<ConflictObjectResult>(result);

            Assert.Equal(QuestMessages.QuestAlreadyCompleted, conflictResult.Value);
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
            var invalidId = Guid.NewGuid();
            mockQuestLog.Setup(q => q.DeleteQuest(invalidId))
                         .Throws(new KeyNotFoundException());
            var controller = new QuestController(mockQuestLog.Object);

            var result = controller.DeleteQuest(invalidId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(QuestMessages.QuestNotFound, notFoundResult.Value);
        }
    }
}
