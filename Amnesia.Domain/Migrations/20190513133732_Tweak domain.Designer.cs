﻿// <auto-generated />
using System;
using Amnesia.Domain.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Amnesia.Domain.Migrations
{
    [DbContext(typeof(BlockchainContext))]
    [Migration("20190513133732_Tweak domain")]
    partial class Tweakdomain
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Amnesia.Domain.Entity.Block", b =>
                {
                    b.Property<byte[]>("Hash")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("ContentHash");

                    b.Property<int>("Nonce");

                    b.Property<byte[]>("PreviousBlockHash");

                    b.HasKey("Hash");

                    b.HasIndex("ContentHash")
                        .IsUnique()
                        .HasFilter("[ContentHash] IS NOT NULL");

                    b.HasIndex("PreviousBlockHash");

                    b.ToTable("Blocks");
                });

            modelBuilder.Entity("Amnesia.Domain.Entity.Content", b =>
                {
                    b.Property<byte[]>("Hash")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Definitions");

                    b.Property<string>("Mutations");

                    b.HasKey("Hash");

                    b.ToTable("Contents");
                });

            modelBuilder.Entity("Amnesia.Domain.Entity.Data", b =>
                {
                    b.Property<byte[]>("Hash")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Blob");

                    b.Property<byte[]>("Key");

                    b.Property<byte[]>("PreviousDefinitionHash");

                    b.Property<byte[]>("Signature");

                    b.HasKey("Hash");

                    b.HasIndex("PreviousDefinitionHash")
                        .IsUnique()
                        .HasFilter("[PreviousDefinitionHash] IS NOT NULL");

                    b.ToTable("Data");
                });

            modelBuilder.Entity("Amnesia.Domain.Entity.Definition", b =>
                {
                    b.Property<byte[]>("Hash")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("DataHash");

                    b.Property<bool>("IsMutation");

                    b.Property<byte[]>("Key");

                    b.Property<string>("Meta");

                    b.Property<byte[]>("PreviousDefinitionHash");

                    b.Property<byte[]>("Signature");

                    b.HasKey("Hash");

                    b.HasIndex("DataHash")
                        .IsUnique()
                        .HasFilter("[DataHash] IS NOT NULL");

                    b.HasIndex("PreviousDefinitionHash")
                        .IsUnique()
                        .HasFilter("[PreviousDefinitionHash] IS NOT NULL");

                    b.ToTable("Definitions");
                });

            modelBuilder.Entity("Amnesia.Domain.Entity.State", b =>
                {
                    b.Property<byte[]>("CurrentBlockHash");

                    b.HasKey("CurrentBlockHash");

                    b.ToTable("State");
                });

            modelBuilder.Entity("Amnesia.Domain.Entity.Block", b =>
                {
                    b.HasOne("Amnesia.Domain.Entity.Content", "Content")
                        .WithOne("Block")
                        .HasForeignKey("Amnesia.Domain.Entity.Block", "ContentHash");

                    b.HasOne("Amnesia.Domain.Entity.Block", "PreviousBlock")
                        .WithMany()
                        .HasForeignKey("PreviousBlockHash");
                });

            modelBuilder.Entity("Amnesia.Domain.Entity.Data", b =>
                {
                    b.HasOne("Amnesia.Domain.Entity.Definition")
                        .WithOne()
                        .HasForeignKey("Amnesia.Domain.Entity.Data", "PreviousDefinitionHash");
                });

            modelBuilder.Entity("Amnesia.Domain.Entity.Definition", b =>
                {
                    b.HasOne("Amnesia.Domain.Entity.Data", "Data")
                        .WithOne()
                        .HasForeignKey("Amnesia.Domain.Entity.Definition", "DataHash");

                    b.HasOne("Amnesia.Domain.Entity.Definition", "PreviousDefinition")
                        .WithOne()
                        .HasForeignKey("Amnesia.Domain.Entity.Definition", "PreviousDefinitionHash");
                });

            modelBuilder.Entity("Amnesia.Domain.Entity.State", b =>
                {
                    b.HasOne("Amnesia.Domain.Entity.Block", "CurrentBlock")
                        .WithMany()
                        .HasForeignKey("CurrentBlockHash")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}