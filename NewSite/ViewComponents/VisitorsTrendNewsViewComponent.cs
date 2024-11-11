using Microsoft.AspNetCore.Mvc;

namespace NewSite.ViewComponents;

[ViewComponent(Name = "VisitorsTrendNews")]
public class VisitorsTrendNewsViewComponent:ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}