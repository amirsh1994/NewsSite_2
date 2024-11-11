using DomainModel.ViewModel.News;

namespace DataAccess.Services;

public interface INewsVisitorsRepository
{
    NewsAddEditModel GetNewsBySlug(string slug);

    Task<NewsVisitorsSearchComplexResult> GetNewsByCategorySlugAsync(NewsVisitorsSearchModel sm);

    Task<List<NewsVisitorsListItem>> GetTopTenLastNews(string?  phrase);

    Task<NewsVisitorsListItem> GetTopOneSpecialNews();

    Task<List<NewsVisitorsListItem>> GetLatestTwoNewsNews();

    List<NewsVisitorsListItem> GetLatestNews(int recordCount);

    List<NewsVisitorsListItem> GetMostViewedNews(int recordCount);

    List<NewsVisitorsListItem> GetHottestNews(int recordCount);
}