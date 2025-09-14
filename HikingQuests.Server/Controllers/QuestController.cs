using HikingQuests.Server.Dtos;
using HikingQuests.Server.Models;
using HikingQuests.Server.Constants;
using Microsoft.AspNetCore.Mvc;

namespace HikingQuests.Server.Controllers
{    
    [ApiController]
    [Route("api/quests")]
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

        [HttpPatch("{id}")]
        public IActionResult UpdateQuest(Guid id, [FromBody] UpdateQuestDto updateQuestDto) =>
            HandleDomainExceptions(() =>
            {
                var titleIsEmpty = string.IsNullOrWhiteSpace(updateQuestDto.Title);
                var descriptionIsEmpty = string.IsNullOrWhiteSpace(updateQuestDto.Description);

                if (titleIsEmpty && descriptionIsEmpty)
                {
                    return BadRequest(QuestMessages.NothingToUpdate);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!string.IsNullOrWhiteSpace(updateQuestDto.Title))
                {
                    questLog.UpdateQuestTitle(id, updateQuestDto.Title);
                }

                if (!string.IsNullOrWhiteSpace(updateQuestDto.Description))
                {
                    questLog.UpdateQuestDescription(id, updateQuestDto.Description);
                }

                return NoContent();
            });

        [HttpPatch("{id}/start")]
        public IActionResult StartQuest(Guid id)
        {
            try 
            {
                questLog.StartQuest(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(QuestMessages.QuestNotFound);
            }
            catch (InvalidOperationException ex) when (ex.Message == QuestMessages.QuestAlreadyInProgress)
            {
                return Conflict(QuestMessages.QuestAlreadyInProgress);
            }
            catch (InvalidOperationException ex) when (ex.Message == QuestMessages.QuestAlreadyCompleted)
            {
                return Conflict(QuestMessages.QuestAlreadyCompleted);
            }
        }

        [HttpPatch("{id}/complete")]
        public IActionResult CompleteQuest(Guid id)
        {
            try
            {
                questLog.CompleteQuest(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(QuestMessages.QuestNotFound);
            }
            catch (InvalidOperationException ex) when (ex.Message == QuestMessages.QuestNotInProgress)
            {
                return Conflict(QuestMessages.QuestNotInProgress);
            }
            catch (InvalidOperationException ex) when (ex.Message == QuestMessages.QuestAlreadyCompleted)
            {
                return Conflict(QuestMessages.QuestAlreadyCompleted);
            }
        }

        [HttpDelete("{id}/delete")]
        public IActionResult DeleteQuest(Guid id)
        {
            try
            {
                questLog.DeleteQuest(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(QuestMessages.QuestNotFound);
            }
        }

        //TODO: Reconsider this generic approach.
        //It might make more sense to handle exceptions explicitly in each function to get accurate messages.
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
