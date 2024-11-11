using DataAccess.Services;
using DomainModel.Models;
using DomainModel.Models.Framework;
using DomainModel.ViewModel.Categories;
using DomainModel.ViewModel.News;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class NewsCategoryRepository(NewsDbContext db) : INewsCategoryRepository
{

    #region Repositories

    public async Task<OperationResult> Add(NewsCategoryAddEditModel addEditModel)
    {
        var op = new OperationResult();
        try
        {
            var model = addEditModel.ToModel();
            db.Categories.Add(model);
            await db.SaveChangesAsync();
            return op.ToSuccess("saved successfully");
        }
        catch (Exception e)
        {
            return op.ToError($"save has failed {e.Message}");

        }
    }

    public async Task<OperationResult> AddChild(NewsCategoryAddEditModel addEditModel, int parentId)
    {
        var op = new OperationResult();
        var model=addEditModel.ToModel();
        model.ParentId=parentId;
        try
        {
            db.Categories.Add(model);
            await db.SaveChangesAsync();
            return op.ToSuccess("saved successfully");
        }
        catch (Exception e)
        {
            return op.ToError("Error" + e.Message);
        }
    }

    public async Task<OperationResult> Delete(int categoryId)
    {
        var op = new OperationResult();

        var cat = await db.Categories.FirstOrDefaultAsync(x => x.NewsCategoryId == categoryId);

        if (cat == null)
        {
            return op.ToError("not exists");
        }
        if (await HasRelatedChildren(categoryId))
        {
            return op.ToError("Has child category");
        }

        if (await HasRelatedNews(categoryId))
        {
            return op.ToError("has related news");
        }


        try
        {

            db.Categories.Remove(cat);
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            return op.ToError("remove failed");
        }

        return op.ToSuccess("remove successfully");

    }

    public async Task<NewsCategoryAddEditModel> Get(int categoryId)
    {
        var model = await db.Categories.FirstAsync(x => x.NewsCategoryId == categoryId);
        return model.ToAddEditModel();
    }

    public async Task<List<NewsCategoryAddEditModel>> GetAll()
    {
        var result = await db.Categories.Select(x => x.ToAddEditModel()).ToListAsync();
        return result;
    }

    public async Task<bool> HasRelatedChildren(int categoryId)
    {
        var addEditModel = await Get(categoryId);
        var model = addEditModel.ToModel();
        return model.Children.Any(x => x.ParentId == categoryId);

    }

    public Task<bool> HasRelatedNews(int categoryId)
    {
        return db.News.AnyAsync(x => x.NewsCategoryId == categoryId);
    }

    public async Task<OperationResult> Update(NewsCategoryAddEditModel newAddEditModel)
    {
        var op = new OperationResult();
        try
        {
            var newModel = newAddEditModel.ToModel();
            db.Categories.Attach(newModel);
            db.Entry(newModel).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return op.ToSuccess("updated successfully");
        }
        catch (Exception ex)
        {

            return op.ToError("update failed" + ex.Message + ex.StackTrace);


        }
    }

    public Task<List<CategoryListItems>> GetCategoryListItem()
    {
        return db.Categories
            .Where(x=>x.ParentId==null)
            .Select(x => new CategoryListItems()
        {
            CategoryId = x.NewsCategoryId,
            NewsCount = x.News.Count,
            CategoryName = x.CategoryName,
            Slug = x.Slug,
            ParentId = x.ParentId,
            Children =x.Children
        }).ToListAsync();

    }

    public List<NewsCategoryAddEditModel> GetRoots()
    {
        var results = db.Categories.Where(x => x.ParentId == null).Select(x => x.ToAddEditModel());
        return results.ToList();
    }

    public List<NewsCategoryAddEditModel> GetSubCategories(int parentId)
    {
        return db.Categories.Where(x => x.ParentId == parentId).Select(x => x.ToAddEditModel()).ToList();
    }

    public List<NewsSearchResult> Search(NewsSearchModel sm, out int recordCount)
    {
        var q = from item in db.News select item;
        if (sm.NewsCategoryId != null)
        {
            q = q.Where(x => x.NewsCategoryId == sm.NewsCategoryId);
        }

        if (!string.IsNullOrWhiteSpace(sm.NewsTitle))
        {
            q = q.Where(x => x.NewsTitle.StartsWith(sm.NewsTitle));
        }

        if (!string.IsNullOrWhiteSpace(sm.Slug))
        {
            q = q.Where(x => x.Slug.StartsWith(sm.Slug));
        }

        if (!string.IsNullOrWhiteSpace(sm.Text))
        {
            q = q.Where(x => x.NewsText.StartsWith(sm.Text));
        }

        recordCount = q.Count();
        sm.RecordCount=recordCount;
        var result = q.Skip(sm.PageIndex * sm.PageSize).Take(sm.PageSize).Select(x => new NewsSearchResult
        {
            NewsId = x.NewsId,
            NewsTitle = x.NewsTitle,
            Slug = x.Slug,
            ImageUrl = x.ImageUrl,
            NewsText = x.NewsText,
            NewsCategoryName = x.NewsCategory.CategoryName,
            VisitCount = x.VisitCount
        });
        return  result.ToList();
    }
    #endregion
}

#region Mapping

public static class CatMapper
{
    public static NewsCategory ToModel(this NewsCategoryAddEditModel addEditModel)
    {
        return new NewsCategory()
        {
            NewsCategoryId = addEditModel.NewsCategoryId,
            CategoryName = addEditModel.CategoryName,
            ParentId = addEditModel.ParentId,
            Slug = addEditModel.Slug,
            SmallDescription = addEditModel.SmallDescription,
        };
    }

    public static NewsCategoryAddEditModel ToAddEditModel(this NewsCategory model)
    {
        return new NewsCategoryAddEditModel()
        {
            NewsCategoryId = model.NewsCategoryId,
            ParentId = model.ParentId,
            CategoryName = model.CategoryName,
            Slug = model.Slug,
            SmallDescription = model.SmallDescription,
        };
    }
}

#endregion
