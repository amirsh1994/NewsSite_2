using DataAccess.Services;
using DomainModel.Models;
using DomainModel.Models.Framework;
using DomainModel.ViewModel.IAdvertisement;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class AdvertisementRepository(NewsDbContext db) : IAdvertisementRepository
{
    public async Task<OperationResult> Add(AdvertisementAddEditModel addEditModel)
    {
        var op = new OperationResult();
        try
        {
            var dbModel = addEditModel.ToModel();
            db.Advertisements.Add(dbModel);
            await db.SaveChangesAsync();
            return op.ToSuccess("saved successfully");
        }
        catch (Exception e)
        {
            return op.ToError(e.Message);
        }
    }

    public async Task<OperationResult> Edit(AdvertisementAddEditModel asAddEditModel)
    {
        var op = new OperationResult();
        var oldModel = await db.Advertisements.FirstOrDefaultAsync(x => x.Id == asAddEditModel.Id);
        if (oldModel == null)
        {
            return op.ToError("was not fount...!");
        }
        try
        {
            oldModel.ImageUrl = asAddEditModel.ImageUrl;
            oldModel.Alt = asAddEditModel.Alt;
            oldModel.IsDefault = asAddEditModel.IsDefault;
            oldModel.LinkUrl = asAddEditModel.LinkUrl;
            oldModel.Title = asAddEditModel.Title;
            await db.SaveChangesAsync();
            return op.ToSuccess("updated successfully");
        }
        catch (Exception ex)
        {
            return op.ToError($"failed {ex.Message}");
        }
    }

    public async Task<OperationResult> Delete(int id)
    {
        var op = new OperationResult();
        var dbModel = await db.Advertisements.FirstOrDefaultAsync(x => x.Id == id);
        if (dbModel == null)
        {
            return op.ToError("model not found");
        }
        try
        {
            db.Advertisements.Remove(dbModel);
            await db.SaveChangesAsync();
            return op.ToSuccess("remove successfully");
        }
        catch (Exception e)
        {
            return op.ToError(e.Message);
        }
    }

    public async Task<AdvertisementAddEditModel> Get()
    {
        //return new AdvertisementAddEditModel
        //{
        //    Id = 1,
        //    Title = "test",
        //    ImageUrl = $"~/NewsImages/Advertisement/kalbas.jpg",
        //    LinkUrl = $"https://pspro.ir/",
        //    Alt = "... An important news ...!",
        //    IsDefault = true
        //};
        var op = new OperationResult();
        var model = await db.Advertisements.OrderByDescending(x => x.Id).FirstOrDefaultAsync(x => x.IsDefault);
        return model != null ? model.ToViewModel() : new AdvertisementAddEditModel();
    }

    public async Task<AdvertisementAddEditModel> Get(int id)
    {
        var result = await db.Advertisements.FirstOrDefaultAsync(x => x.Id == id);
        return result != null ? result.ToViewModel():new AdvertisementAddEditModel();
    }

    public async Task<List<AdvertisementAddEditModel>> GetAll()
    {
        return await db.Advertisements.Select(x => new AdvertisementAddEditModel
        {
            Id = x.Id,
            Title = x.Title,
            ImageUrl = x.ImageUrl,
            LinkUrl = x.LinkUrl,
            Alt = x.Alt,
            IsDefault = x.IsDefault
        }).ToListAsync();
    }
}

public static class AdvertisementUtil
{
    public static AdvertisementAddEditModel ToViewModel(this Advertisement model)
    {
        return new AdvertisementAddEditModel
        {
            Id = model.Id,
            Title = model.Title,
            ImageUrl = model.ImageUrl,
            LinkUrl = model.LinkUrl,
            Alt = model.Alt,
            IsDefault = model.IsDefault
        };
    }

    public static Advertisement ToModel(this AdvertisementAddEditModel viewModel)
    {
        return new Advertisement
        {
            Id = viewModel.Id,
            Title = viewModel.Title,
            ImageUrl = viewModel.ImageUrl,
            LinkUrl = viewModel.LinkUrl,
            Alt = viewModel.Alt,
            IsDefault = false
        };
    }
}