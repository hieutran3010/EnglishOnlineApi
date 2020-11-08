#region

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

#endregion

namespace EnglishZone.Data
{
    public class EnglishZoneDbContextFactory : IDesignTimeDbContextFactory<EnglishZoneDbContext>
    {
        public EnglishZoneDbContext CreateDbContext(string[] args)
        {
            var connection = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            var optionsBuilder = new DbContextOptionsBuilder<EnglishZoneDbContext>();
            optionsBuilder.UseNpgsql(connection ??
                                     "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=EnglishZone;");

            return new EnglishZoneDbContext(optionsBuilder.Options, null);
        }
    }
}