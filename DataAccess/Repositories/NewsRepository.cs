using DataAccess.Services;
using DomainModel.Models;
using DomainModel.Models.Framework;
using DomainModel.ViewModel.Categories;
using DomainModel.ViewModel.News;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class NewsRepository(NewsDbContext db) : INewsRepository
{
    public async Task<OperationResult> Add(NewsAddEditModel addEditModel)
    {
        var op = new OperationResult();
        if (await ExistsSlug(addEditModel.Slug))
        {
            return op.ToError("duplicated slug");
        }

        if (await ExistsNewsTitle(addEditModel.NewsTitle))
        {
            return op.ToError("duplicated newsTitle");
        }

        try
        {
            var model = addEditModel.ToModel();
            db.News.Add(model);
            await db.SaveChangesAsync();
            return op.ToSuccess("saved successfully");
        }
        catch (Exception ex)
        {
            return op.ToError(ex.Message);
        }
    }

    public async Task<OperationResult> Delete(int id)
    {
        var op = new OperationResult();
        var news = await db.News.FirstOrDefaultAsync(x => x.NewsId == id);
        if (news == null)
            return op.ToError("news not found");

        try
        {
            db.Remove(news);
            await db.SaveChangesAsync();
            return op.ToSuccess("remove successfully");
        }
        catch (Exception e)
        {
            return op.ToError(e.Message);
        }
    }

    public async Task<NewsAddEditModel> Get(int id)
    {
        var model = await db.News.FirstOrDefaultAsync(x => x.NewsId == id);

        return model.ToViewModel();
    }

    public async Task<List<NewsSearchResult>> GetAll()
    {
        return await db.News.Select(x => new NewsSearchResult
        {
            NewsId = x.NewsId,
            NewsTitle = x.NewsTitle,
            Slug = x.Slug,
            ImageUrl = x.ImageUrl,
            NewsText = x.NewsText,
            NewsCategoryName = x.NewsCategory.CategoryName,
            VisitCount = x.VisitCount
        }).ToListAsync();

    }


    public async Task<OperationResult> Update(NewsAddEditModel newAddEditModel)
    {
        var op = new OperationResult();
        var model = await db.News.FirstOrDefaultAsync(x => x.NewsTitle == newAddEditModel.NewsTitle);

        if (model != null && newAddEditModel.NewsId != model.NewsId)
        {
            return op.ToError("this title belongs to another news");
        }
        model = await db.News.FirstOrDefaultAsync(x => x.Slug == newAddEditModel.Slug);

        if (model != null && newAddEditModel.NewsId != model.NewsId)
        {
            return op.ToError("this slug belongs to another news");
        }

        model = await db.News.FirstOrDefaultAsync(x => x.NewsId == newAddEditModel.NewsId);

        if (model == null)
            return op.ToError("not found");
        try
        {
            model.Slug = newAddEditModel.Slug;
            model.NewsCategoryId = newAddEditModel.NewsCategoryId;
            model.SortOrder = newAddEditModel.SortOrder;
            model.NewsText = newAddEditModel.NewsText;
            model.NewsTitle = newAddEditModel.NewsTitle;
            model.VisitCount = newAddEditModel.VisitCount;
            model.PublishedAt = newAddEditModel.PublishedAt;
            model.SmallDescription = newAddEditModel.SmallDescription;
            model.VoteSummation = newAddEditModel.VoteSummation;
            model.ImageUrl=newAddEditModel.ImageUrl;
            model.VoteCount=newAddEditModel.VoteCount;
            model.IsSpecial=newAddEditModel.IsSpecial;
            await db.SaveChangesAsync();
            return op.ToSuccess("updated successfully");
        }
        catch (Exception e)
        {
            return op.ToError(e.Message);
        }


    }

    public async Task<NewsListComplexSearchResult> Search(NewsSearchModel sm)
    {
        var r = new NewsListComplexSearchResult();
        var q = from item in db.News select item;

        if (string.IsNullOrEmpty(sm.Text) == false)
        {
            q = q.Where(x => x.NewsText.StartsWith(sm.Text));
        }
        if (string.IsNullOrEmpty(sm.NewsTitle) == false)
        {
            q = q.Where(x => x.NewsTitle.StartsWith(sm.NewsTitle));
        }
        if (string.IsNullOrEmpty(sm.Slug) == false)
        {
            q = q.Where(x => x.Slug.StartsWith(sm.Slug));
        }

        if (sm.NewsCategoryId is > 0)
        {
            q = q.Where(x => x.NewsCategoryId == sm.NewsCategoryId);
        }

        r.RecordCount = await q.CountAsync();
        r.Results = await
            q.OrderByDescending(x => x.SortOrder)
            .Skip(sm.PageIndex * sm.PageSize)
            .Take(sm.PageSize)
            .Select(x => new NewsSearchResult
            {
                NewsId = x.NewsId,
                NewsTitle = x.NewsTitle,
                Slug = x.Slug,
                ImageUrl = x.ImageUrl,
                NewsText = x.NewsText,
                NewsCategoryName = x.NewsCategory.CategoryName,
                VisitCount = x.VisitCount
            }).ToListAsync();
        r.sm = new NewsSearchModel();
        return r;
    }

    public async Task<bool> ExistsNewsTitle(string title)
    {
        return await db.News.AnyAsync(x => x.NewsTitle == title);
    }

    public async Task<bool> ExistsSlug(string slug)
    {
        return await db.News.AnyAsync(x => x.Slug == slug);
    }

    public async Task<OperationResult> SetNoImage(int id)
    {
        var op = new OperationResult();

        var news = await db.News.FirstOrDefaultAsync(x => x.NewsId == id);
        if (news==null)
        {
            return op.ToError("news not found");
        }
        news.ImageUrl = $"~/NoImage/noImage.jpg";
        await db.SaveChangesAsync();
        return op.ToSuccess("changed successfully");
    }
}

public static class NewsMapper
{
    public static NewsAddEditModel ToViewModel(this News? viewModel)
    {
        return new NewsAddEditModel
        {
            NewsId = viewModel.NewsId,
            NewsTitle = viewModel.NewsTitle,
            Slug = viewModel.Slug,
            SmallDescription = viewModel.SmallDescription,
            ImageUrl = viewModel.ImageUrl,
            NewsText = viewModel.NewsText,
            NewsCategoryId = viewModel.NewsCategoryId,
            PublishedAt = viewModel.PublishedAt,
            SortOrder = viewModel.SortOrder,
            VisitCount = viewModel.VisitCount,
            VoteSummation = viewModel.VoteSummation,
            VoteCount = viewModel.VoteCount,
            IsSpecial = false
        };
    }

    public static News ToModel(this NewsAddEditModel viewModel)
    {
        return new News()
        {
            NewsId = viewModel.NewsId,
            NewsTitle = viewModel.NewsTitle,
            Slug = viewModel.Slug,
            SmallDescription = viewModel.SmallDescription,
            ImageUrl = viewModel.ImageUrl,
            NewsText = viewModel.NewsText,
            NewsCategoryId = viewModel.NewsCategoryId,
            PublishedAt = DateTime.Now,
            SortOrder = viewModel.SortOrder,
            VisitCount = viewModel.VisitCount,
            VoteSummation = viewModel.VoteSummation,
            VoteCount = viewModel.VoteCount,
            IsSpecial = viewModel.IsSpecial
        };
    }
}