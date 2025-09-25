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
        public IActionResult GetQuests()
        {
            var quests = questLog.GetAllQuestItems();
            return Ok(quests);
        }

        [HttpGet("{id}")]
        public IActionResult GetQuestItemById(Guid id)
        {
            var questItem = questLog.GetQuestById(id);
            return Ok(questItem);
        }

        [HttpPost]
        public IActionResult AddQuest([FromBody] QuestItem incomingQuestItem)
        {
            var questItem = questLog.AddQuest(incomingQuestItem);
            return CreatedAtAction(nameof(GetQuestItemById), new { id = questItem.Id }, questItem);
        }


        [HttpPatch("{id}")]
        public IActionResult UpdateQuest(Guid id, [FromBody] UpdateQuestDto updateQuestDto)
        {            
            if (updateQuestDto == null)
                throw new ArgumentNullException(nameof(updateQuestDto), QuestMessages.UpdateQuestDtoCannotBeNull);

            var titleIsEmpty = string.IsNullOrWhiteSpace(updateQuestDto.Title);
            var descriptionIsEmpty = string.IsNullOrWhiteSpace(updateQuestDto.Description);

            if (titleIsEmpty && descriptionIsEmpty)
                throw new ArgumentException(QuestMessages.NothingToUpdate);

            if (!ModelState.IsValid)
                throw new ArgumentException(
                    string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage))
                );

            if (!titleIsEmpty)
            {
                questLog.UpdateQuestTitle(id, updateQuestDto.Title!);
            }

            if (!descriptionIsEmpty)
            {
                questLog.UpdateQuestDescription(id, updateQuestDto.Description!);
            }

            var updatedQuest = questLog.GetQuestById(id);

            var response = new QuestResponseDto
            {
                Id = updatedQuest.Id,
                Title = updatedQuest.Title,
                Description = updatedQuest.Description,
            };

            return Ok(response);
        }

        [HttpPatch("{id}/start")]
        public IActionResult StartQuest(Guid id)
        {
            var updatedQuest = questLog.StartQuest(id);
            return Ok(updatedQuest);
        }

        [HttpPatch("{id}/complete")]
        public IActionResult CompleteQuest(Guid id)
        {
            questLog.CompleteQuest(id);
            return NoContent();
        }

        [HttpDelete("{id}/delete")]
        public IActionResult DeleteQuest(Guid id)
        {
            questLog.DeleteQuest(id);
            return NoContent();
        }
    }
}
