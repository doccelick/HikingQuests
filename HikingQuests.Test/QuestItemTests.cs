using HikingQuests.Server;

namespace HikingQuests.Test
{
    public class QuestItemTests
    {
        [Fact]
        public void QuestItem_Status_Defaults_To_Planned_When_Created()
        {
            // Arrange
            var title = "5 km forest hike";

            // Act
            var questItem = new QuestItem(title, "");

            // Assert
            Assert.Equal(QuestStatus.Planned, questItem.Status);
        }

        [Fact]
        public void QuestItem_Description_Is_Set_Correctly()
        {
            // Arrange
            var title = "5 km forest hike";
            var description = "Walk 5 km following a forest path.";

            // Act
            var questItem = new QuestItem(title, description);
            
            // Assert
            Assert.Equal(description, questItem.Description);
        }
    }
}