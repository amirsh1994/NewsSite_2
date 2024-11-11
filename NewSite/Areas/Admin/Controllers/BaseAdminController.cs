using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewSite.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class BaseAdminController : Controller
{

}

