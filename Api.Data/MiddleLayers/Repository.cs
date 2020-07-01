namespace Api.Data.MiddleLayers
{
    using EFPostgresEngagement;
    using GraphQLDoorNet.Abstracts;

    public class Repository<TEntity> : RepositoryBase<TEntity, MyDbContext>,
        IRepository<TEntity>
        where TEntity : class, IEntityBase, new()
    {
        public Repository(MyDbContext dbContext) : base(dbContext)
        {
        }
    }
}