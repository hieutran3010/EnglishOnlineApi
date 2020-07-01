namespace Api.Data.MiddleLayers
{
    using System.Threading;
    using System.Threading.Tasks;
    using GraphQLDoorNet.Abstracts;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDbContext _dbContext;

        public UnitOfWork(MyDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public void Dispose()
        {
            this._dbContext.Dispose();
        }

        public IRepository<T> GetRepository<T>()
            where T : class, IEntityBase, new()
        {
            return new Repository<T>(this._dbContext);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return this._dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}