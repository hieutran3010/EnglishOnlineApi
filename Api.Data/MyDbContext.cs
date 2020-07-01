namespace Api.Data
{
    using Entities;
    using EFPostgresEngagement.Abstract;
    using EFPostgresEngagement.DbContextBase;
    using Microsoft.EntityFrameworkCore;
    
    public class MyDbContext : PostgresDbContextBase<MyDbContext>
    {
        public DbSet<SampleModel> SampleModels { get; set; }
        public MyDbContext(DbContextOptions<MyDbContext> options, IDbTracker dbTracker) : base(options,
            dbTracker)
        {
        }
    }
}