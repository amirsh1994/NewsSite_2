using DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModel.Models.Mapping;

public class CategoryConfig : IEntityTypeConfiguration<NewsCategory>
{
    public void Configure(EntityTypeBuilder<NewsCategory> builder)
    {
        builder.HasKey(x => x.NewsCategoryId);
        builder.Property(x => x.CategoryName).HasMaxLength(200);
        builder.Property(x => x.Slug).HasMaxLength(20);
        builder.Property(x => x.SmallDescription).HasMaxLength(400);
        builder
            .HasMany(x => x.Children)
            .WithOne(x => x.Parent)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasMany(x => x.News)
            .WithOne(x => x.NewsCategory)
            .HasForeignKey(x => x.NewsCategoryId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}