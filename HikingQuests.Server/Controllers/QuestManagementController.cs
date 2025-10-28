using HikingQuests.Server.Constants;
using HikingQuests.Server.Dtos;
using HikingQuests.Server.Infrastructure.Entities;
using HikingQuests.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace HikingQuests.Server.Controllers
{
    [Route("api/quest-management")]
    [ApiController]
    public class QuestManagementController : ControllerBase
    {
        private readonly IQuestManagementService _questManagementService;

        public QuestManagementController(IQuestManagementService questManagementService) 
            => _questManagementService = questManagementService;

        [HttpPost]
        public IActionResult AddQuest([FromBody] QuestItem incomingQuestItem)
        {
            var questItem = _questManagementService.AddQuest(incomingQuestItem);

            return CreatedAtRoute(
                routeName: "GetQuestByIdRoute", 
                routeValues: new { id = questItem.Id }, 
                value: questItem);
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateQuest(Guid id, [FromBody] UpdateQuestDto updateQuestDto)
        {
            if (updateQuestDto == null)
            {
                throw new ArgumentNullException(nameof(updateQuestDto), QuestMessages.UpdateQuestDtoCannotBeNull);
            }

            var titleIsEmpty = string.IsNullOrWhiteSpace(updateQuestDto.Title);
            var descriptionIsEmpty = string.IsNullOrWhiteSpace(updateQuestDto.Description);

            if (titleIsEmpty && descriptionIsEmpty)
            {
                throw new ArgumentException(QuestMessages.NothingToUpdate);
            }

            if (!ModelState.IsValid)
                throw new ArgumentException(
                    string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage))
                );

            _questManagementService.UpdateQuestTitle(id, updateQuestDto.Title!);
            _questManagementService.UpdateQuestDescription(id, updateQuestDto.Description!);


            return NoContent();
        }

        [HttpDelete("{id}/delete")]
        public IActionResult DeleteQuest(Guid id)
        {
            _questManagementService.DeleteQuest(id);
            return NoContent();
        }
    }
}
