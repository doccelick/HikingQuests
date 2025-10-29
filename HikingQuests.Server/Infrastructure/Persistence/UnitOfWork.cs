using HikingQuests.Server.Application.Interfaces;

namespace HikingQuests.Server.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly QuestDbContext _questDbContext;

        public UnitOfWork(QuestDbContext questDbContext) => _questDbContext = questDbContext;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) 
            => await _questDbContext.SaveChangesAsync(cancellationToken);

        public void Dispose() => _questDbContext.Dispose();
    }
}
