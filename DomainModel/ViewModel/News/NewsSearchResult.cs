namespace DomainModel.ViewModel.News;

public class NewsSearchResult
{
    public int NewsId { get; set; }

    public string NewsTitle { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public string NewsText { get; set; } = string.Empty;

    public string NewsCategoryName { get; set; }

    public int VisitCount { get; set; }


}