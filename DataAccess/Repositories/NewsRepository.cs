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

    public Task<NewsAddEditModel> Get(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<NewsSearchResults>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult> Update(NewsAddEditModel newAddEditModel)
    {
        throw new NotImplementedException();
    }

    public Task<List<NewsSearchResults>> Search(NewsSearchModel sm, ref int recordCount)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistsNewsTitle(string title)
    {
        return await db.News.AnyAsync(x => x.NewsTitle == title);
    }

    public async Task<bool> ExistsSlug(string slug)
    {
        return await db.News.AnyAsync(x => x.Slug == slug);
    }
}

public static class NewsMapper
{
    public static NewsAddEditModel ToViewModel(this News viewModel)
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
            VoteCount = viewModel.VoteCount
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
            VoteCount = viewModel.VoteCount
        };
    }
}