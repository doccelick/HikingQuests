using HikingQuests.Server.Application.Interfaces;
using HikingQuests.Server.Infrastructure;

namespace HikingQuests.Server.Application
{
    public class QuestWorkflowService : IQuestWorkflowService
    {
        private readonly IQuestRepository _repository;

        public QuestWorkflowService(IQuestRepository repository) => _repository = repository;

        public async Task CompleteQuestAsync(Guid id)
        {
            var questItem = await _repository.GetByIdAsync(id);
            questItem.CompleteQuest();
            await _repository.UpdateAsync(questItem);
        }

        public async Task StartQuestAsync(Guid id)
        {
            var questItem = await _repository.GetByIdAsync(id);
            questItem.StartQuest();
            await _repository.UpdateAsync(questItem);
        }
    }
}
