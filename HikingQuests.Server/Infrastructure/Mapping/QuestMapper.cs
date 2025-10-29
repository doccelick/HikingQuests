using HikingQuests.Server.Domain.Entities;
using HikingQuests.Server.Infrastructure.Entities;

namespace HikingQuests.Server.Infrastructure.Mapping
{
    public class QuestMapper
    {
        public static QuestEntity ToEntity(QuestItem questItem)
        {
            var entity = new QuestEntity() 
            {
                Id = questItem.Id,
                Title = questItem.Title,
                Description = questItem.Description,
                Status = questItem.Status
            };

            return entity;
        }

        public static QuestItem ToDomain(QuestEntity entity)
        {
            var item = new QuestItem(
                entity.Id,
                entity.Title,
                entity.Description,
                entity.Status
                );

            return item;
        }
    }
}
