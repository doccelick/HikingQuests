using HikingQuests.Server.Domain.Entities;

namespace HikingQuests.Server.Application.Interfaces
{
    public interface IQuestQueryService
    {
        Task<QuestItem> GetQuestByIdAsync(Guid id);
        Task<QuestItem> GetQuestByTitleAsync(string title);
        Task<IEnumerable<QuestItem>> GetAllQuestsAsync();
    }
}
