using DomainModel.ViewModel.News;

namespace DataAccess.Services;

public interface INewsVisitorsRepository
{
    NewsAddEditModel GetNewsBySlug(string slug);

    Task<NewsVisitorsSearchComplexResult> GetNewsByCategorySlugAsync(NewsVisitorsSearchModel sm);

    List<NewsVisitorsListItem> GetLatestNews(int recordCount);

    List<NewsVisitorsListItem> GetMostViewedNews(int recordCount);

    List<NewsVisitorsListItem> GetHottestNews(int recordCount);






}