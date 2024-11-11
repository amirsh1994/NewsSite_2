namespace DomainModel.ViewModel.News;

public class NewsListComplexSearchResult
{
    public int RecordCount { get; set; }

    public List<NewsSearchResult> Results { get; set; }

    public NewsSearchModel  sm { get; set; }
}