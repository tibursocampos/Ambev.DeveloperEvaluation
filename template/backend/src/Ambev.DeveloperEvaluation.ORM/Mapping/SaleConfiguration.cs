using Ambev.DeveloperEvaluation.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        // Configuração da chave primária herdada de BaseEntity
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.CustomerId).IsRequired().HasColumnType("int");
        builder.Property(s => s.SaleDate).IsRequired().HasColumnType("timestamp");

        // Configuração da relação com SaleItems
        builder.HasMany(s => s.Items)
               .WithOne()
               .HasForeignKey(si => si.SaleId);
    }
}

