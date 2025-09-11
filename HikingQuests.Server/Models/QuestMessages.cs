namespace HikingQuests.Server.Models
{
    public static class QuestMessages
    {
        public const string TitleCannotBeNullOrEmpty = "Title cannot be null or empty.";
        public const string DescriptionCannotBeNullOrEmpty = "Description cannot be null or empty.";
        public const string QuestAlreadyCompleted = "Quest is already completed.";
        public const string QuestNotInProgress = "Quest must be in progress to be completed.";
        public const string QuestAlreadyExistsInLog = "Quest with same ID already exists in the quest log.";
        public const string QuestNotFound = "Quest not found in the quest log.";
    }
}
