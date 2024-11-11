using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;

namespace NewSite.ViewComponents;


[ViewComponent(Name = "NewsCategory")]
public class NewsCategoryViewComponent(IMenuVisitorsRepository repository) :ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = await repository.GetAllWithChildren();
        return View(model);
    }
}