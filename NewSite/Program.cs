using BootStrap;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using NewSite.FrameworkUI;
using NewSite.FrameworkUI.Services;
using Security;

namespace NewSite;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.
        builder.Services.AddControllersWithViews();
        //configure newsDbContext
        builder.Services.WiredUp(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty);
        builder.Services.AddHsts(op =>
        {
            op.Preload = true;
            op.IncludeSubDomains = true;
            op.MaxAge = TimeSpan.FromDays(365);
        });
        builder.Services.AddScoped<IFileManager, FileManager>();
        //configure SecurityDbContext
        builder.Services.InitSecurity(builder.Configuration);

        //Configure Identity
        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
                options.Password.RequireDigit = false; // عدم نیاز به حداقل یک عدد
                options.Password.RequiredLength = 6; // حداقل طول رمز عبور
                options.Password.RequireNonAlphanumeric = false; // عدم نیاز به کاراکتر غیرحرفی
                options.Password.RequireUppercase = false; // عدم نیاز به حروف بزرگ
                options.Password.RequireLowercase = false; // عدم نیاز به حروف کوچک
                options.Password.RequiredUniqueChars = 1; // حداقل تعداد کاراکترهای یکتا
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
            }).AddEntityFrameworkStores<SecurityDbContext>()
            .AddDefaultTokenProviders();
        
        builder.Services.ConfigureApplicationCookie(option =>
        {
            option.ExpireTimeSpan = TimeSpan.FromDays(30); // کوکی ۳۰ روز معتبر بماند
            option.SlidingExpiration = false; // تمدید خودکار غیرفعال
            option.LoginPath = "/Account/Login";
            option.Cookie.Name = ".MyNewsWebSite";
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

        }
        app.UseHsts();
        app.UseHttpsRedirection();
        app.UseStaticFiles();


        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "Admin",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllerRoute
            (
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

        });
        app.Run();
    }
}

