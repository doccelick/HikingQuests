using HikingQuests.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace HikingQuests.Server.Controllers
{
    [Route("api/quests")]
    [ApiController]
    public class QuestController : ControllerBase
    {
        private readonly IQuestLog questLog;

        public QuestController(IQuestLog incomingQuestLog) => questLog = incomingQuestLog;

        [HttpGet]
        public IActionResult GetQuests() =>
        HandleDomainExceptions(() =>
        {
            var quests = questLog.GetAllQuestItems();
            return Ok(quests);
        });

        [HttpGet("{id}")]
        public IActionResult GetQuestItemById(Guid id) =>
            HandleDomainExceptions(() =>
            {
                var questItem = questLog.GetQuestById(id);
                return Ok(questItem);
            });

        [HttpPost]
        public IActionResult AddQuest([FromBody] QuestItem questItem) =>
            HandleDomainExceptions(() =>
            {
                questLog.AddQuest(questItem);
                return CreatedAtAction(nameof(GetQuestItemById), new { id = questItem.Id }, questItem);
            });

        [HttpPatch("{id}/title")]
        public IActionResult UpdateQuestTitle(Guid id, [FromBody] string newTitle) =>
            HandleDomainExceptions(() =>
            {
                questLog.UpdateQuestTitle(id, newTitle);
                return NoContent();
            });

        [HttpPatch("{id}/description")]
        public IActionResult UpdateQuestDescription(Guid id, [FromBody] string newDescription) =>
            HandleDomainExceptions(() =>
            {
                questLog.UpdateQuestDescription(id, newDescription);
                return NoContent();
            });

        private IActionResult HandleDomainExceptions(Func<IActionResult> action)
        {
            try
            {
                return action();
            }
            catch (ArgumentNullException)
            {
                return BadRequest(QuestMessages.QuestItemCannotBeNull);
            }
            catch (InvalidOperationException)
            {
                return Conflict(QuestMessages.QuestAlreadyExistsInLog);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(QuestMessages.QuestNotFound);
            }
        }
    }
}
