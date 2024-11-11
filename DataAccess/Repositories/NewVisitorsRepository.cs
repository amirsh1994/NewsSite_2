using DataAccess.Services;
using DomainModel.Models;
using DomainModel.ViewModel.News;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class NewVisitorsRepository(NewsDbContext db) : INewsVisitorsRepository
{
    public NewsAddEditModel GetNewsBySlug(string slug)
    {
        var q = db.News.FirstOrDefault(x => x.Slug == slug)?.ToViewModel();
        return q;
    }

    public async Task<NewsVisitorsSearchComplexResult> GetNewsByCategorySlugAsync(NewsVisitorsSearchModel sm)
    {
        var complexModel = new NewsVisitorsSearchComplexResult();

        var cat = await db.Categories.FirstOrDefaultAsync(x => x.Slug == sm.CategorySlug);

        var q = db.News.Where(x => x.NewsCategoryId == cat!.NewsCategoryId);

        complexModel.RecordCount = await q.CountAsync();

        complexModel.Items = await q.OrderByDescending(x => x.NewsId).Skip(sm.PageIndex * sm.PageSize).Take(sm.PageSize)
            .Select(x => new NewsVisitorsListItem
            {
                Slug = x.Slug,
                SmallDescription = x.SmallDescription,
                NewsTitle = x.NewsTitle
            }).AsNoTrackingWithIdentityResolution().ToListAsync();
        return complexModel;
    }

    public async Task<List<NewsVisitorsListItem>> GetTopTenLastNews(string? phrase)
    {
        var q = db.News.AsQueryable();
        if (string.IsNullOrWhiteSpace(phrase) == false)
        {
            q = q.Where(x => x.SmallDescription.Contains(phrase));
        }

        var result =await q.OrderByDescending(x => x.NewsId).Take(10).Select(x => new NewsVisitorsListItem
        {
            Slug = x.Slug,
            SmallDescription = x.SmallDescription,
            NewsTitle = x.NewsTitle,
            ImageUrl = x.ImageUrl
        }).ToListAsync();
        return result;
    }

    public async Task<List<NewsVisitorsListItem>> GetLatestTwoNewsNews()
    {
        return await db.News.OrderByDescending(x => x.NewsId).Take(2).Select(x => new NewsVisitorsListItem
        {
            NewsId = x.NewsId,
            Slug = x.Slug,
            SmallDescription = x.SmallDescription,
            NewsTitle = x.NewsTitle,
            ImageUrl = x.ImageUrl,
            NewsText = x.NewsText
        }).ToListAsync();
    }

    public List<NewsVisitorsListItem> GetLatestNews(int recordCount)
    {
        return db.News.OrderByDescending(x => x.NewsId).Take(recordCount).Select(x => new NewsVisitorsListItem
        {
            Slug = x.Slug,
            SmallDescription = x.SmallDescription,
            NewsTitle = x.NewsTitle,
            ImageUrl = x.ImageUrl
        }).ToList();
    }

    public List<NewsVisitorsListItem> GetMostViewedNews(int recordCount)
    {
        return db.News.OrderByDescending(x => x.VisitCount).Take(recordCount).Select(x => new NewsVisitorsListItem
        {
            Slug = x.Slug,
            SmallDescription = x.SmallDescription,
            NewsTitle = x.NewsTitle
        }).ToList();
    }

    public List<NewsVisitorsListItem> GetHottestNews(int recordCount)
    {
        return db.News.OrderByDescending(x => x.VoteCount).Take(recordCount).Select(x => new NewsVisitorsListItem
        {
            Slug = x.Slug,
            SmallDescription = x.SmallDescription,
            NewsTitle = x.NewsTitle
        }).ToList();


    }

    public async Task<NewsVisitorsListItem> GetTopOneSpecialNews()
    {
        var result = await db.News.FirstOrDefaultAsync(x => x.IsSpecial);

        return result != null ? new NewsVisitorsListItem
        {
            NewsId = result.NewsId,
            Slug = result.Slug,
            SmallDescription = result.SmallDescription,
            NewsTitle = result.NewsTitle,
            ImageUrl = result.ImageUrl
        } : new NewsVisitorsListItem();
    }
}

