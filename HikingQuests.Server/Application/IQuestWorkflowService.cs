using HikingQuests.Server.Domain.Entities;

namespace HikingQuests.Server.Application
{
    public interface IQuestWorkflowService
    {        
        QuestItem StartQuest(Guid id);
        void CompleteQuest(Guid id);
    }
}
