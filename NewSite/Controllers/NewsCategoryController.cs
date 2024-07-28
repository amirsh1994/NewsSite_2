using DataAccess.Services;
using DomainModel.ViewModel;
using DomainModel.ViewModel.News;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewSite.ViewModels.NewsCategories;

namespace NewSite.Controllers;

public class NewsCategoryController(INewsCategoryRepository repository) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }


    public async Task<IActionResult> List()
    {
        var cats = await repository.GetCategoryListItem();
        return View(cats);
    }


    [HttpPost]
    public async Task<JsonResult> Delete(int id)
    {
        var op = await repository.Delete(id);
        return Json(op);
    }

    public async Task<IActionResult> Get(int id)
    {
        var cat = await repository.Get(id);
        return Json(cat);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var cat =await repository.Get(id);
       
        return View(cat);
    }
}