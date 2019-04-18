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
            var connectionString = configuration.GetConnectionString("BlockchainDatabase");
            builder.UseSqlServer(connectionString);
            return new BlockchainContext(builder.Options);
        }
    }
}