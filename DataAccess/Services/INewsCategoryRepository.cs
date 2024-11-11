using DomainModel.Models;
using DomainModel.Models.Framework;
using DomainModel.ViewModel.Categories;
using DomainModel.ViewModel.News;

namespace DataAccess.Services;

public interface INewsCategoryRepository
{
    Task<OperationResult> Add(NewsCategoryAddEditModel addEditModel);

    Task<OperationResult> AddChild(NewsCategoryAddEditModel addEditModel,int parentId);

    Task<OperationResult> Delete(int categoryId);

    Task<NewsCategoryAddEditModel> Get(int categoryId);

    Task<List<NewsCategoryAddEditModel>> GetAll();

    Task<bool> HasRelatedChildren(int categoryId);

    Task<bool> HasRelatedNews(int categoryId);

    Task<OperationResult> Update(NewsCategoryAddEditModel newAddEditModel);

    Task<List<CategoryListItems>> GetCategoryListItem();

    List<NewsCategoryAddEditModel> GetRoots();

    List<NewsCategoryAddEditModel> GetSubCategories(int parentId);

    List<NewsSearchResult> Search(NewsSearchModel sm, out int recordCount);

}