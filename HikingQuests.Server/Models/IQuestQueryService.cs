namespace HikingQuests.Server.Models
{
    public interface IQuestQueryService
    {
        QuestItem GetQuestById(Guid id);
        QuestItem GetQuestByTitle(string title);
        IEnumerable<QuestItem> GetAllQuestItems();
    }
}
