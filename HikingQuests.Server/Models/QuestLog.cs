namespace HikingQuests.Server.Models
{
    public class QuestLog
    {
        public QuestLog() { }

        private Dictionary<Guid, QuestItem> questItems = new Dictionary<Guid, QuestItem>();
        
        public void AddQuest(QuestItem? questItem)
        {
            if (questItem == null)
            {
                throw new ArgumentNullException(nameof(questItem));
            }

            if (questItems.ContainsKey(questItem.Id))
            {
                throw new InvalidOperationException(QuestMessages.QuestAlreadyExistsInLog);
            }
            questItems[questItem.Id] = questItem;
        }

        public QuestItem GetQuestById(Guid questId)
        {
            if (!questItems.ContainsKey(questId))
            {
                throw new KeyNotFoundException(QuestMessages.QuestNotFound);
            }
            return questItems[questId];
        }

        public QuestItem GetQuestByTitle(string title)
        {
            var questItem = questItems.Values.FirstOrDefault(q => q.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (questItem == null)
            {
                throw new KeyNotFoundException(QuestMessages.QuestNotFound);
            }
            return questItem;
        }

        public IEnumerable<QuestItem> GetAllQuestItems()
        {
            return questItems.Values;
        }

        public void UpdateQuestTitle(Guid questId, string newTitle)
        {
            var questItem = GetQuestById(questId);
            questItem.UpdateTitle(newTitle);
        }

        public void UpdateQuestDescription(Guid questId, string newDescription)
        {
            var questItem = GetQuestById(questId);
            questItem.UpdateDescription(newDescription);
        }
    }
}
