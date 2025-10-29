using HikingQuests.Server.Application.Interfaces;
using HikingQuests.Server.Domain.Entities;
using HikingQuests.Server.Infrastructure;

namespace HikingQuests.Server.Application
{
    public class QuestQueryService : IQuestQueryService
    {
        private readonly IQuestRepository _repository;

        public QuestQueryService(IQuestRepository repository) => _repository = repository;


        public async Task<IEnumerable<QuestItem>> GetAllQuestsAsync()
        {
            return await _repository.GetAllQuestsAsync();
        }

        public async Task<QuestItem> GetQuestByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<QuestItem> GetQuestByTitleAsync(string title)
        {
            return await _repository.GetByTitleAsync(title);
        }
    }
}
