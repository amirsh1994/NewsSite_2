using Microsoft.AspNetCore.Mvc;

namespace NewSite.ViewComponents;


[ViewComponent(Name = "SpecialNews")]
public class SpecialNewsViewComponent:ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}