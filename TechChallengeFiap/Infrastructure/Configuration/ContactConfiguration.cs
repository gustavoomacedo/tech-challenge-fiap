﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TechChallengeFiap.Models;

namespace TechChallengeFiap.Infrastructure.Configuration
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contact");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnType("INT").ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(u => u.DataCriacao).HasColumnType("DATETIME").IsRequired();
            builder.Property(u => u.Name).HasColumnType("VARCHAR(500)").IsRequired();
            builder.Property(u => u.Email).HasColumnType("VARCHAR(500)").IsRequired();
            builder.Property(u => u.DDD).HasColumnType("INT").IsRequired();
            builder.Property(u => u.Telefone).HasColumnType("INT").IsRequired();
        }
    }
}