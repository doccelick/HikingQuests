using System.ComponentModel.DataAnnotations;
using HikingQuests.Server.Constants;

namespace HikingQuests.Server.Dtos
{
    public class UpdateQuestDto
    {       
        [StringLength(100, ErrorMessage = QuestMessages.TitleTooLong)]
        public string? Title { get; set; }

        [StringLength(500, ErrorMessage = QuestMessages.DescriptionTooLong)]
        public string? Description { get; set; }
    }
}
