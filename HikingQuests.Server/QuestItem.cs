namespace HikingQuests.Server
{
    public class QuestItem
    {
        private readonly Guid questIdentifier = Guid.NewGuid();
        public Guid Id => questIdentifier;

        public string Title { get; private set; }
        public string Description { get; private set; }
        public QuestStatus Status { get; private set; }

        public QuestItem(string? title, string? description)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException(QuestMessages.TitleCannotBeNullOrEmpty);
            }
            
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException(QuestMessages.DescriptionCannotBeNullOrEmpty);
            }
            //Id = Guid.NewGuid();
            Title = title;            
            Description = description;
            Status = QuestStatus.Planned;
        }

        public void UpdateTitle(string? newTitle)
        {            
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                throw new ArgumentException(QuestMessages.TitleCannotBeNullOrEmpty);
            }
            Title = newTitle;
        }

        public void UpdateDescription(string? newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
            {
                throw new ArgumentException(QuestMessages.DescriptionCannotBeNullOrEmpty);
            }
            Description = newDescription;
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
