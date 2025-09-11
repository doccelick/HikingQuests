namespace HikingQuests.Server
{
    public class QuestLog
    {
        public QuestLog() { }

        private Dictionary<Guid, QuestItem> _questItems = new Dictionary<Guid, QuestItem>();
        public IEnumerable<QuestItem> QuestItems => _questItems.Values;

        public void AddQuest(QuestItem questItem)
        {            
            _questItems[questItem.Id] = questItem;
        }

        public QuestItem GetQuestById(Guid questId)
        {            
            return _questItems[questId];
        }
    }
}
