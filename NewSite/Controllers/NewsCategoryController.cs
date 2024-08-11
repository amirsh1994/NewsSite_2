using DataAccess.Services;
using DomainModel.Models.Framework;
using DomainModel.ViewModel.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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


    public async Task<IActionResult> Edit(int id)
    {
        var cat = await repository.Get(id);

        return View(cat);
    }

    [HttpPost]
    public async Task<JsonResult> Edit(NewsCategoryAddEditModel newModel)
    {

        var op = await repository.Update(newModel);
        return Json(op);

    }

    public async Task<IActionResult> Add()
    {
        BindRoot();
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> Add(NewsCategoryAddEditModel cat)
    {
        BindRoot();
        if (ModelState.IsValid)
        {
            if (cat.ParentId == 0)
            {
                cat.ParentId = null;
            }
            var op = await repository.Add(cat);
            return Json(op);

        }

        return Json( new OperationResult().ToError("Add failed"));


    }

    private void BindRoot()
    {
        var roots = repository.GetRoots();
        roots.Insert(0, new NewsCategoryAddEditModel() { CategoryName = "دسته اصلی" });
        var drpParent = new SelectList(roots, "NewsCategoryId", "CategoryName");
        ViewBag.drpParent = drpParent;
    }

    private void BindChildren(int parentId)
    {
        var roots = repository.GetSubCategories(parentId);
        var drpSubCategories = new SelectList(roots, "NewsCategoryId", "CategoryName");
        ViewBag.drpSubCategories = drpSubCategories;
    }
}