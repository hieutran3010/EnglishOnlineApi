#region

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

#endregion

namespace HelenExpress.Data
{
    public class HelenExpressDbContextFactory : IDesignTimeDbContextFactory<HeLenExpressDbContext>
    {
        public HeLenExpressDbContext CreateDbContext(string[] args)
        {
            var connection = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            var optionsBuilder = new DbContextOptionsBuilder<HeLenExpressDbContext>();
            optionsBuilder.UseNpgsql(connection ??
                                     "Host=localhost;Port=5432;Username=postgres;Password=1nS1t3;Database=he-prod-bk1;");

            return new HeLenExpressDbContext(optionsBuilder.Options, null);
        }
    }
}