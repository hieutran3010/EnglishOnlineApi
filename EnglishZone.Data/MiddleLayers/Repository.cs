#region

using EFPostgresEngagement;
using GraphQLDoorNet.Abstracts;

#endregion

namespace EnglishZone.Data.MiddleLayers
{
    public class Repository<TEntity> : RepositoryBase<TEntity, EnglishZoneDbContext>,
        IRepository<TEntity>
        where TEntity : class, IEntityBase, new()
    {
        public Repository(EnglishZoneDbContext dbContext) : base(dbContext)
        {
        }
    }
}