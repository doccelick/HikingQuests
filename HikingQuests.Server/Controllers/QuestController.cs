using HikingQuests.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace HikingQuests.Server.Controllers
{
    [Route("api/quests")]
    [ApiController]
    public class QuestController : ControllerBase
    {
        private readonly IQuestLog questLog;

        public QuestController(IQuestLog incomingQuestLog)
        {
            questLog = incomingQuestLog;
        }

        [HttpGet]
        public ActionResult<IEnumerable<QuestItem>> GetQuests()
        {
            var quests = questLog.GetAllQuestItems();
            return Ok(quests);
        }

        [HttpGet("{id}")]
        public IActionResult GetQuestItemById(Guid id)
        {
            try
            {
                var questItem = questLog.GetQuestById(id);
                return Ok(questItem);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
        }
    }
}
