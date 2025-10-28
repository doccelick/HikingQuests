namespace HikingQuests.Server.Application.Dtos
{
    public class QuestResponseDto
    {public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
