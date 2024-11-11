using Microsoft.AspNetCore.Mvc;

namespace NewSite.Areas.Admin.Controllers;

public class DashboardController : BaseAdminController
{
    public IActionResult Index()
    {
        return View();
    }
}

