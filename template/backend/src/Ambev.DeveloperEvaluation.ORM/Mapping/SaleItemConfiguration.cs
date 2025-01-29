﻿using Ambev.DeveloperEvaluation.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        // Configuração da chave primária herdada de BaseEntity
        builder.HasKey(si => si.Id);
        builder.Property(si => si.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(si => si.ProductId).IsRequired().HasColumnType("int");
        builder.Property(si => si.ProductName).IsRequired().HasMaxLength(100);
        builder.Property(si => si.Quantity).IsRequired().HasColumnType("int");
        builder.Property(si => si.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(si => si.Discount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(si => si.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");

        // Configuração da chave estrangeira para Sale
        builder.HasOne<Sale>()
               .WithMany(s => s.Items)
               .HasForeignKey(si => si.SaleId);
    }
}

