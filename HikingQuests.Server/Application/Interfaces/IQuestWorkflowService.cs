namespace HikingQuests.Server.Application.Interfaces
{
    public interface IQuestWorkflowService
    {        
        Task StartQuestAsync(Guid id);
        Task CompleteQuestAsync(Guid id);
    }
}
