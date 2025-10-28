namespace HikingQuests.Server.Models
{
    public interface IQuestManagementService
    {
        QuestItem AddQuest(QuestItem quest);
        void UpdateQuestTitle(Guid id, string newTitle);
        void UpdateQuestDescription(Guid id, string newDescription);
        void DeleteQuest(Guid id);
    }
}
