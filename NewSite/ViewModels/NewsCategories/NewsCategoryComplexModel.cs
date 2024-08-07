using DomainModel.ViewModel.Categories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewSite.ViewModels.NewsCategories;

public class NewsCategoryComplexModel
{
    public NewsCategoryAddEditModel NewsCategoryAddEditModel { get; set; }

    public List<CategoryListItems> ItemsList { get; set; }

    public NewsCategoryAddEditModel EditModel { get; set; }

    public SelectList DrpParentId { get; set; }

    
}