#pragma warning disable CS0618
using Domain.Brands;
using Domain.Categories;
using Domain.Common.Rules;
using Domain.ProductGroups;
using Domain.ProductGroups.Products;
using Domain.ProductGroups.ValueObjects;
using Infrastructure.Common.EfCore.ValueComparers;
using Infrastructure.Common.EfCore.ValueConverters;
namespace Infrastructure.Data.Configs.Business;

internal sealed class ProductGroupsConfig : BaseEntityConfig<ProductGroup>
{

    public override void Configure(EntityTypeBuilder<ProductGroup> builder)
    {
        base.Configure(builder);

        builder.Ignore(x => x.Specifications);


        builder.HasKey(e => e.Id)
               .IsClustered();

        builder.Property(x => x.Id)
               .HasConversion(
                   id => id.Value,
                   value => new ProductGroupId(value)
               )
               .ValueGeneratedNever();

        builder.Property(x => x.Title)
               .HasColumnType("NVARCHAR")
               .HasMaxLength(ProductGroupRules.MaxTitleLength * 2)
               .IsRequired();

        builder.Property(x => x.NormalizedTitle)
               .HasColumnType("NVARCHAR")
               .HasMaxLength(ProductGroupRules.MaxTitleLength * 2)
               .IsRequired();

        builder.Property(x => x.BrandName)
               .HasColumnType("NVARCHAR(100)")
               .IsRequired();

        builder.Property(x => x.CategoryName)
               .HasColumnType("NVARCHAR(100)")
               .IsRequired();

        builder.Property(x => x.Description)
               .HasColumnType("NVARCHAR")
               .HasMaxLength(ProductGroupRules.MaxDescriptionLength * 2)
               .IsRequired();

        builder.Property(x => x.AverageRating)
               .HasConversion(
                   averageRating => averageRating.Value,
                   value => new ProductAverageRating(value)
               )
               .ValueGeneratedNever();

        builder.Property("_specifications")
               .HasColumnName("Specifications")
               .HasColumnType("NVARCHAR(MAX)")
               .HasConversion<JsonConverter<Dictionary<string, string>>>(new JsonDictionaryValueComparer())
               .IsRequired(false);

        builder.Property(x => x.Status)
               .HasConversion<int>()
               .IsRequired();

        builder.HasOne<Brand>()
               .WithMany()
               .HasForeignKey(x => x.BrandId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();

        builder.HasOne<Category>()
               .WithMany()
               .HasForeignKey(x => x.CategoryId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();

        builder.HasOne(x => x.MainProduct)
               .WithMany()
               .HasForeignKey(x => x.MainProductId)
               .IsRequired(false);

        builder.HasIndex(x => x.MainProductId)
               .IsUnique()
               .HasDatabaseName("UX_ProductGroup_FeaturedProductId");

        builder.HasIndex(x => new
        {
            x.Status,
            x.NormalizedTitle
        })
        .HasDatabaseName("IX_ProductGroups_Search")
        .HasFilter($"[{nameof(ProductGroup.MainProductId)}] IS NOT NULL")
        .IncludeProperties(x => new
        {
            x.Id,
            x.MainProductId,
            x.Title,
            x.BrandName,
            x.AverageRating
        });


        builder.ToTable("ProductGroups", x =>
        {
            x.HasCheckConstraint("CK_ProductGroups_AverageRating", $"[AverageRating] between {ProductGroupRules.MinAverageRatingValue} and {ProductGroupRules.MaxAverageRatingValue}");

        });
    }
}
