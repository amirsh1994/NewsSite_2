using DomainModel.Models.Framework;
using DomainModel.ViewModel.Categories;
using DomainModel.ViewModel.News;

namespace DataAccess.Services;

public interface INewsRepository
{
    Task<OperationResult> Add(NewsAddEditModel addEditModel);

    Task<OperationResult> Delete(int id);

    Task<NewsAddEditModel> Get(int id);

    Task<List<NewsSearchResult>> GetAll();

    

    Task<OperationResult> Update(NewsAddEditModel newAddEditModel);

    Task<NewsListComplexSearchResult> Search(NewsSearchModel sm);

    Task<bool> ExistsNewsTitle(string title);

    Task<bool> ExistsSlug(string slug);

    Task<OperationResult> SetNoImage(int id);
}