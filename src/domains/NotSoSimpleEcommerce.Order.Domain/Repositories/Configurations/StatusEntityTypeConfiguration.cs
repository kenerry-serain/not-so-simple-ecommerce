using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotSoSimpleEcommerce.Shared.Enums;
using NotSoSimpleEcommerce.Shared.Models;

namespace NotSoSimpleEcommerce.Order.Domain.Repositories.Configurations;

public class StatusEntityTypeConfiguration: IEntityTypeConfiguration<StatusEntity>
{
    public void Configure(EntityTypeBuilder<StatusEntity> builder)
    {
        builder.ToTable("Status");
        builder.HasKey(status => status.Id);
        builder
            .Property(status => status.Id)
            .HasColumnType("integer")
            .HasConversion<int>();

        builder
            .Property(status => status.Description)
            .HasColumnType("text")
            .IsRequired();
    }
}
