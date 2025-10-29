using HikingQuests.Server.Constants;
using HikingQuests.Server.Domain.Entities;
using HikingQuests.Server.Infrastructure.Mapping;
using HikingQuests.Server.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HikingQuests.Server.Infrastructure
{
    public class QuestRepository : IQuestRepository
    {
        private readonly QuestDbContext _context;

        public QuestRepository(QuestDbContext context) => _context = context;

        public Task AddAsync(QuestItem item)
        {
            var entity = QuestMapper.ToEntity(item);
            _context.QuestItems.Add(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<QuestItem>> GetAllQuestsAsync()
        {
            var entities = await _context.QuestItems
                .AsNoTracking()
                .ToListAsync();

            return entities.Select(QuestMapper.ToDomain);
        }

        public async Task<QuestItem> GetByIdAsync(Guid id)
        {
            var entity = await _context.QuestItems
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                throw new KeyNotFoundException(QuestMessages.QuestNotFound);
            }

            return QuestMapper.ToDomain(entity);
        }

        public async Task<QuestItem> GetByTitleAsync(string title)
        {
            var entity = await _context.QuestItems
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Title == title);

            if (entity == null)
            {
                throw new KeyNotFoundException(QuestMessages.QuestNotFound);
            }

            return QuestMapper.ToDomain(entity);
        }

        public async Task UpdateAsync(QuestItem questItem)
        {
            var existingEntity = await _context.QuestItems.FindAsync(questItem.Id);
            if (existingEntity == null)
            {
                throw new InvalidOperationException(QuestMessages.QuestNotFound);
            }

            existingEntity.Title = questItem.Title;
            existingEntity.Description = questItem.Description;
            existingEntity.Status = questItem.Status;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.QuestItems.FindAsync(id);

            if (entity is null) 
            {
                return;
            }

            _context.QuestItems.Remove(entity);
        }       
    }
}
