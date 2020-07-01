#region

using EFPostgresEngagement;
using GraphQLDoorNet.Abstracts;

#endregion

namespace HelenExpress.Data.MiddleLayers
{
    public class Repository<TEntity> : RepositoryBase<TEntity, HeLenExpressDbContext>,
        IRepository<TEntity>
        where TEntity : class, IEntityBase, new()
    {
        public Repository(HeLenExpressDbContext dbContext) : base(dbContext)
        {
        }
    }
}