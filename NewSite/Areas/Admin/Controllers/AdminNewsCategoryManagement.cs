using DataAccess.Services;
using DomainModel.ViewModel.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewSite.Areas.Admin.Controllers;

public class AdminNewsCategoryManagement(INewsCategoryRepository repository) : BaseAdminController
{
    private async Task BindRoot()
    {
        var roots = repository.GetRoots();
        roots.Insert(0, new NewsCategoryAddEditModel() { CategoryName = "دسته اصلی" });
        var drpParent = new SelectList(roots, "NewsCategoryId", "CategoryName");
        ViewBag.drpParent = drpParent;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> List()
    {
        var cats = await repository.GetCategoryListItem();
        return View(cats);
    }

    public async Task<IActionResult> Create(int parentId)
    {
        var model = new NewsCategoryAddEditModel { ParentId = parentId };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(NewsCategoryAddEditModel model)
    {
        if (model.ParentId == 0)
        {
            model.ParentId = null;
        }
        var op = await repository.Add(model);
        if (op.Success)
        {
            return RedirectToAction("Index", "AdminNewsCategoryManagement");
        }
        TempData["ErrorMessage"] = op.Message;
        return View(model);
    }

    public async Task<IActionResult> AddChild()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddChild(NewsCategoryAddEditModel model)
    {

        var op = await repository.Add(model);
        if (op.Success)
        {
            return RedirectToAction("Index", "AdminNewsCategoryManagement");
        }
        TempData["Error"] = op.Message;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var op = await repository.Delete(id);
        return Json(op);
    }


    public async Task<IActionResult> Edit(int newsCategoryId)
    {
        var old = await repository.Get(newsCategoryId);
        var model = new NewsCategoryAddEditModel
        {
            NewsCategoryId = old.NewsCategoryId,
            CategoryName = old.CategoryName,
            SmallDescription = old.SmallDescription,
            Slug = old.Slug,
            ParentId = old.ParentId
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(NewsCategoryAddEditModel model)
    {
        if (model.NewsCategoryId < 0)
        {
            TempData["ErrorMessage"] = "لطفا دسته بندی رو انتخا کنید";
            return View(model);
        }
        var op = await repository.Update(model);
        if (op.Success)
        {
            return RedirectToAction("Index", "AdminNewsCategoryManagement");
        }
        TempData["ErrorMessage"] = $"{op.Message}";
        return View(model);
    }
}