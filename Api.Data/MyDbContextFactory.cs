using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Api.Data
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            var connection = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseNpgsql(connection ?? "Host=localhost;Port=5432;Username=postgres;Password=1nS1t3;Database=MyDb;");

            return new MyDbContext(optionsBuilder.Options, null);
        }
    }
}