using DataAccess.Services;
using DomainModel.ViewModel.IAdvertisement;
using Microsoft.AspNetCore.Mvc;
using NewSite.FrameworkUI.Services;
using NewSite.ViewModels.News;

namespace NewSite.Areas.Admin.Controllers;

public class AdminAdvertisementController(IAdvertisementRepository repository, IFileManager fileManager) : BaseAdminController
{
    public async Task<IActionResult> Index()
    {
        var result = await repository.GetAll();

        return View(result);
    }


    public async Task<IActionResult> Create()
    {

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AdvertisementAddViewModel viewModel)
    {
        var dbFileName = "";
        var op = fileManager.SaveFile(viewModel.Picture, "NewsImages/Advertisement");
        if (op.Success)
        {
            dbFileName = op.Message;
        }
        var model = new AdvertisementAddEditModel
        {
            Title = viewModel.Title,
            ImageUrl = dbFileName,
            LinkUrl = viewModel.LinkUrl,
            Alt = viewModel.Alt,
            IsDefault = viewModel.IsDefault,
        };
        if (ModelState.IsValid)
        {
            var add = await repository.Add(model);
            if (add.Success)
            {
                return RedirectToAction("Index", "AdminAdvertisement");
            }
            TempData["ErrorMessage"] = add.Message;
            return View(viewModel);
        }
        TempData["ErrorMessage"] = "خطایی در عمیلات پیش اومد";
        return View(viewModel);
    }


    public async Task<IActionResult> Delete(int id)
    {
        var model = await repository.Get(id);
        var path = fileManager.ToPhysicalAddress(Path.GetFileName(model.ImageUrl), "NewsImages/Advertisement");
        var op = await repository.Delete(id);
        if (op.Success)
        {
            var removeFile = fileManager.RemoveFile(path);
            if (removeFile)
            {
                return RedirectToAction("Index", "AdminAdvertisement");
            }

            TempData["ErrorMessage"] = "عملیات با خطا مواجه شد";
        }
        TempData["ErrorMessage"] = op.Message;
        return RedirectToAction("Index", "AdminAdvertisement");
    }
    

    public async Task<IActionResult> Edit(int id)
    {
        var model = await repository.Get(id);

        var viewModel = new AdvertisementEditViewModel
        {
            Id = model.Id,
            Title = model.Title,
            LinkUrl = model.LinkUrl,
            Alt = model.Alt,
            IsDefault = model.IsDefault
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AdvertisementEditViewModel viewModel)
    {
        var old=await repository.Get(viewModel.Id);
        var oldDbFileName = old.ImageUrl;
        var physicalAddress = fileManager.ToPhysicalAddress(Path.GetFileName(oldDbFileName), "NewsImages/Advertisement");

        if (viewModel.Picture==null)
        {
            if (ModelState.IsValid)
            {
                var addEditModel = new AdvertisementAddEditModel
                {
                    Id = viewModel.Id,
                    Title = viewModel.Title,
                    ImageUrl = oldDbFileName,
                    LinkUrl = viewModel.LinkUrl,
                    Alt = viewModel.Alt,
                    IsDefault = viewModel.IsDefault
                };
                var op = await repository.Edit(addEditModel);
                if (op.Success)
                {
                    return RedirectToAction("Index", "AdminAdvertisement");
                }
                TempData["ErrorMessage="] = op.Message;
                return View(viewModel);
            }
            TempData["ErrorMessage="] = "مشکلی در عملیات پیش اومد";
            return View(viewModel);
        }
        else
        {
            var operationResult = fileManager.SaveFile(viewModel.Picture, "NewsImages/Advertisement");
            if (operationResult.Success)
            {
                oldDbFileName = operationResult.Message;
            }
            var addEditModel = new AdvertisementAddEditModel
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                ImageUrl = oldDbFileName,
                LinkUrl = viewModel.LinkUrl,
                Alt = viewModel.Alt,
                IsDefault = viewModel.IsDefault
            };
            var op=await repository.Edit(addEditModel);
            if (op.Success)
            {
                var removeFile = fileManager.RemoveFile(physicalAddress);
                if (removeFile)
                {
                    return RedirectToAction("Index", "AdminAdvertisement");
                }
                TempData["ErrorMessage"] = "could not delete file";
                return View(viewModel);
            }
            TempData["ErrorMessage"] = op.Message;
            return View(viewModel);
        }
    }
}

