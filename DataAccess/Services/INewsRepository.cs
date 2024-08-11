using DomainModel.Models.Framework;
using DomainModel.ViewModel.Categories;
using DomainModel.ViewModel.News;

namespace DataAccess.Services;

public interface INewsRepository
{
    Task<OperationResult> Add(NewsAddEditModel addEditModel);

    Task<OperationResult> Delete(int id);

    Task<NewsAddEditModel> Get(int id);

    Task<List<NewsSearchResults>> GetAll();

    Task<OperationResult> Update(NewsAddEditModel newAddEditModel);

    Task<List<NewsSearchResults>> Search(NewsSearchModel sm,ref int recordCount);

    Task<bool> ExistsNewsTitle(string title);

    Task<bool> ExistsSlug(string slug);




}