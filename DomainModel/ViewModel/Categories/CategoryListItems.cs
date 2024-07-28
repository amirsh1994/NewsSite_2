namespace DomainModel.ViewModel.Categories;

public class CategoryListItems
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public int NewsCount { get; set; }
}
