using Microsoft.AspNetCore.Mvc;

namespace NewSite.ViewComponents;


[ViewComponent(Name = "VisitorsFooter")]
public class VisitorsFooterComponent:ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}