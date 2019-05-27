using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Amnesia.Domain.Context
{
    public class BlockchainDesignContext : IDesignTimeDbContextFactory<BlockchainContext>
    {
        public BlockchainContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<BlockchainContext>();

            if (configuration.GetValue("UseSqlite", false))
            {
                builder.UseSqlite(configuration.GetConnectionString("Sqlite"));
            }
            else
            {
                builder.UseSqlServer(configuration.GetConnectionString("Sql"));
            }

            return new BlockchainContext(builder.Options);
        }
    }
}