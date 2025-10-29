namespace HikingQuests.Server.Constants
{
    public static class QuestMessages
    {
        public const string TitleCannotBeNullOrEmpty = "Title cannot be null or empty.";
        public const string DescriptionCannotBeNullOrEmpty = "Description cannot be null or empty.";
        public const string QuestCannotBeStarted = "Quest cannot be started";
        public static string QuestAlreadyInProgress = "Quest is already in progress";
        public const string QuestAlreadyCompleted = "Quest is already completed.";
        public const string QuestNotInProgress = "Quest must be in progress to be completed.";
        public const string QuestAlreadyExistsInLog = "Quest with same ID already exists.";
        public const string QuestNotFound = "Quest not found.";
        public const string QuestItemCannotBeNull = "Quest cannot be null.";
        public const string NothingToUpdate = "Title and Description cannot be null or empty.";
        public const string TitleTooLong = "Title cannot exceed 100 characters.";
        public const string DescriptionTooLong = "Description cannot exceed 500 characters.";
        public const string UnexpectedError = "An unexpected error occured";
        public const string UpdateQuestDtoCannotBeNull = "'UpdateQuestDto' cannot be null";
    }
}