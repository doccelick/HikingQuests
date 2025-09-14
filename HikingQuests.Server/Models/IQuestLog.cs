namespace HikingQuests.Server.Models
{
    public interface IQuestLog
    {
        void AddQuest(QuestItem quest);
        QuestItem GetQuestById(Guid id);
        QuestItem GetQuestByTitle(string title);
        IEnumerable<QuestItem> GetAllQuestItems();
        void UpdateQuestTitle(Guid id, string newTitle);
        void UpdateQuestDescription(Guid id, string newDescription);
        void StartQuest(Guid id);
        void CompleteQuest(Guid id);
    }
}
