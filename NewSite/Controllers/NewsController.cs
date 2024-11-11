using DataAccess.Repositories;
using DataAccess.Services;
using DomainModel.ViewModel.Categories;
using DomainModel.ViewModel.News;
using Framework.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using NewSite.ViewModels.News;
namespace NewSite.Controllers;

public class NewsController(INewsRepository newsRepository, INewsCategoryRepository categoryRepository, IHostEnvironment env, IMemoryCache memoryCache,IXmlLogService log) : Controller
{
    private async Task InflateCategory()
    {
        var cats = await categoryRepository.GetCategoryListItem();
        cats.Insert(0, new CategoryListItems() { CategoryId = -1, CategoryName = "..please select" });
        ViewBag.Categories = new SelectList(cats, "CategoryId", "CategoryName");
    }

    public async Task<IActionResult> Index(NewsSearchModel sm)
    {

        await InflateCategory();
        return View(sm);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await DeleteFileImageIfExists(id);
        var op = await newsRepository.Delete(id);
        return RedirectToAction("Index", "News");
    }


    public async Task<IActionResult> NewsListAction(NewsSearchModel sm)
    {
        //memoryCache.Remove("main-page");
        return ViewComponent("NewsList", sm);
    }


    public async Task<IActionResult> Add()
    {
        await InflateCategory();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(NewsAddViewModel v)
    {
        string dbFileName;

        // If no picture is uploaded, use the default "noImage.jpg"
        if (v.Picture == null)
        {
            dbFileName = "~/NoImage/noImage.jpg"; // Default image path
        }
        else
        {
            if (v.Picture.FileName.CheckFileName() == false)
            {
                return Json(new { Message = "File name is not valid" });
            }

            var fn = Path.GetFileName(v.Picture.FileName.ToUniqueFileName());
            dbFileName = $"~/NewsImages/{fn}";

            var path = Path.Combine(env.ContentRootPath, "wwwroot", "NewsImages");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filePath = Path.Combine(path, fn);
            await using var fs = new FileStream(filePath, FileMode.Create);
            await v.Picture.CopyToAsync(fs);
        }

        var model = new NewsAddEditModel
        {
            NewsTitle = v.NewsTitle,
            Slug = v.Slug,
            SmallDescription = v.SmallDescription,
            ImageUrl = dbFileName,  // Use the image or default
            NewsText = v.NewsText,
            NewsCategoryId = v.NewsCategoryId,
            PublishedAt = v.PublishedAt,
        };
        var op = await newsRepository.Add(model);

        if (op.Success && ModelState.IsValid) return Json(op);
        await log.GenerateXmlLog(new XmlModel
        {
            Exception = op.Message,
            Id = model.NewsId,
            Time = DateTime.Now
        });
        return Json(op);

    }


    public async Task<IActionResult> Edit(int id)
    {
        await InflateCategory();
        var model = await newsRepository.Get(id);
        var viewModel = new NewsEditViewModel
        {
            NewsId = id,
            NewsTitle = model.NewsTitle,
            Slug = model.Slug,
            SmallDescription = model.SmallDescription,
            NewsText = model.NewsText,
            NewsCategoryId = model.NewsCategoryId,
            PublishedAt = model.PublishedAt,
            
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(NewsEditViewModel v)
    {
        await InflateCategory();
        var old =await  newsRepository.Get(v.NewsId);
        if (v.Picture == null)
        {
            if (v.NewsCategoryId < 0)
            {
                TempData["Error"] = "...Please select Category...";
                return View(v);
            }

            var newsAddEditModel = new NewsAddEditModel
            {
                NewsTitle = v.NewsTitle,
                Slug = v.Slug,
                SmallDescription = v.SmallDescription,
                ImageUrl =old.ImageUrl ,
                NewsText = v.NewsText,
                NewsCategoryId = v.NewsCategoryId,
                PublishedAt = v.PublishedAt,
                NewsId = v.NewsId,
                SortOrder = 0,
                VisitCount = 0,
                VoteSummation = 0,
                VoteCount = 0
            };
            var op = await newsRepository.Update(newsAddEditModel);
            if (op.Success==false)
            {
                TempData["Error"] = $"{op.Message}";
            }
            return RedirectToAction("Index","News");
        }
        else
        {
            await DeleteFileImageIfExists(v.NewsId);

            if (v.Picture.FileName.CheckFileName()==false)
            {
                return new ObjectResult(new{Messgae="file name is not valid"});
            }

            if (v.NewsCategoryId<0)
            {
                TempData["Error"] = "...Please select Category...";
                return View(v);
            }

            var fn = Path.GetFileName(v.Picture.FileName.ToUniqueFileName());
            var dbFileName = $"~/NewsImages/{fn}";
            var path = Path.Combine(env.ContentRootPath, "wwwroot", "NewsImages");
            if (Directory.Exists(path)==false)
            {
                Directory.CreateDirectory(path);
            }

            var filePath = Path.Combine(path, fn);
            await using var fs = new FileStream(filePath,FileMode.Create);
            await v.Picture.CopyToAsync(fs);
            var model = new NewsAddEditModel
            {
                NewsId = v.NewsId,
                NewsTitle = v.NewsTitle,
                Slug = v.Slug,
                SmallDescription = v.SmallDescription,
                ImageUrl = dbFileName,
                NewsText = v.NewsText,
                NewsCategoryId = v.NewsCategoryId,
                PublishedAt = v.PublishedAt,
            };
            var op = await newsRepository.Update(model);
            if (op.Success==false)
            {
                TempData["Error"] = $"{op.Message}";
            }
            return RedirectToAction("Index", "News");
        }
    }

    #region IO
    private async Task DeleteFileImageIfExists(int id)
    {
        var news = await newsRepository.Get(id);
        if (string.IsNullOrWhiteSpace(news.ImageUrl)==false)
        {
            const string defaultImagePath = "~/NoImage/noImage.jpg";
            if (news.ImageUrl.ToLower()!=Path.GetFileName(defaultImagePath))
            {
                var path = env.ContentRootPath + @"/wwwroot" + news.ImageUrl.Substring(1, news.ImageUrl.Length - 1);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                } 
            }
        }
    }
    #endregion"

}

