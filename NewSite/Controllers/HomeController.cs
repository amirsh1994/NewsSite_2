using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;

namespace NewSite.Controllers;

public class HomeController(INewsVisitorsRepository repository):Controller
{
    public async Task<IActionResult> Index(string? phrase)
    {
        var news =await repository.GetTopTenLastNews(phrase);
        return View("~/views/Home/Index.cshtml",news);
    }

    public  IActionResult News(string id)
    {
        var news=repository.GetNewsBySlug(id);
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}