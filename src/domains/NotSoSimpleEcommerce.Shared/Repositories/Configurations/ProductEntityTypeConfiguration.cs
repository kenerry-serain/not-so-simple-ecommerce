using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotSoSimpleEcommerce.Shared.Models;

namespace NotSoSimpleEcommerce.Shared.Repositories.Configurations;

public class ProductEntityTypeConfiguration: IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.ToTable("Product");
        builder.HasKey(product => product.Id);
        builder
            .Property(product => product.Id)
            .HasColumnType("integer")
            .ValueGeneratedOnAdd();
        
        builder
            .Property(order => order.Name)
            .HasColumnType("text")
            .IsRequired();

        builder
            .Property(product => product.Price)
            .HasColumnType("numeric")
            .IsRequired();
    }
}
