using HikingQuests.Server.Controllers;
using HikingQuests.Server.Models;
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
                new QuestItem("Quest 1", "Description 1"),
                new QuestItem("Quest 2", "Description 2"),
                new QuestItem("Quest 3", "Description 3")
            });

            var controller = new QuestController(mockQuestLog.Object);

            var result = controller.GetQuests();

            var quests = Assert.IsAssignableFrom<IEnumerable<QuestItem>>(result);
            Assert.Equal(3, quests.Count());
        }
    }
}
