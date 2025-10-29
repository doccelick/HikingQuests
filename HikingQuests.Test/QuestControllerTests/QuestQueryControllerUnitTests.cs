using HikingQuests.Server.Application.Interfaces;
using HikingQuests.Server.Constants;
using HikingQuests.Server.Controllers;
using HikingQuests.Server.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HikingQuests.Tests.QuestControllerTests
{
    public class QuestQueryControllerUnitTests
    {
        private readonly Mock<IQuestQueryService> _mockQuestQueryService;
        private readonly QuestQueryController _controller;

        public QuestQueryControllerUnitTests()
        {
            _mockQuestQueryService = new Mock<IQuestQueryService>();            
            _controller = new QuestQueryController(_mockQuestQueryService.Object);
        }

        [Fact]
        public async Task GetAllQuestsAsync_Returns_All_Quests()
        {
            // Arrange
            var expectedQuests = new List<QuestItem>{
                new QuestItem("Quest A", "Description A"),
                new QuestItem("Quest B", "Description B"),
                new QuestItem("Quest C", "Description C")
            };

            _mockQuestQueryService
                .Setup(q => q.GetAllQuestsAsync())
                .Returns(Task.FromResult<IEnumerable<QuestItem>>(expectedQuests));

            // ACT
            var result = await _controller.GetAllQuestsAsync();

            // ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result);
            var quests = Assert.IsAssignableFrom<IEnumerable<QuestItem>>(okResult.Value);

            Assert.Equal(3, quests.Count());
            _mockQuestQueryService.Verify(q => q.GetAllQuestsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllQuestsAsync_Returns_Empty_List_When_No_Quests()
        {
            // ARRANGE
            var expectedQuests = new List<QuestItem>{};

            _mockQuestQueryService
                .Setup(q => q.GetAllQuestsAsync())
                .Returns(Task.FromResult<IEnumerable<QuestItem>>(expectedQuests));

            // ACT
            var result = await _controller.GetAllQuestsAsync();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var quests = Assert.IsAssignableFrom<IEnumerable<QuestItem>>(okResult.Value);
            Assert.Empty(quests);
            _mockQuestQueryService.Verify(q => q.GetAllQuestsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllQuestsAsync_Throws_Exception_When_QuestLog_Fails()
        {
            // ARRANGE
            _mockQuestQueryService
                .Setup(q => q.GetAllQuestsAsync())
                .ThrowsAsync(new Exception("Database error"));
            
            //ACT & ASSERT
            var exception = await Assert.ThrowsAsync<Exception>(
                async () => await _controller.GetAllQuestsAsync());
            Assert.Equal("Database error", exception.Message);

            _mockQuestQueryService.Verify(q => q.GetAllQuestsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllQuestItemsAsync_Calls_GetAllQuestItems_Once()
        {            
            // ARRANGE
            var expectedQuests = new List<QuestItem> { };

            _mockQuestQueryService
                .Setup(q => q.GetAllQuestsAsync())
                 .Returns(Task.FromResult<IEnumerable<QuestItem>>(expectedQuests));

            // ACT
            var result = await _controller.GetAllQuestsAsync();
            
            // ASSERT

            _mockQuestQueryService.Verify(q => q.GetAllQuestsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllQuestsAsync_Returns_Correct_Quest_Titles()
        {
            // ARRANGE
            var expectedQuests = new List<QuestItem>{
                new QuestItem("Quest A", "Description A"),
                new QuestItem("Quest B", "Description B"),
                new QuestItem("Quest C", "Description C")
            };

            _mockQuestQueryService
                .Setup(q => q.GetAllQuestsAsync())
                .Returns(Task.FromResult<IEnumerable<QuestItem>>(expectedQuests));

            // ACT
            var result = await _controller.GetAllQuestsAsync();

            // ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result);
            var quests = Assert.IsAssignableFrom<IEnumerable<QuestItem>>(okResult.Value);
            var titles = quests.Select(q => q.Title).ToList();

            Assert.Contains("Quest A", titles);
            Assert.Contains("Quest B", titles);
            Assert.Contains("Quest C", titles);
        }

        [Fact]
        public async Task GetQuestByIdAsync_Returns_Correct_Quest()
        {
            // ARRANGE
            var mockedQuestItem = new QuestItem("Quest A", "Description A");
            
            _mockQuestQueryService
                .Setup(q => q.GetQuestByIdAsync(mockedQuestItem.Id))
                .Returns(Task.FromResult(mockedQuestItem));

            // ACT
            var result = await _controller.GetQuestItemByIdAsync(mockedQuestItem.Id);

            // ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result);
            var questItem = Assert.IsType<QuestItem>(okResult.Value);

            Assert.Equal(mockedQuestItem.Id, questItem.Id);
            Assert.Equal("Quest A", questItem.Title);
            Assert.Equal("Description A", questItem.Description);

            _mockQuestQueryService.Verify(q => q.GetQuestByIdAsync(questItem.Id), Times.Once);
        }

        [Fact]
        public async Task GetQuestByIdAsync_Throws_KeyNotFoundException_For_Invalid_Id()
        {
            // ARRANGE
            var invalidId = Guid.NewGuid();
            
            _mockQuestQueryService
                .Setup(q => q.GetQuestByIdAsync(invalidId))
                .ThrowsAsync(new KeyNotFoundException(QuestMessages.QuestNotFound));

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await _controller.GetQuestItemByIdAsync(invalidId));
            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);

            _mockQuestQueryService.Verify(q => q.GetQuestByIdAsync(invalidId), Times.Once);
        }
    }
}
