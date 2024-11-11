using DomainModel.Models.Framework;
using DomainModel.ViewModel.IAdvertisement;

namespace DataAccess.Services;

public interface IAdvertisementRepository
{
    Task<OperationResult> Add(AdvertisementAddEditModel addEditModel);

    Task<OperationResult> Edit(AdvertisementAddEditModel asAddEditModel);

    Task<OperationResult> Delete(int id);

    Task<AdvertisementAddEditModel> Get();

    Task<AdvertisementAddEditModel> Get(int id);

    Task<List<AdvertisementAddEditModel>> GetAll();

}