using DataAccess.Services;
using DomainModel.ViewModel.Categories;
using DomainModel.ViewModel.News;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewSite.FrameworkUI.Services;
using NewSite.ViewModels.News;

namespace NewSite.Areas.Admin.Controllers;

public class AdminNewsManagementController(INewsRepository repository, INewsCategoryRepository catRepository, IFileManager fileManager, IHostEnvironment env) : BaseAdminController
{
    private async Task InflateCategory()
    {
        var cats = await catRepository.GetAll();
        cats.Insert(0, new NewsCategoryAddEditModel() { NewsCategoryId = -1, CategoryName = "..please select..." });
        var selectList = new SelectList(cats, "NewsCategoryId", "CategoryName");
        ViewBag.drpCategory = selectList;
    }

    public async Task<IActionResult> Index(NewsSearchModel sm)
    {
        await InflateCategory();
        return View(sm);
    }

    public async Task<IActionResult> Add()
    {
        await InflateCategory();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(NewsAddViewModel news)
    {
        string dbFileName = "";

        if (news.Picture == null)
        {
            dbFileName = $"~/NoImage/noImage.jpg";
        }
        else
        {

            var result = fileManager.ValidateFileSize(news.Picture, 2048, 2097152);

            if (result.Success == false)
            {
                TempData["ErrorMessage"] = result.Message;
                await InflateCategory();
                return View(news);
            }

            if (fileManager.ValidateFileName(news.Picture.FileName) == false)
            {
                TempData["ErrorMessage"] = "فایل نامعتبر هست";
                await InflateCategory();
                return View(news);
            }

            var op = fileManager.SaveFile(news.Picture, "NewsImages");
            if (op.Success)
            {
                dbFileName = op.Message;
            }
        }
        if (news.NewsCategoryId < 0)
        {
            TempData["ErrorMessage"] = "دسته بندی اصلی را انتخاب نمایید";
            await InflateCategory();
            return View(news);
        }
        var model = new NewsAddEditModel
        {
            NewsTitle = news.NewsTitle,
            Slug = news.Slug,
            SmallDescription = news.SmallDescription,
            ImageUrl = dbFileName,
            NewsText = news.NewsText,
            NewsCategoryId = news.NewsCategoryId,
            PublishedAt = DateTime.Now,
            SortOrder = 0,
            VisitCount = 0,
            VoteSummation = 0,
            VoteCount = 0,
            IsSpecial = false
        };
        var operationResult = await repository.Add(model);
        if (operationResult.Success)
        {
            return RedirectToAction("Index", "AdminNewsManagement");
        }
        TempData["ErrorMessage"] = operationResult.Message;
        await InflateCategory();
        return View(news);
    }

    public async Task<IActionResult> Delete(int newsId)
    {
        var news = await repository.Get(newsId);
        var fileName = Path.GetFileName(news.ImageUrl);
        var physicalPath = fileManager.ToPhysicalAddress(fileName, "NewsImages");
        var op = await repository.Delete(newsId);
        var removeFile = fileManager.RemoveFile(physicalPath);
        if (op.Success && removeFile)
        {
            await repository.SetNoImage(newsId);
            return RedirectToAction("Index", "AdminNewsManagement");
        }

        TempData["ErrorMessage"] = op.Message;
        await InflateCategory();
        return RedirectToAction("Index", "AdminNewsManagement");
    }

    public async Task<IActionResult> DeleteImage(int newsId)
    {
        var news = await repository.Get(newsId);
        var fileName = Path.GetFileName(news.ImageUrl);
        var physicalPath = fileManager.ToPhysicalAddress(fileName, "NewsImages");
        var removeFile = fileManager.RemoveFile(physicalPath);
        if (removeFile)
        {
            await repository.SetNoImage(newsId);
            return RedirectToAction("Index", "AdminNewsManagement");
        }
        TempData["ErrorMessage"] = "مشکلی در عملیات حذف عکس پیش امد";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int newsId)
    {
        await InflateCategory();
        var old = await repository.Get(newsId);
        var editViewModel = new NewsEditViewModel
        {
            NewsId = old.NewsId,
            NewsTitle = old.NewsTitle,
            Slug = old.Slug,
            SmallDescription = old.SmallDescription,
            NewsText = old.NewsText,
            NewsCategoryId = old.NewsCategoryId,
            PublishedAt = old.PublishedAt,
            IsSpecial = old.IsSpecial
        };
        return View(editViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(NewsEditViewModel viewModel)
    {
        if (viewModel.NewsCategoryId < 0)
        {
            await InflateCategory();
            TempData["ErrorMessage"] = "لطفا نام دسته بندی رو انتخاب نمایید";
            return View(viewModel);
        }

        var oldNews = await repository.Get(viewModel.NewsId);
        var dbFileName = oldNews.ImageUrl;
        var fileName = Path.GetFileName(oldNews.ImageUrl);
        var physicalPath = fileManager.ToPhysicalAddress(fileName, "NewsImages");


        if (viewModel.Picture != null && fileName!="noImage.jpg")
        {
            if (fileManager.ValidateFileName(fileName) == false)
            {
                await InflateCategory();
                TempData["ErrorMessage"] = "پسوند فایل نا معتبر می باشد";
                return View(viewModel);
            }
            var validateFileSize = fileManager.ValidateFileSize(viewModel.Picture, 2048, 2097152);
            if (validateFileSize.Success == false)
            {
                await InflateCategory();
                TempData["ErrorMessage"] = "سایز فایل نا معتبر میباشد";
                return View(viewModel);

            }

            var removeFile = fileManager.RemoveFile(physicalPath);
            if (removeFile)
            {
                await repository.SetNoImage(viewModel.NewsId);
                var result = fileManager.SaveFile(viewModel.Picture, "NewsImages");
                if (result.Success)
                {
                }
            }
        }

        var saveFile = fileManager.SaveFile(viewModel.Picture!, "NewsImages");
        dbFileName = saveFile.Message;
        var model = new NewsAddEditModel
        {
            NewsId = viewModel.NewsId,
            NewsTitle = viewModel.NewsTitle,
            Slug = viewModel.Slug,
            SmallDescription = viewModel.SmallDescription,
            ImageUrl = dbFileName,
            NewsText = viewModel.NewsText,
            NewsCategoryId = viewModel.NewsCategoryId,
            PublishedAt = viewModel.PublishedAt,
            IsSpecial = viewModel.IsSpecial
        };
        var op = await repository.Update(model);
        if (op.Success)
        {
            return RedirectToAction("Index","AdminNewsManagement");
        }
        TempData["ErrorMessage"] = op.Message;
        return View(viewModel);
    }
}

