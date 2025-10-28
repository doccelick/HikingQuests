using HikingQuests.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace HikingQuests.Server.Controllers
{
    [Route("api/quest-query")]
    [ApiController]
    public class QuestQueryController : ControllerBase
    {
        private readonly IQuestQueryService _questQueryService;
        public QuestQueryController(IQuestQueryService incomingQuestLog) 
            => _questQueryService = incomingQuestLog;

        [HttpGet]
        public IActionResult GetQuests()
        {
            var quests = _questQueryService.GetAllQuestItems();
            return Ok(quests);
        }

        [HttpGet("{id}",Name = "GetQuestByIdRoute")]
        public IActionResult GetQuestItemById(Guid id)
        {
            var questItem = _questQueryService.GetQuestById(id);
            return Ok(questItem);
        }
    }
}
