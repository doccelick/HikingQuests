using HikingQuests.Server;

namespace HikingQuests.Test
{
    public class QuestLogTests
    {
        [Fact]
        public void QuestLog_Initial_Quest_Count_Is_Zero()
        {
            var questLog = new QuestLog();
            var questCount = questLog.QuestItems.Count();

            Assert.Equal(0, questCount);
        }

        [Fact]
        public void QuestLog_A_Single_QuestItem_Added_Increases_Quest_Count_To_One()
        {
            var questLog = new QuestLog();
            var questItem = new QuestItem("5 km forest hike", "Walk 5 km following a forest path.");
            questLog.AddQuest(questItem);

            var questCount = questLog.QuestItems.Count();

            Assert.Equal(1, questCount);
        }

        [Fact]
        public void QuestLog_A_Single_QuestItem_Added_Can_Be_Retrieved_By_ID()
        {
            var questLog = new QuestLog();
            var questItem = new QuestItem("5 km forest hike", "Walk 5 km following a forest path.");
            var questId = questItem.Id;

            questLog.AddQuest(questItem);

            var retrievedQuest = questLog.GetQuestById(questId);

            Assert.Equal(questItem.Id, retrievedQuest.Id);
        }

        [Fact]
        public void QuestLog_A_Single_QuestItem_Added_Can_Be_Retrieved_By_Title()
        {
            var questLog = new QuestLog();
            var questItem = new QuestItem("5 km forest hike", "Walk 5 km following a forest path.");
            var questTitle = questItem.Title;
            questLog.AddQuest(questItem);
            var retrievedQuest = questLog.GetQuestByTitle(questTitle);
            Assert.Equal(questItem.Id, retrievedQuest.Id);
        }

        [Fact]
        public void QuestLog_Adding_A_Null_QuestItem_Throws_Exception()
        {
            var questLog = new QuestLog();

            QuestItem? questItem = null;

            Assert.Throws<ArgumentNullException>(() => questLog.AddQuest(questItem));
        }

        [Fact]
        public void QuestLog_Adding_Duplicate_QuestItem_Throws_Exception()
        {
            var questLog = new QuestLog();
            var questItem = new QuestItem("5 km forest hike", "Walk 5 km following a forest path.");
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
        public void QuestLog_Adding_Multiple_QuestItems_Increases_Quest_Count_Accordingly()
        {
            var questLog = new QuestLog();
            var questItem1 = new QuestItem("5 km forest hike", "Walk 5 km following a forest path.");
            var questItem2 = new QuestItem("10 km mountain hike", "Climb 10 km up a mountain trail.");
            var questItem3 = new QuestItem("3 km lake walk", "Stroll 3 km around the lake.");
            questLog.AddQuest(questItem1);
            questLog.AddQuest(questItem2);
            questLog.AddQuest(questItem3);
            var questCount = questLog.QuestItems.Count();
            Assert.Equal(3, questCount);
        }
    }
}
