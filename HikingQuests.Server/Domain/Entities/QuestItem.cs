using HikingQuests.Server.Constants;
using HikingQuests.Server.Domain.ValueObjects;

namespace HikingQuests.Server.Domain.Entities
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

            Title = title;            
            Description = description;
            Status = QuestStatus.Planned;
        }

        public QuestItem(Guid questIdentifier, string title, string description, QuestStatus status) : this (title, description)
        {
            this.questIdentifier = questIdentifier;
            this.Status = status;
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
            if (Status == QuestStatus.InProgress)
            {
                throw new InvalidOperationException(QuestMessages.QuestAlreadyInProgress);
            }

            if (Status == QuestStatus.Completed)
            {
                throw new InvalidOperationException(QuestMessages.QuestAlreadyCompleted);
            }

            Status = QuestStatus.InProgress;
        }

        public void CompleteQuest()
        {
            if (Status == QuestStatus.Completed)
            {
                throw new InvalidOperationException(QuestMessages.QuestAlreadyCompleted);
            }

            if (Status == QuestStatus.Planned)
            {
                throw new InvalidOperationException(QuestMessages.QuestNotInProgress);
            }

            Status = QuestStatus.Completed;
        }
    }
}
