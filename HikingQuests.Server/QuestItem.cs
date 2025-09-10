namespace HikingQuests.Server
{
    public class QuestItem
    {
        public string Title { get; }
        public string Description { get; }
        public QuestStatus Status { get; private set; }

        public QuestItem(string title, string description)
        {
            Title = title;
            Description = description;
            Status = QuestStatus.Planned;
        }
    }
}
