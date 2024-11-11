using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;

namespace NewSite.ViewComponents;


[ViewComponent(Name = "LastTwoNews")]
public class LastTwoNewsViewComponent(INewsVisitorsRepository repository):ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var latestTwoNews = await repository.GetLatestTwoNewsNews();
        return View(latestTwoNews);
    }
}