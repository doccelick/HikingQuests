using HikingQuests.Server.Application.Dtos;
using HikingQuests.Server.Application.Interfaces;
using HikingQuests.Server.Constants;
using Microsoft.AspNetCore.Mvc;

namespace HikingQuests.Server.Controllers
{
    [Route("api/quest-management")]
    [ApiController]
    public class QuestManagementController : ControllerBase
    {
        private readonly IQuestManagementService _questManagementService;
        private readonly IUnitOfWork _unitOfWork;

        public QuestManagementController(IQuestManagementService questManagementService, IUnitOfWork unitOfWork)
        {
            _questManagementService = questManagementService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestAsync([FromBody] AddQuestDto addQuestDto)
        {
            var questItem = await _questManagementService.AddQuestAsync(addQuestDto);

            await _unitOfWork.SaveChangesAsync();

            return CreatedAtRoute(
                routeName: "GetQuestByIdRoute",
                routeValues: new { id = questItem.Id },
                value: questItem);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateQuestAsync(Guid id, [FromBody] UpdateQuestDto updateQuestDto)
        {
            if (updateQuestDto == null)
            {
                throw new ArgumentNullException(nameof(updateQuestDto), QuestMessages.UpdateQuestDtoCannotBeNull);
            }

            if (!ModelState.IsValid)
                throw new ArgumentException(
                    string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage))
                );

            var titleHasValue = !string.IsNullOrWhiteSpace(updateQuestDto.Title);
            var descriptionHasValue = !string.IsNullOrWhiteSpace(updateQuestDto.Description);

            if (!titleHasValue && !descriptionHasValue)
            {
                throw new ArgumentException(QuestMessages.NothingToUpdate);
            }

            await _questManagementService.UpdateQuestAsync(id, updateQuestDto);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteQuestAsync(Guid id)
        {
            await _questManagementService.DeleteQuestAsync(id);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
