
using Domain.Categories;
using Domain.Common.Rules;

namespace Infrastructure.Data.Configs.Business;

internal sealed class CategoryConfig : BaseEntityConfig<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);


        builder.HasKey(e => e.Id)
               .IsClustered();

        builder.Property(x => x.Id)
               .HasConversion(id => id.Value, value => new CategoryId(value))
               .ValueGeneratedNever();

        builder.Property(x => x.Name)
               .HasMaxLength(CategoryRules.MaxCategoryNameLength)
               .IsRequired();

        builder.HasOne<Category>()
               .WithMany()
               .HasForeignKey(x => x.ParentCategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.Name)
               .IsUnique()
               .HasDatabaseName("IX_Categories_Name");

        builder.ToTable("Categories");

    }
}
