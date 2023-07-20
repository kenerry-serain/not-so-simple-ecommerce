using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotSoSimpleEcommerce.Shared.Models;

namespace NotSoSimpleEcommerce.Order.Domain.Repositories.Configurations;

public class OrderEntityTypeConfiguration: IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.ToTable("Order");
        builder.HasKey(order => order.Id);
        builder
            .Property(order => order.Id)
            .HasColumnType("integer")
            .ValueGeneratedOnAdd();
            
        builder
            .Property(order => order.StatusId)
            .HasColumnType("integer")
            .HasConversion<int>()
            .IsRequired();
        
        builder
            .Property(order => order.Quantity)
            .HasColumnType("integer")
            .IsRequired();
        
        builder
            .Property(order => order.BoughtBy)
            .HasColumnType("text")
            .IsRequired();
        
        builder
            .Property(order => order.ProductId)
            .HasColumnType("integer")
            .IsRequired();
        
        builder
            .HasOne(order => order.Product)
            .WithOne()
            .HasForeignKey<OrderEntity>(order => order.ProductId);
            
        builder
            .HasOne(order => order.Status)
            .WithOne()
            .HasForeignKey<OrderEntity>(order => order.StatusId);
    }
}
