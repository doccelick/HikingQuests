using HikingQuests.Server.Application.Dtos;
using HikingQuests.Server.Application.Interfaces;
using HikingQuests.Server.Constants;
using HikingQuests.Server.Domain.Entities;
using HikingQuests.Server.Infrastructure;

namespace HikingQuests.Server.Application
{
    public class QuestManagementService : IQuestManagementService
    {
        private readonly IQuestRepository _repository;
                
        public QuestManagementService(IQuestRepository repository) => _repository = repository;

        public async Task<QuestItem> AddQuestAsync(AddQuestDto addQuestDto)
        {
            var newQuestItem = new QuestItem(addQuestDto.Title, addQuestDto.Description);

            await _repository.AddAsync(newQuestItem);

            return newQuestItem;
        }

        public async Task UpdateQuestAsync(Guid id, UpdateQuestDto updateQuestDto)
        {
            var questItem = await _repository.GetByIdAsync(id);

            if (questItem is null)
            {
                throw new KeyNotFoundException(QuestMessages.QuestNotFound);
            }

            questItem.UpdateDescription(updateQuestDto.Description);
            questItem.UpdateTitle(updateQuestDto.Title);
            await _repository.UpdateAsync(questItem);
        }
                        
        public async Task DeleteQuestAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}