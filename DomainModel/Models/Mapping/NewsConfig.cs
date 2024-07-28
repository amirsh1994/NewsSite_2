using DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModel.Models.Mapping;

public class NewsConfig : IEntityTypeConfiguration<News>
{
    public void Configure(EntityTypeBuilder<News> builder)
    {
        builder.HasKey(x => x.NewsId);
        builder.Property(x => x.SmallDescription).HasMaxLength(400);
        builder.Property(x => x.Slug).HasMaxLength(100);
        builder.Property(x => x.ImageUrl).HasMaxLength(100);
        builder.Property(x => x.NewsText).HasMaxLength(500);
        builder.Property(x => x.NewsTitle).HasMaxLength(200);

    }
}