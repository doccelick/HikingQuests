using HikingQuests.Server.Domain.Entities;

namespace HikingQuests.Server.Application
{
    public interface IQuestQueryService
    {
        QuestItem GetQuestById(Guid id);
        QuestItem GetQuestByTitle(string title);
        IEnumerable<QuestItem> GetAllQuestItems();
    }
}
