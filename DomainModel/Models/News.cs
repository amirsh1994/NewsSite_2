namespace DomainModel.Models;

public class News
{
    public int NewsId { get; set; }

    public string NewsTitle { get; set; } = string.Empty;

    public string SmallDescription { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public string NewsText { get; set; } = string.Empty;

    public int NewsCategoryId { get; set; }

    public NewsCategory NewsCategory { get; set; }

    public DateTime PublishedAt { get; set; }

    public int SortOrder { get; set; }

    public int VisitCount { get; set; }

    public int VoteSummation { get; set; }

    public int VoteCount { get; set; }

    public bool IsSpecial { get; set; }
}