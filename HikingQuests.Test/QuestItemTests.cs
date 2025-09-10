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
            var title = string.Empty;
            var description = string.Empty;

            var questItem = new QuestItem(title, description);

            Assert.Equal(QuestStatus.Planned, questItem.Status);
        }

        [Fact]
        public void QuestItem_Title_Is_Set_Correctly()
        {
            var title = template_title;
            var description = string.Empty;

            var questItem = new QuestItem(title, description);

            Assert.Equal(title, questItem.Title);
        }

        [Fact]
        public void QuestItem_Description_Is_Set_Correctly()
        {
            var title = template_title;
            var description = template_description;

            var questItem = new QuestItem(title, description);

            Assert.Equal(description, questItem.Description);
        }

        [Fact]
        public void QuestItem_Status_Can_Be_Updated()
        {
            var questItem = new QuestItem(template_title, template_description);

            questItem.StartQuest();

            Assert.Equal(QuestStatus.InProgress, questItem.Status);
        }

        [Fact]
        public void QuestItem_Can_Be_Marked_As_Completed()
        {
            var questItem = new QuestItem(template_title, template_description);

            questItem.StartQuest();
            questItem.CompleteQuest();

            Assert.Equal(QuestStatus.Completed, questItem.Status);
        }

        [Fact]
        public void QuestItem_Cannot_Be_Completed_Without_Starting()
        {
            var questItem = new QuestItem(template_title, template_description);
            
            Assert.Throws<InvalidOperationException>(() => questItem.CompleteQuest());
        }
    }
}