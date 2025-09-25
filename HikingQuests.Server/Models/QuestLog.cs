using HikingQuests.Server.Constants;

namespace HikingQuests.Server.Models
{
    public class QuestLog : IQuestLog
    {
        public QuestLog() { }

        private Dictionary<Guid, QuestItem> questItems = new Dictionary<Guid, QuestItem>();
        
        public QuestItem AddQuest(QuestItem questItem)
        {
            if (questItem == null)
            {
                throw new ArgumentNullException(nameof(questItem), QuestMessages.QuestItemCannotBeNull);
            }

            if (questItems.ContainsKey(questItem.Id))
            {
                throw new InvalidOperationException(QuestMessages.QuestAlreadyExistsInLog);
            }
            questItems[questItem.Id] = questItem;
            return questItems[questItem.Id];
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
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                throw new ArgumentException(QuestMessages.TitleCannotBeNullOrEmpty);
            }

            var questItem = GetQuestById(questId);
            questItem.UpdateTitle(newTitle);
        }

        public void UpdateQuestDescription(Guid questId, string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
            {
                throw new ArgumentException(QuestMessages.DescriptionCannotBeNullOrEmpty);
            }
            var questItem = GetQuestById(questId);
            questItem.UpdateDescription(newDescription);
        }

        public QuestItem StartQuest(Guid questId)
        {
            var questItem = GetQuestById(questId);

            if (questItem == null)
            {
                throw new KeyNotFoundException(QuestMessages.QuestNotFound);
            }
            if (questItem.Status == QuestStatus.InProgress)
            {
                throw new InvalidOperationException(QuestMessages.QuestAlreadyInProgress);
            }
            if (questItem.Status == QuestStatus.Completed)
            {
                throw new InvalidOperationException(QuestMessages.QuestAlreadyCompleted);
            }
            questItem.StartQuest();
            return questItem;
        }

        public void CompleteQuest(Guid questId)
        {
            var questItem = GetQuestById(questId);

            if (questItem == null)
            {
                throw new KeyNotFoundException(QuestMessages.QuestNotFound);
            }
            if (questItem.Status == QuestStatus.Planned)
            {
                throw new InvalidOperationException(QuestMessages.QuestNotInProgress);
            }
            if (questItem.Status == QuestStatus.Completed)
            {
                throw new InvalidOperationException(QuestMessages.QuestAlreadyCompleted);
            }            
            questItem.CompleteQuest();
        }

        public void DeleteQuest(Guid questId)
        {
            var questItem = GetQuestById(questId);

            if (questItem == null)
            {
                throw new KeyNotFoundException(QuestMessages.QuestNotFound);
            }

            questItems.Remove(questId);
        }
    }
}
