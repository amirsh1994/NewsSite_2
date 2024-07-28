using DomainModel.Models;
using DomainModel.Models.Framework;
using DomainModel.ViewModel.Categories;
using DomainModel.ViewModel.News;

namespace DataAccess.Services;

public interface INewsCategoryRepository
{
    Task<int> Add(NewsCategoryAddEditModel addEditModel);

    Task<OperationResult> Delete(int categoryId);

    Task<NewsCategoryAddEditModel> Get(int categoryId);

    Task<List<NewsCategoryAddEditModel>> GetAll();

    Task<bool> HasRelatedChildren(int categoryId);

    Task<bool> HasRelatedNews(int categoryId);

    Task<bool> Update(NewsCategoryAddEditModel newAddEditModel);

    Task<List<CategoryListItems>> GetCategoryListItem();

    



}