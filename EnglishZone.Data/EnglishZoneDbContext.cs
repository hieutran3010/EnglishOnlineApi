#region

using EFPostgresEngagement.Abstract;
using EFPostgresEngagement.DbContextBase;
using EnglishZone.Data.Entities;
using Microsoft.EntityFrameworkCore;

#endregion

namespace EnglishZone.Data
{
    public class EnglishZoneDbContext : PostgresDbContextBase<EnglishZoneDbContext>
    {
        public EnglishZoneDbContext(DbContextOptions<EnglishZoneDbContext> options, IDbTracker dbTracker) : base(
            options,
            dbTracker)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
    }
}