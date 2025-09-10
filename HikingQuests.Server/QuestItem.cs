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
