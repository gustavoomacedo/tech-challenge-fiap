﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TechChallengeFiap.Infrastructure.Repository;

#nullable disable

namespace TechChallengeFiap.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241017204714_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TechChallengeFiap.Models.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DDD")
                        .HasColumnType("INT");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("DATETIME");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("VARCHAR(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR(500)");

                    b.Property<int>("Telefone")
                        .HasColumnType("INT");

                    b.HasKey("Id");

                    b.ToTable("Contact", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}