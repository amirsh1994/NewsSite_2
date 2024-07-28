namespace DomainModel.Models;

public class NewsCategory
{
    public int NewsCategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public string SmallDescription { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public int ? ParentId { get; set; }

    public NewsCategory Parent { get; set; }

    public ICollection<NewsCategory> Children { get; set; } = [];

    public ICollection<News> News { get; set; } = [];

}