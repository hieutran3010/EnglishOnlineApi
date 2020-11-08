#region

using System.Threading;
using System.Threading.Tasks;
using GraphQLDoorNet.Abstracts;

#endregion

namespace EnglishZone.Data.MiddleLayers
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EnglishZoneDbContext _dbContext;

        public UnitOfWork(EnglishZoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IRepository<T> GetRepository<T>()
            where T : class, IEntityBase, new()
        {
            return new Repository<T>(_dbContext);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}