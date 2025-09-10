namespace HikingQuests.Server
{
    public class QuestItem
    {
        public string Title { get; private set; }
        public string Description { get; }
        public QuestStatus Status { get; private set; }

        public QuestItem(string? title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException(QuestMessages.TitleCannotBeNullOrEmpty);
            }
            
            Title = title;            
            Description = description;
            Status = QuestStatus.Planned;
        }

        public void UpdateTitle(string newTitle)
        {            
            Title = newTitle;
        }

        public void StartQuest()
        {
            Status = QuestStatus.InProgress;
        }

        public void CompleteQuest()
        {
            if (Status == QuestStatus.Completed)
            {
                throw new InvalidOperationException(QuestMessages.QuestAlreadyCompleted);
            }

            if (Status != QuestStatus.InProgress)
            {
                throw new InvalidOperationException(QuestMessages.QuestNotInProgress);
            }

            Status = QuestStatus.Completed;
        }
    }
}
