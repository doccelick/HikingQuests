using HikingQuests.Server.Domain.Entities;

namespace HikingQuests.Server.Infrastructure
{
    public interface IQuestRepository
    {
        Task AddAsync(QuestItem questItem);
        Task<QuestItem> GetByIdAsync(Guid id);
        Task<QuestItem> GetByTitleAsync(string title);
        Task<IEnumerable<QuestItem>> GetAllQuestsAsync();
        Task UpdateAsync(QuestItem questItem);
        Task DeleteAsync(Guid id);
    }
}
