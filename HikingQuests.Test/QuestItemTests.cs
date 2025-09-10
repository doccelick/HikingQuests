using HikingQuests.Server;

namespace HikingQuests.Test
{
    public class QuestItemTests
    {
        [Fact]
        public void QuestItem_Status_Defaults_To_Planned_When_Created()
        {
            var questItem = new QuestItem("Walk 5 km");

            Assert.Equal(QuestStatus.Planned, questItem.Status);
        }
    }
}