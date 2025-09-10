namespace HikingQuests.Server
{
    public class QuestItem
    {
        public string Title { get; }
        public QuestStatus Status { get; private set; }

        public QuestItem(string title)
        {
            Title = title;
            Status = QuestStatus.Planned;
        }
    }
}
