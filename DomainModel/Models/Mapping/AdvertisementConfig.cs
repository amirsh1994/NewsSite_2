using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModel.Models.Mapping;

public class AdvertisementConfig:IEntityTypeConfiguration<Advertisement>
{
    public void Configure(EntityTypeBuilder<Advertisement> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Alt).HasMaxLength(50);
        builder.Property(x => x.ImageUrl).HasMaxLength(250);
        builder.Property(x => x.LinkUrl).HasMaxLength(50);
        builder.Property(x => x.Title).HasMaxLength(50);
    }
}