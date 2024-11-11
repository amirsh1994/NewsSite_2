using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;

namespace NewSite.ViewComponents;


[ViewComponent(Name = "TopOneSpecialNews")]
public class TopOneSpecialNewsViewComponent(INewsVisitorsRepository repository):ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var specialNews = await repository.GetTopOneSpecialNews();
        return View(specialNews);
    }
}