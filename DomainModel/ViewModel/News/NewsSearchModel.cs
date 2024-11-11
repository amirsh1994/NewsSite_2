namespace DomainModel.ViewModel.News;

public class NewsSearchModel:PageModel
{
    public int ? NewsCategoryId { get; set; }

    public string Slug { get; set; }

    public string NewsTitle { get; set; }

    public string Text { get; set; }
    
}