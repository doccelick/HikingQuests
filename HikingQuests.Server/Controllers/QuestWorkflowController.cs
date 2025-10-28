using HikingQuests.Server.Application;
using Microsoft.AspNetCore.Mvc;

namespace HikingQuests.Server.Controllers
{
    [Route("api/quest-workflow")]
    [ApiController]
    public class QuestWorkflowController : ControllerBase
    {
        private readonly IQuestWorkflowService _questWorkFlowService;
        public QuestWorkflowController(IQuestWorkflowService questWorkFlowService) 
            => _questWorkFlowService = questWorkFlowService;

        [HttpPatch("{id}/start")]
        public IActionResult StartQuest(Guid id)
        {
            var updatedQuest = _questWorkFlowService.StartQuest(id);
            return Ok(updatedQuest);
        }

        [HttpPatch("{id}/complete")]
        public IActionResult CompleteQuest(Guid id)
        {
            _questWorkFlowService.CompleteQuest(id);
            return NoContent();
        }
    }
}
