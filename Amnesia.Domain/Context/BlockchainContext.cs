using System.Collections.Generic;
using Amnesia.Domain.Entity;
using Amnesia.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Amnesia.Domain.Context
{
    public class BlockchainContext : DbContext
    {
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Definition> Definitions { get; set; }
        public DbSet<Data> Data { get; set; }
        public DbSet<State> State { get; set; }

        public BlockchainContext(DbContextOptions options) : base(options)
        {
        }

        private static void SetupHashable<T>(ModelBuilder modelBuilder) where T : HashableObject
        {
            modelBuilder.Entity<T>(hashable =>
            {
                hashable.HasKey(h => h.Hash);
                hashable.Ignore(h => h.PrimaryHash);
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetupHashable<Block>(modelBuilder);
            SetupHashable<Content>(modelBuilder);
            SetupHashable<Definition>(modelBuilder);
            SetupHashable<Data>(modelBuilder);
            modelBuilder.Entity<State>().HasKey(s => s.PeerId);

            modelBuilder.Entity<Block>(block =>
            {
                block
                    .HasOne(b => b.Content)
                    .WithOne(c => c.Block)
                    .HasForeignKey<Block>(b => b.ContentHash);

                block
                    .HasOne(b => b.PreviousBlock)
                    .WithMany()
                    .HasForeignKey(b => b.PreviousBlockHash);
            });

            modelBuilder.Entity<Content>(content =>
            {
                content.Property(c => c.Definitions).HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<IList<byte[]>>(v));

                content.Property(c => c.Mutations).HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<IList<byte[]>>(v));
            });

            modelBuilder.Entity<Definition>(definition =>
            {
                definition
                    .HasOne(d => d.Data)
                    .WithOne()
                    .HasForeignKey<Definition>(d => d.DataHash);

                definition
                    .HasOne(d => d.PreviousDefinition)
                    .WithOne()
                    .HasForeignKey<Definition>(d => d.PreviousDefinitionHash);

                definition.Ignore(d => d.SignatureHash);
            });

            modelBuilder.Entity<Data>(data =>
            {
                data
                    .HasOne<Definition>()
                    .WithOne()
                    .HasForeignKey<Data>(d => d.PreviousDefinitionHash);

                data.Ignore((d) => d.SignatureHash);
            });

            modelBuilder.Entity<State>(state =>
            {
                state
                    .HasOne(s => s.CurrentBlock)
                    .WithMany()
                    .HasForeignKey(s => s.CurrentBlockHash);
            });
        }
    }
}