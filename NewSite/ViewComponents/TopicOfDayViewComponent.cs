using Microsoft.AspNetCore.Mvc;

namespace NewSite.ViewComponents;


[ViewComponent(Name = "TopicOfDay")]
public class TopicOfDayViewComponent:ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}