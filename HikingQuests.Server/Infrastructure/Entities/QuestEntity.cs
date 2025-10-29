using HikingQuests.Server.Domain.ValueObjects;

namespace HikingQuests.Server.Infrastructure.Entities
{
    public class QuestEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public QuestStatus Status { get; set; }
    }
}
