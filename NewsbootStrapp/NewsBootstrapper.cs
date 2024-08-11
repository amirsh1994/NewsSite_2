using DataAccess.Repositories;
using DataAccess.Services;
using DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BootStrap;

public static class NewsBootstrapper
{
    public static void WiredUp(this IServiceCollection service,string cnnr)
    {
        service.AddDbContext<NewsDbContext>(option =>
        {
            option.UseSqlServer(cnnr);
        },contextLifetime:ServiceLifetime.Scoped);

        service.AddScoped<INewsCategoryRepository, NewsCategoryRepository>();

        service.AddScoped<INewsVisitorsRepository, NewVisitorsRepository>();
    }

}