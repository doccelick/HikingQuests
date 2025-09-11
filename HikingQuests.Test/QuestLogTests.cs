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
    }
}
