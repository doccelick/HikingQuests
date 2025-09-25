namespace HikingQuests.Server.Models
{
    public interface IQuestLog
    {
        QuestItem AddQuest(QuestItem quest);
        QuestItem GetQuestById(Guid id);
        QuestItem GetQuestByTitle(string title);
        IEnumerable<QuestItem> GetAllQuestItems();
        void UpdateQuestTitle(Guid id, string newTitle);
        void UpdateQuestDescription(Guid id, string newDescription);
        QuestItem StartQuest(Guid id);
        void CompleteQuest(Guid id);
        void DeleteQuest(Guid id);
    }
}
