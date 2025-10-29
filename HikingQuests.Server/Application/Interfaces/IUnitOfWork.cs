namespace HikingQuests.Server.Application.Interfaces
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Commits all tracked changes in the current context to the database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>The number of state entries written to the database</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken =  default);
    }
}
