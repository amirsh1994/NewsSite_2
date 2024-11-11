using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;

namespace NewSite.Controllers;

    public class NewsVisitorsController (INewsVisitorsRepository repository): Controller
    {
        public  IActionResult NewsDetails(string slug)
        {
            var news =  repository.GetNewsBySlug(slug);
            return View(news);
        }

        public async Task<IActionResult> ContactUs()
        {
            return View();
        }

        public async Task<IActionResult> AboutUs()
        {
            return View();
        }
    }

