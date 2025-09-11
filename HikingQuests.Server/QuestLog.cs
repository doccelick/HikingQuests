namespace HikingQuests.Server
{
    public class QuestLog
    {
        public QuestLog() { }

        private Dictionary<Guid, QuestItem> _questItems = new Dictionary<Guid, QuestItem>();
        public IEnumerable<QuestItem> QuestItems => _questItems.Values;

        public void AddQuest(QuestItem? questItem)
        {
            if (questItem == null)
            {
                throw new ArgumentNullException(nameof(questItem));
            }

            if (_questItems.ContainsKey(questItem.Id))
            {
                throw new InvalidOperationException(QuestMessages.QuestAlreadyExistsInLog);
            }
            _questItems[questItem.Id] = questItem;
        }

        public QuestItem GetQuestById(Guid questId)
        {
            if (!_questItems.ContainsKey(questId))
            {
                throw new KeyNotFoundException(QuestMessages.QuestNotFound);
            }
            return _questItems[questId];
        }

        public QuestItem GetQuestByTitle(string title)
        {
            var questItem = _questItems.Values.FirstOrDefault(q => q.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (questItem == null)
            {
                throw new KeyNotFoundException(QuestMessages.QuestNotFound);
            }
            return questItem;
        }
    }
}
