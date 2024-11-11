using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Security;

public class SecurityDbContext(DbContextOptions<SecurityDbContext> options):IdentityDbContext<ApplicationUser, ApplicationRole,int>(options)
{
   

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>(x =>
        {
            x.HasKey(x => x.Id);
            x.Property(o => o.FirstName).HasMaxLength(100);
            x.Property(o => o.LastName).HasMaxLength(100);
            x.Property(o => o.NationalCode).HasMaxLength(100).IsRequired(false);
            x.Property(o => o.UserAddress).HasMaxLength(100).IsRequired(false);
            x.HasMany(o => o.ApplicationUserRoles)
                .WithOne(o => o.ApplicationUser)
                .HasForeignKey(o => o.ApplicationUserId);
        });
        builder.Entity<ApplicationRole>(o =>
        {
            o.Property(x => x.Description).HasMaxLength(100);
            o.HasKey(x => x.Id);
            o.HasMany(x => x.ApplicationUserRoles)
                .WithOne(x => x.ApplicationRole)
                .HasForeignKey(x => x.ApplicationRoleId);
        });

        builder.Entity<ApplicationUserRole>(o =>
        {
            o.HasKey(x => new { x.ApplicationUserId, x.ApplicationRoleId });
        });
        base.OnModelCreating(builder);
    }
}