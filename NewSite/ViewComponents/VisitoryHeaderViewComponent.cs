using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;

namespace NewSite.ViewComponents;

[ViewComponent(Name = "VisitoryHeader")]

public class VisitoryHeaderViewComponent(IAdvertisementRepository repository):ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = await repository.Get();
        return View(model);
    }
}