using HikingQuests.Server.Models;

namespace HikingQuests.Server.Controllers
{
    public class QuestController
    {
        private IQuestLog questLog;

        public QuestController(IQuestLog incomingQuestLog)
        {
            questLog = incomingQuestLog;
        }

        public IEnumerable<QuestItem> GetQuests()
        {
            return questLog.GetAllQuestItems();
        }
    }
}
