using HikingQuests.Server;

namespace HikingQuests.Test
{
    public class QuestItemTests
    {
        private string template_title = "5 km forest hike";
        private string template_description = "Walk 5 km following a forest path.";

        [Fact]
        public void QuestItem_Status_Defaults_To_Planned_When_Created()
        {
            // Arrange
            var title = string.Empty;
            var description = string.Empty;

            // Act
            var questItem = new QuestItem(title, description);

            // Assert
            Assert.Equal(QuestStatus.Planned, questItem.Status);
        }

        [Fact]
        public void QuestItem_Title_Is_Set_Correctly()
        {
            // Arrange
            var title = template_title;
            var description = string.Empty;

            // Act
            var questItem = new QuestItem(title, description);

            // Assert
            Assert.Equal(title, questItem.Title);
        }

        [Fact]
        public void QuestItem_Description_Is_Set_Correctly()
        {
            // Arrange
            var title = template_title;
            var description = template_description;

            // Act
            var questItem = new QuestItem(title, description);
            
            // Assert
            Assert.Equal(description, questItem.Description);
        }

        [Fact]
        public void QuestItem_Status_Can_Be_Updated()
        {
            // Arrange
            var title = template_title;
            var description = template_description;
            var questItem = new QuestItem(title, description);

            // Act
            questItem.StartQuest();

            // Assert
            Assert.Equal(QuestStatus.InProgress, questItem.Status);
        }

        [Fact]
        public void QuestItem_Can_Be_Marked_As_Completed()
        {
            // Arrange
            var title = template_title;
            var description = template_description;
            var questItem = new QuestItem(title, description);

            // Act
            questItem.StartQuest();
            questItem.CompleteQuest();

            // Assert
            Assert.Equal(QuestStatus.Completed, questItem.Status);
        }
    }
}