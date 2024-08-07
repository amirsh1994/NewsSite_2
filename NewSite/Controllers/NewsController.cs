using Microsoft.AspNetCore.Mvc;

namespace NewSite.Controllers;

    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

