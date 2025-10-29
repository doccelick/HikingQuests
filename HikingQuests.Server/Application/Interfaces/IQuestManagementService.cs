using HikingQuests.Server.Application.Dtos;
using HikingQuests.Server.Domain.Entities;

namespace HikingQuests.Server.Application.Interfaces
{
    public interface IQuestManagementService
    {
        Task<QuestItem> AddQuestAsync(AddQuestDto addQuestDto);
        Task UpdateQuestAsync(Guid id, UpdateQuestDto updateQuestDto);
        Task DeleteQuestAsync(Guid id);
    }
}
