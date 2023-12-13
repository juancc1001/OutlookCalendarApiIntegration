using ProySuministros.DataAccess.Layer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProySuministros.DataAccess.Layer.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasMaxLength(50);
            builder.Property(x => x.Name).HasMaxLength(20);
            builder.Property(x => x.CreatedBy).HasMaxLength(200);
            builder.Property(x => x.UpdatedBy).HasMaxLength(200);
        }
    }
}
