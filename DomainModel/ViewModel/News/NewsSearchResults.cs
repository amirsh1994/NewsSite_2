namespace DomainModel.ViewModel.News;

public class NewsSearchResults
{
    public int NewsId { get; set; }

    public string NewsTitle { get; set; } = string.Empty;

    public string SmallDescription { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public string NewsText { get; set; } = string.Empty;

    public int NewsCategoryId { get; set; }

    public DateTime PublishedAt { get; set; }

    public int VisitCount { get; set; }

    public int VoteSummation { get; set; }

    public int VoteCount { get; set; }

}