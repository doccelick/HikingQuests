using HikingQuests.Server.Constants;
using HikingQuests.Server.Controllers;
using HikingQuests.Server.Dtos;
using HikingQuests.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace HikingQuests.Tests.QuestControllerTests
{
    public class QuestManagementControllerTests
    {
        [Fact]
        public void AddQuest_Adds_Quest_Successfully()
        {
            var mockQuestLog = new Mock<IQuestManagementService>();
            var controller = new QuestManagementController(mockQuestLog.Object);

            var newQuest = new QuestItem("New Quest", "New Description");

            mockQuestLog
                .Setup(q => q.AddQuest(It.IsAny<QuestItem>()))
                .Returns((QuestItem q) => q);

            var result = controller.AddQuest(newQuest);

            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnedQuest = Assert.IsType<QuestItem>(createdAtRouteResult.Value);
            Assert.Equal(newQuest.Id, returnedQuest.Id);

            mockQuestLog.Verify(q => q.AddQuest(It.IsAny<QuestItem>()), Times.Once);
        }

        [Fact]
        public void AddQuest_Returns_ArgumentNullException_When_Quest_Is_Null()
        {
            var mockQuestLog = new Mock<IQuestManagementService>();
            var controller = new QuestManagementController(mockQuestLog.Object);

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
            var mockQuestLog = new Mock<IQuestManagementService>();
            var controller = new QuestManagementController(mockQuestLog.Object);
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
            var mockQuestLog = new Mock<IQuestManagementService>();
            var invalidId = Guid.NewGuid();
            var controller = new QuestManagementController(mockQuestLog.Object);

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
            var mockQuestLog = new Mock<IQuestManagementService>();
            var invalidId = Guid.NewGuid();
            var controller = new QuestManagementController(mockQuestLog.Object);

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
            var mockQuestLog = new Mock<IQuestManagementService>();
            var existingId = Guid.NewGuid();
            var controller = new QuestManagementController(mockQuestLog.Object);

            var updateQuestDto = new UpdateQuestDto { };

            var exception = Assert.Throws<ArgumentException>(() =>
                controller.UpdateQuest(existingId, updateQuestDto));

            Assert.Equal(QuestMessages.NothingToUpdate, exception.Message);
        }


        [Fact]
        public void UpdateQuest_Title_Longer_Than_100_Characters_Throws_ArgumentException()
        {
            var mockQuestLog = new Mock<IQuestManagementService>();
            var existingId = Guid.NewGuid();
            var controller = new QuestManagementController(mockQuestLog.Object);

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
            var mockQuestLog = new Mock<IQuestManagementService>();
            var existingId = Guid.NewGuid();
            var controller = new QuestManagementController(mockQuestLog.Object);

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
        public void DeleteQuest_Calls_QuestLog_DeleteQuest_And_Returns_NoContent()
        {
            var mockQuestLog = new Mock<IQuestManagementService>();
            var questId = Guid.NewGuid();
            var controller = new QuestManagementController(mockQuestLog.Object);

            var result = controller.DeleteQuest(questId);

            var noContentResult = Assert.IsType<NoContentResult>(result);

            mockQuestLog.Verify(q => q.DeleteQuest(questId), Times.Once());
        }

        [Fact]
        public void DeleteQuest_Returns_NotFound_When_Quest_Does_Not_Exist()
        {
            var mockQuestLog = new Mock<IQuestManagementService>();
            var questId = Guid.NewGuid();

            mockQuestLog
                .Setup(q => q.DeleteQuest(questId))
                .Throws(new InvalidOperationException(QuestMessages.QuestNotFound));

            var controller = new QuestManagementController(mockQuestLog.Object);

            var exception = Assert.Throws<InvalidOperationException>(() => controller.DeleteQuest(questId));
            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);

            mockQuestLog.Verify(q => q.DeleteQuest(questId), Times.Once);
        }
    }
}
