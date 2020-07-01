#region

using EFPostgresEngagement.Abstract;
using EFPostgresEngagement.DbContextBase;
using HelenExpress.Data.Entities;
using Microsoft.EntityFrameworkCore;

#endregion

namespace HelenExpress.Data
{
    public class HeLenExpressDbContext : PostgresDbContextBase<HeLenExpressDbContext>
    {
        public HeLenExpressDbContext(DbContextOptions<HeLenExpressDbContext> options, IDbTracker dbTracker) : base(
            options,
            dbTracker)
        {
        }

        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<BillDescription> BillDescriptions { get; set; }
        public DbSet<ExportSession> BillExportSessions { get; set; }
        public DbSet<Params> Params { get; set; }

        public override void OnExtendModelCreating(ModelBuilder modelBuilder)
        {
            base.OnExtendModelCreating(modelBuilder);
            modelBuilder.Entity<Bill>()
                .HasOne(p => p.Sender)
                .WithMany(b => b.SendBills)
                .HasForeignKey(b => b.SenderId);

            modelBuilder.Entity<Bill>()
                .HasOne(p => p.Receiver)
                .WithMany(b => b.ReceivedBills)
                .HasForeignKey(b => b.ReceiverId);
        }
    }
}