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

        var q = db.News.Where(x => x.NewsCategoryId == cat.NewsCategoryId);

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


    public List<NewsVisitorsListItem> GetLatestNews(int recordCount)
    {
        return db.News.OrderByDescending(x => x.NewsId).Take(recordCount).Select(x => new NewsVisitorsListItem
        {
            Slug = x.Slug,
            SmallDescription = x.SmallDescription,
            NewsTitle = x.NewsTitle
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
}

