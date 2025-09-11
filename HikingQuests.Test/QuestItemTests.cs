using HikingQuests.Server.Models;

namespace HikingQuests.Test
{
    public class QuestItemTests
    {
        private string template_title = "5 km forest hike";
        private string template_description = "Walk 5 km following a forest path.";
        
        //Quest Item Title tests

        [Fact]
        public void QuestItem_Title_Is_Set_Correctly_In_Constructor()
        {
            var title = template_title;
            var description = template_description;

            var questItem = new QuestItem(title, description);

            Assert.Equal(title, questItem.Title);
        }

        [Fact]
        public void QuestItem_Title_Cannot_Be_Null_In_Constructor()
        {
            string? title = null;
            var description = string.Empty;

            var exception = Assert.Throws<ArgumentException>(() => new QuestItem(title, description));
            Assert.Equal(QuestMessages.TitleCannotBeNullOrEmpty, exception.Message);
        }

        [Fact]
        public void QuestItem_Title_Cannot_Be_Empty_In_Constructor()
        {
            var title = string.Empty;
            var description = template_description;

            var exception = Assert.Throws<ArgumentException>(() => new QuestItem(title, description));
            Assert.Equal(QuestMessages.TitleCannotBeNullOrEmpty, exception.Message);
        }


        [Fact]
        public void QuestItem_Title_Cannot_Be_Whitespace_In_Constructor()
        {
            var title = "   ";
            var description = template_description;
            var exception = Assert.Throws<ArgumentException>(() => new QuestItem(title, description));
            Assert.Equal(QuestMessages.TitleCannotBeNullOrEmpty, exception.Message);
        }

        [Fact]
        public void QuestItem_Title_Can_Be_Updated()
        {
            var questItem = new QuestItem(template_title, template_description);

            var newTitle = "10 km mountain hike";

            questItem.UpdateTitle(newTitle);

            Assert.Equal(newTitle, questItem.Title);
        }

        [Fact]
        public void QuestItem_Title_Cannot_Be_Null_When_Updated()
        {
            var questItem = new QuestItem(template_title, template_description);

            string? newTitle = null;

            var exception = Assert.Throws<ArgumentException>(() => questItem.UpdateTitle(newTitle));
            Assert.Equal(QuestMessages.TitleCannotBeNullOrEmpty, exception.Message);
        }

        [Fact]
        public void QuestItem_Title_Cannot_Be_Empty_When_Updated()
        {
            var questItem = new QuestItem(template_title, template_description);

            var newTitle = string.Empty;

            var exception = Assert.Throws<ArgumentException>(() => questItem.UpdateTitle(newTitle));
            Assert.Equal(QuestMessages.TitleCannotBeNullOrEmpty, exception.Message);
        }

        [Fact]
        public void QuestItem_Title_Cannot_Be_Whitespace_When_Updated()
        {
            var questItem = new QuestItem(template_title, template_description);

            var newTitle = "   ";

            var exception = Assert.Throws<ArgumentException>(() => questItem.UpdateTitle(newTitle));
            Assert.Equal(QuestMessages.TitleCannotBeNullOrEmpty, exception.Message);
        }

        //Quest Item Description tests

        [Fact]
        public void QuestItem_Description_Is_Set_Correctly_In_Constructor()
        {
            var title = template_title;
            var description = template_description;

            var questItem = new QuestItem(title, description);

            Assert.Equal(description, questItem.Description);
        }

        [Fact]
        public void QuestItem_Description_Cannot_Be_Null_In_Constructor()
        {
            var title = template_title;
            string? description = null;
            var exception = Assert.Throws<ArgumentException>(() => new QuestItem(title, description));
            Assert.Equal(QuestMessages.DescriptionCannotBeNullOrEmpty, exception.Message);
        }

        [Fact]
        public void QuestItem_Description_Cannot_Be_Empty_In_Constructor()
        {
            var title = template_title;
            var description = string.Empty;
            var exception = Assert.Throws<ArgumentException>(() => new QuestItem(title, description));
            Assert.Equal(QuestMessages.DescriptionCannotBeNullOrEmpty, exception.Message);
        }

        [Fact]
        public void QuestItem_Description_Cannot_Be_Whitespace_In_Constructor()
        {
            var title = template_title;
            var description = "   ";
            var exception = Assert.Throws<ArgumentException>(() => new QuestItem(title, description));
            Assert.Equal(QuestMessages.DescriptionCannotBeNullOrEmpty, exception.Message);
        }   


        [Fact]
        public void QuestItem_Description_Can_Be_Updated()
        {
            var questItem = new QuestItem(template_title, template_description);
            var newDescription = "Walk 10 km following a mountain path.";

            questItem.UpdateDescription(newDescription);

            Assert.Equal(newDescription, questItem.Description);
        }

        [Fact]
        public void QuestItem_Description_Cannot_Be_Null_When_Updated()
        {
            var questItem = new QuestItem(template_title, template_description);

            string? newDescription = null;

            var exception = Assert.Throws<ArgumentException>(() => questItem.UpdateDescription(newDescription));
            Assert.Equal(QuestMessages.DescriptionCannotBeNullOrEmpty, exception.Message);
        }

        [Fact]
        public void QuestItem_Description_Cannot_Be_Empty_When_Updated()
        {
            var questItem = new QuestItem(template_title, template_description);

            var newDescription = string.Empty;

            var exception = Assert.Throws<ArgumentException>(() => questItem.UpdateDescription(newDescription));
            Assert.Equal(QuestMessages.DescriptionCannotBeNullOrEmpty, exception.Message);
        }

        [Fact]
        public void QuestItem_Description_Cannot_Be_Whitespace_When_Updated()
        {
            var questItem = new QuestItem(template_title, template_description);

            var newDescription = "   ";

            var exception = Assert.Throws<ArgumentException>(() => questItem.UpdateDescription(newDescription));
            Assert.Equal(QuestMessages.DescriptionCannotBeNullOrEmpty, exception.Message);
        }

        //Quest Item Status tests

        [Fact]
        public void QuestItem_Status_Defaults_To_Planned_In_Constructor()
        {
            var title = template_title;
            var description = template_description;

            var questItem = new QuestItem(title, description);

            Assert.Equal(QuestStatus.Planned, questItem.Status);
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
            
            var exception = Assert.Throws<InvalidOperationException>(questItem.CompleteQuest);
            Assert.Equal(QuestMessages.QuestNotInProgress, exception.Message);
        }

        [Fact]
        public void QuestItem_Cannot_Be_Completed_Twice()
        {
            var questItem = new QuestItem(template_title, template_description);
            
            questItem.StartQuest();
            questItem.CompleteQuest();
            
            var exception = Assert.Throws<InvalidOperationException>(questItem.CompleteQuest);
            Assert.Equal(QuestMessages.QuestAlreadyCompleted, exception.Message);
        }
    }
}