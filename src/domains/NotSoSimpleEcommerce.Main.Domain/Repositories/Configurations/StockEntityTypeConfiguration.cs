using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotSoSimpleEcommerce.Shared.Models;

namespace NotSoSimpleEcommerce.Main.Domain.Repositories.Configurations;

public class StockEntityTypeConfiguration: IEntityTypeConfiguration<StockEntity>
{
    public void Configure(EntityTypeBuilder<StockEntity> builder)
    {
        builder.ToTable("Stock");
        builder.HasKey(stock => stock.Id);
        builder
            .Property(stock => stock.Id)
            .HasColumnType("integer")
            .ValueGeneratedOnAdd();
        
        builder
            .Property(stock => stock.ProductId)
            .HasColumnType("integer")
            .IsRequired();
        
        builder
            .Property(stock => stock.Quantity)
            .HasColumnType("integer")
            .IsRequired();

        builder
            .HasOne(stock => stock.Product)
            .WithOne()
            .HasForeignKey<StockEntity>(stock => stock.ProductId);
    }
}
