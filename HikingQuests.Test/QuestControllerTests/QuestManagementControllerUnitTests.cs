using HikingQuests.Server.Application.Dtos;
using HikingQuests.Server.Application.Interfaces;
using HikingQuests.Server.Constants;
using HikingQuests.Server.Controllers;
using HikingQuests.Server.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HikingQuests.Tests.QuestControllerTests
{
    public class QuestManagementControllerUnitTests
    {
        private readonly Mock<IQuestManagementService> _mockQuestManagementService;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly QuestManagementController _controller;

        public QuestManagementControllerUnitTests()
        {
            _mockQuestManagementService = new Mock<IQuestManagementService>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _controller = new QuestManagementController(_mockQuestManagementService.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task AddQuest_Adds_Quest_Successfully()
        {
            // ARRANGE
            var newQuest = new QuestItem("New Quest", "New Description");
            var addQuestDto = new AddQuestDto() { Title = newQuest.Title, Description = newQuest.Description };

            _mockQuestManagementService
                .Setup(q => q.AddQuestAsync(It.IsAny<AddQuestDto>()))
                .ReturnsAsync(newQuest);

            // ACT
            var result = await _controller.AddQuestAsync(addQuestDto);

            // ASSERT
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnedQuest = Assert.IsType<QuestItem>(createdAtRouteResult.Value);
            Assert.Equal(newQuest.Id, returnedQuest.Id);

            _mockQuestManagementService.Verify(q => q.AddQuestAsync(It.IsAny<AddQuestDto>()), Times.Once);
        }

        [Fact]
        public async Task AddQuest_Returns_Conflict_When_Quest_Already_Exists()
        {
            // ARRANGE
            var existingQuest = new QuestItem("Existing Quest", "Existing Description");
            var addQuestDto = new AddQuestDto() { Title = existingQuest.Title, Description = existingQuest.Description };

            _mockQuestManagementService
                .Setup(q => q.AddQuestAsync(addQuestDto))
                .ThrowsAsync(new InvalidOperationException(QuestMessages.QuestAlreadyExistsInLog)
                );

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _controller.AddQuestAsync(addQuestDto));
            Assert.Equal(QuestMessages.QuestAlreadyExistsInLog, exception.Message);

            _mockQuestManagementService.Verify(q => q.AddQuestAsync(addQuestDto), Times.Once);
        }


        [Fact]
        public async Task UpdateQuest_Updating_Title_Only_Throws_KeyNotFoundException_For_Invalid_Id()
        {
            // ARRANGE
            var invalidId = Guid.NewGuid();
            
            var updateQuestDto = new UpdateQuestDto { Title = "Updated Title" };

            _mockQuestManagementService
                .Setup(q => q.UpdateQuestAsync(invalidId, It.IsAny<UpdateQuestDto>()))
                .ThrowsAsync(new KeyNotFoundException(QuestMessages.QuestNotFound));

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await _controller.UpdateQuestAsync(invalidId, updateQuestDto));

            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);

            _mockQuestManagementService.Verify(q => q.UpdateQuestAsync(invalidId, updateQuestDto), Times.Once);
        }

        [Fact]
        public async Task UpdateQuest_Throws_KeyNotFoundException_For_Invalid_Id()
        {
            // ARRANGE
            var invalidId = Guid.NewGuid();
            
            var updateQuestDto = new UpdateQuestDto { Description = "Updated Description" };

            _mockQuestManagementService
                .Setup(q => q.UpdateQuestAsync(invalidId, It.IsAny<UpdateQuestDto>()))
                .ThrowsAsync(new KeyNotFoundException(QuestMessages.QuestNotFound));

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await _controller.UpdateQuestAsync(invalidId, updateQuestDto));

            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);

            _mockQuestManagementService.Verify(q => q.UpdateQuestAsync(invalidId, updateQuestDto), Times.Once);
        }


        [Fact]
        public async Task UpdateQuest_Throws_ArgumentException_When_No_Fields_To_Update()
        {
            // ARRANGE
            var existingId = Guid.NewGuid();
            
            var updateQuestDto = new UpdateQuestDto { };

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _controller.UpdateQuestAsync(existingId, updateQuestDto));

            Assert.Equal(QuestMessages.NothingToUpdate, exception.Message);
        }


        [Fact]
        public async Task UpdateQuest_Title_Length_Error_Propagates_ArgumentException()
        {
            // ARRANGE
            var existingId = Guid.NewGuid();            
            var updateQuestDto = new UpdateQuestDto { Title = "A long title..." };
            _controller.ModelState.AddModelError("Title", QuestMessages.TitleTooLong);

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _controller.UpdateQuestAsync(existingId, updateQuestDto));

            Assert.Contains(QuestMessages.TitleTooLong, exception.Message);

            _mockQuestManagementService.Verify(q => q.UpdateQuestAsync(It.IsAny<Guid>(), It.IsAny<UpdateQuestDto>()), Times.Never);
        }

        [Fact]
        public async Task UpdateQuest_Description_Length_Error_Propagates_ArgumentException()
        {
            // ARRANGE
            var existingId = Guid.NewGuid();            
            var updateQuestDto = new UpdateQuestDto { Description = "A long description..." };
            _controller.ModelState.AddModelError("Description", QuestMessages.DescriptionTooLong);

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _controller.UpdateQuestAsync(existingId, updateQuestDto));

            Assert.Contains(QuestMessages.DescriptionTooLong, exception.Message);

            _mockQuestManagementService.Verify(q => q.UpdateQuestAsync(It.IsAny<Guid>(), It.IsAny<UpdateQuestDto>()), Times.Never);
        }

        [Fact]
        public async Task DeleteQuest_Calls_QuestLog_DeleteQuest_And_Returns_NoContent()
        {
            // ARRANGE
            var questId = Guid.NewGuid();
            
            // ACT
            var result = await _controller.DeleteQuestAsync(questId);

            // ASSERT
            var noContentResult = Assert.IsType<NoContentResult>(result);

            _mockQuestManagementService.Verify(q => q.DeleteQuestAsync(questId), Times.Once());
        }

        [Fact]
        public async Task DeleteQuest_Returns_NotFound_When_Quest_Does_Not_Exist()
        {
            // ARRANGE
            var questId = Guid.NewGuid();

            _mockQuestManagementService
                .Setup(q => q.DeleteQuestAsync(questId))
                .Throws(new InvalidOperationException(QuestMessages.QuestNotFound));
            
            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
               async () => await _controller.DeleteQuestAsync(questId));
            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);

            _mockQuestManagementService.Verify(q => q.DeleteQuestAsync(questId), Times.Once);
        }
    }
}
