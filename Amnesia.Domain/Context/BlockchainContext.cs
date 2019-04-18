using System.Collections.Generic;
using Amnesia.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Amnesia.Domain.Context
{
    public class BlockchainContext : DbContext
    {
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Definition> Definitions { get; set; }
        public DbSet<Data> Data { get; set; }

        public BlockchainContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Block>     ().HasKey(b => b.Hash);
            modelBuilder.Entity<Content>   ().HasKey(c => c.Hash);
            modelBuilder.Entity<Definition>().HasKey(d => d.Hash);
            modelBuilder.Entity<Data>      ().HasKey(d => d.Hash);

            modelBuilder.Entity<Definition>()
                .Property(d => d.Meta)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<Dictionary<string, string>>(m)
                );

            modelBuilder.Entity<Block>()
                .HasOne(b => b.Content)
                .WithOne(c => c.Block)
                .HasForeignKey<Block>(b => b.ContentHash);

            modelBuilder.Entity<Block>()
                .HasOne(b => b.PreviousBlock)
                .WithMany()
                .HasForeignKey(b => b.PreviousBlockHash);

            modelBuilder.Entity<Content>()
                .HasMany(c => c.Definitions)
                .WithOne()
                .HasForeignKey(d => d.ContentDefinitionHash);

            modelBuilder.Entity<Content>()
                .HasMany(c => c.Mutations)
                .WithOne()
                .HasForeignKey(m => m.ContentMutationHash);

            modelBuilder.Entity<Definition>()
                .HasOne(d => d.Data)
                .WithOne()
                .HasForeignKey<Definition>(d => d.DataHash);

            modelBuilder.Entity<Definition>()
                .HasOne(d => d.PreviousDefinition)
                .WithOne()
                .HasForeignKey<Definition>(d => d.PreviousDefinitionHash);

            modelBuilder.Entity<Data>()
                .HasOne<Definition>()
                .WithOne()
                .HasForeignKey<Data>(d => d.PreviousDefinitionHash);
        }
    }
}