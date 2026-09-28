
using Domain.Brands;
using Domain.Common.Rules;

namespace Infrastructure.Data.Configs.Business;

internal sealed class BrandConfig : BaseEntityConfig<Brand>
{
    public override void Configure(EntityTypeBuilder<Brand> builder)
    {
        base.Configure(builder);

        builder.HasKey(e => e.Id)
               .IsClustered();

        builder.Property(x => x.Id)
               .HasConversion(
               id => id.Value,
               value => new BrandId(value))
               .ValueGeneratedNever();

        builder.Property(x => x.CompanyName)
               .HasColumnType("NVARCHAR")
               .HasMaxLength(BrandRules.MaxCompanyNameLength)
               .IsRequired();

        builder.Property(x => x.Name)
               .HasColumnType("NVARCHAR")
               .HasMaxLength(BrandRules.MaxNameLength)
               .IsRequired();

        builder.Property(x => x.Slug)
               .HasColumnType("VARCHAR")
               .HasMaxLength(BrandRules.MaxSkuNameLength)
               .IsRequired();

        builder.Property(x => x.Description)
               .HasColumnType("NVARCHAR")
               .HasMaxLength(BrandRules.MaxDescriptionLength)
               .IsRequired();

        builder.Property(x => x.LogoUrl)
               .HasColumnType("NVARCHAR")
               .HasMaxLength(255)
               .IsRequired(false);

        builder.HasIndex(x => x.Name)
               .IsUnique()
               .HasDatabaseName("IX_Brands_Name");

        builder.HasIndex(x => x.CompanyName)
               .IsUnique()
               .HasDatabaseName("IX_Brands_CompanyName");

        builder.HasIndex(x => x.Slug)
               .IsUnique()
               .HasDatabaseName("IX_Brands_Slug");

        builder.ToTable("Brands");
    }
}
