using DataAccess.Services;
using DomainModel.Models;
using DomainModel.Models.Framework;
using DomainModel.ViewModel.Categories;
using DomainModel.ViewModel.News;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class NewsCategoryRepository(NewsDbContext db) : INewsCategoryRepository
{

    public async Task<int> Add(NewsCategoryAddEditModel addEditModel)
    {
        var model = addEditModel.ToModel();
        db.Categories.Add(model);
        await db.SaveChangesAsync();
        return model.NewsCategoryId;
    }

    public async Task<OperationResult> Delete(int categoryId)
    {
        var op = new OperationResult();

        var cat = await db.Categories.FirstOrDefaultAsync(x => x.NewsCategoryId == categoryId);

        if (cat==null)
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

    public async Task<bool> HasRelatedNews(int categoryId)
    {
        return await db.News.AnyAsync(x => x.NewsCategoryId == categoryId);
    }

    public async Task<bool> Update(NewsCategoryAddEditModel newAddEditModel)
    {
        try
        {
            var newModel = newAddEditModel.ToModel();
            db.Categories.Attach(newModel);
            db.Entry(newModel).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + ex.InnerException?.Message);
            return false;


        }
    }

    public async Task<List<CategoryListItems>> GetCategoryListItem()
    {
        return await db.Categories.Select(x => new CategoryListItems()
        {
            CategoryId = x.NewsCategoryId,
            NewsCount = x.News.Count,
            CategoryName = x.CategoryName,
            Slug = x.Slug
        }).ToListAsync();

    }
}


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