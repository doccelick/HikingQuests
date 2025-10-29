using HikingQuests.Server.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HikingQuests.Server.Controllers
{
    [Route("api/quest-workflow")]
    [ApiController]
    public class QuestWorkflowController : ControllerBase
    {
        private readonly IQuestWorkflowService _questWorkFlowService;
        private readonly IUnitOfWork _unitOfWork;

        public QuestWorkflowController(IQuestWorkflowService questWorkFlowService, IUnitOfWork unitOfWork)
        {
            _questWorkFlowService = questWorkFlowService;
            _unitOfWork = unitOfWork;
        }

        [HttpPatch("{id}/start")]
        public async Task<IActionResult> StartQuestAsync(Guid id)
        {
            await _questWorkFlowService.StartQuestAsync(id);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> CompleteQuestAsync(Guid id)
        {
            await _questWorkFlowService.CompleteQuestAsync(id);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
