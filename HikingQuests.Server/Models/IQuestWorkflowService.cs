namespace HikingQuests.Server.Models
{
    public interface IQuestWorkflowService
    {        
        QuestItem StartQuest(Guid id);
        void CompleteQuest(Guid id);
    }
}
