using HikingQuests.Server.Constants;
using HikingQuests.Server.Models;

namespace HikingQuests.Test
{
    public class QuestLogTests
    {
        private string template_title = "5 km forest hike";
        private string template_description = "Walk 5 km following a forest path.";

        [Fact]
        public void QuestLog_Initial_Quest_Count_Is_Zero()
        {
            var questLog = new QuestLog();
            var questCount = questLog.GetAllQuestItems().Count();

            Assert.Equal(0, questCount);
        }

        [Fact]
        public void QuestLog_A_Single_QuestItem_Added_Increases_Quest_Count_To_One()
        {
            var questLog = new QuestLog();
            var questItem = new QuestItem(template_title, template_description);
            questLog.AddQuest(questItem);

            var questCount = questLog.GetAllQuestItems().Count();

            Assert.Equal(1, questCount);
        }

        [Fact]
        public void QuestLog_A_Single_QuestItem_Added_Can_Be_Retrieved_By_ID()
        {
            var questLog = new QuestLog();
            var questItem = new QuestItem(template_title, template_description);
            var questId = questItem.Id;

            questLog.AddQuest(questItem);

            var retrievedQuest = questLog.GetQuestById(questId);

            Assert.Equal(questItem.Id, retrievedQuest.Id);
        }

        [Fact]
        public void QuestLog_A_Single_QuestItem_Added_Can_Be_Retrieved_By_Title()
        {
            var questLog = new QuestLog();
            var questItem = new QuestItem(template_title, template_description);
            var questTitle = questItem.Title;
            questLog.AddQuest(questItem);
            var retrievedQuest = questLog.GetQuestByTitle(questTitle);
            Assert.Equal(questItem.Id, retrievedQuest.Id);
        }

        [Fact]
        public void QuestLog_Adding_A_Null_QuestItem_Throws_Exception()
        {
            var questLog = new QuestLog();

            QuestItem? questItem = null!;

            Assert.Throws<ArgumentNullException>(() => questLog.AddQuest(questItem));
        }

        [Fact]
        public void QuestLog_Adding_Duplicate_QuestItem_Throws_Exception()
        {
            var questLog = new QuestLog();
            var questItem = new QuestItem(template_title, template_description);
            questLog.AddQuest(questItem);

            var exception = Assert.Throws<InvalidOperationException>(() => questLog.AddQuest(questItem));
            Assert.Equal(QuestMessages.QuestAlreadyExistsInLog, exception.Message);
        }

        [Fact]
        public void QuestLog_Getting_Nonexistent_QuestItem_By_ID_Throws_Exception()
        {
            var questLog = new QuestLog();
            var nonExistentQuestId = Guid.NewGuid();
            var exception = Assert.Throws<KeyNotFoundException>(() => questLog.GetQuestById(nonExistentQuestId));
            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);
        }

        [Fact]
        public void QuestLog_Getting_Nonexistent_QuestItem_By_Empty_Guid_Throws_Exception()
        {
            var questLog = new QuestLog();
            var nonExistentQuestId = Guid.Empty;
            var exception = Assert.Throws<KeyNotFoundException>(() => questLog.GetQuestById(nonExistentQuestId));
            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);
        }

        [Fact]
        public void QuestLog_Getting_Nonexistent_QuestItem_By_Title_Throws_Exception()
        {
            var questLog = new QuestLog();
            var nonExistentTitle = "Nonexistent Quest";
            var exception = Assert.Throws<KeyNotFoundException>(() => questLog.GetQuestByTitle(nonExistentTitle));
            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);
        }

        [Fact]
        public void QuestLog_Adding_QuestItem_With_Same_Title_But_Different_ID_Is_Allowed()
        {
            var questLog = new QuestLog();
            var questItem1 = new QuestItem(template_title, template_description);
            var questItem2 = new QuestItem(template_title, "A different description for the same title.");
            questLog.AddQuest(questItem1);
            questLog.AddQuest(questItem2);
            var allQuests = questLog.GetAllQuestItems();
            Assert.Equal(2, allQuests.Count());
            Assert.Contains(questItem1, allQuests);
            Assert.Contains(questItem2, allQuests);
        }

        [Fact]
        public void QuestLog_Adding_Multiple_QuestItems_Increases_Quest_Count_Accordingly()
        {
            var questLog = new QuestLog();
            var questItem1 = new QuestItem(template_title, template_description);
            var questItem2 = new QuestItem("Catch a trout with a fly", "Use a fly rod to catch a trout with a fly.");
            var questItem3 = new QuestItem("Build a campfire", "Build a campfire using fallen sticks and dry leaves.");
            questLog.AddQuest(questItem1);
            questLog.AddQuest(questItem2);
            questLog.AddQuest(questItem3);
            var questCount = questLog.GetAllQuestItems().Count();
            Assert.Equal(3, questCount);
        }

        [Fact]
        public void QuestLog_QuestItems_Are_Enumerable()
        {
            var questLog = new QuestLog();
            var questItem1 = new QuestItem(template_title, template_description);
            var questItem2 = new QuestItem("Catch a trout with a fly", "Use a fly rod to catch a trout with a fly.");

            questLog.AddQuest(questItem1);
            questLog.AddQuest(questItem2);

            var titles = new List<string>();
            var allQuests = questLog.GetAllQuestItems();

            foreach (var quest in allQuests)
            {
                titles.Add(quest.Title);
            }
            Assert.Contains(template_title, titles);
            Assert.Contains("Catch a trout with a fly", titles);
        }

        [Fact]
        public void QuestLog_GetAllQuests_Returns_All_Added_QuestItems()
        {
            var questLog = new QuestLog();
            var questItem1 = new QuestItem(template_title, template_description);
            var questItem2 = new QuestItem("Catch a trout with a fly", "Use a fly rod to catch a trout with a fly.");
            var questItem3 = new QuestItem("Build a campfire", "Build a campfire using fallen sticks and dry leaves.");

            questLog.AddQuest(questItem1);
            questLog.AddQuest(questItem2);
            questLog.AddQuest(questItem3);

            var allQuests = questLog.GetAllQuestItems();

            Assert.Equal(3, allQuests.Count());
            Assert.Contains(questItem1, allQuests);
            Assert.Contains(questItem2, allQuests);
            Assert.Contains(questItem3, allQuests);
        }

        [Fact]
        public void QuestLog_GetAllQuests_On_Empty_Log_Returns_Empty_Collection()
        {
            var questLog = new QuestLog();
            var allQuests = questLog.GetAllQuestItems();
            Assert.Empty(allQuests);
        }

        [Fact]
        public void QuestLog_Can_Update_QuestItem_Title()
        {
            var questLog = new QuestLog();
            var questItem = new QuestItem("Original Title", template_description);

            questLog.AddQuest(questItem);

            var newTitle = "Updated Title";

            questLog.UpdateQuestTitle(questItem.Id, newTitle);

            var updatedQuest = questLog.GetQuestById(questItem.Id);

            Assert.Equal(newTitle, updatedQuest.Title);
        }

        [Fact]
        public void QuestLog_Can_Update_QuestItem_Description()
        {
            var questLog = new QuestLog();
            var questItem = new QuestItem(template_title, "Original Description");

            questLog.AddQuest(questItem);
            var newDescription = "Updated Description";

            questLog.UpdateQuestDescription(questItem.Id, newDescription);

            var updatedQuest = questLog.GetQuestById(questItem.Id);

            Assert.Equal(newDescription, updatedQuest.Description);
        }

        [Fact]
        public void QuestLog_Updating_Title_Of_Nonexistent_QuestItem_Throws_Exception()
        {
            var questLog = new QuestLog();
            var nonExistentQuestId = Guid.NewGuid();
            var newTitle = "New Title";

            var exception = Assert.Throws<KeyNotFoundException>(() => questLog.UpdateQuestTitle(nonExistentQuestId, newTitle));
            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);
        }

        [Fact]
        public void QuestLog_Updating_Description_Of_Nonexistent_QuestItem_Throws_Exception()
        {
            var questLog = new QuestLog();
            var nonExistentQuestId = Guid.NewGuid();
            var newDescription = "New Description";
            var exception = Assert.Throws<KeyNotFoundException>(() => questLog.UpdateQuestDescription(nonExistentQuestId, newDescription));
            Assert.Equal(QuestMessages.QuestNotFound, exception.Message);
        }
    }
}
