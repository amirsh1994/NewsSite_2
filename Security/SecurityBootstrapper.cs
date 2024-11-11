using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Security;

public static class SecurityBootstrapper
{
    public static void InitSecurity(this IServiceCollection service,IConfiguration configuration)
    {
        service.AddDbContext<SecurityDbContext>(option =>
        {
            option.UseSqlServer(configuration.GetConnectionString("SecurityConnection"));
        },ServiceLifetime.Scoped);
    }
}