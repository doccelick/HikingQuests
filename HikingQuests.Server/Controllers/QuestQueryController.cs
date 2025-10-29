using HikingQuests.Server.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HikingQuests.Server.Controllers
{
    [Route("api/quest-query")]
    [ApiController]
    public class QuestQueryController : ControllerBase
    {
        private readonly IQuestQueryService _questQueryService;

        public QuestQueryController(IQuestQueryService questQueryService) 
            => _questQueryService = questQueryService;

        [HttpGet]
        public async Task<IActionResult> GetAllQuestsAsync()
        {
            var quests = await _questQueryService.GetAllQuestsAsync();
            return Ok(quests);
        }

        [HttpGet("{id}",Name = "GetQuestByIdRoute")]
        public async Task<IActionResult> GetQuestItemByIdAsync(Guid id)
        {
            var questItem = await _questQueryService.GetQuestByIdAsync(id);

            if (questItem == null)
            {
                return NotFound();
            }

            return Ok(questItem);
        }
    }
}
