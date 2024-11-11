using DataAccess.Services;
using DomainModel.ViewModel.News;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace NewSite.ViewComponents;

[ViewComponent(Name = "NewsList")]
public class NewsListViewComponent(INewsRepository repository, IMemoryCache memoryCache) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(NewsSearchModel sm)
    {
        //var resultSearch = await memoryCache.GetOrCreateAsync("main-page", entry =>
        //{
        //    entry.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(20);
        //    return repository.Search(sm);
        //});

        var resultSearch = await repository.Search(sm);
        //var r = resultSearch?.Results;
        if (sm.PageSize==0)
        {
            sm.PageSize = 1;
        }

        sm.RecordCount = resultSearch.RecordCount;
        if (sm.RecordCount%sm.PageSize==0)
        {
            sm.PageCount = sm.RecordCount / sm.PageSize;
        }
        else
        {
            sm.PageCount = sm.RecordCount / sm.PageSize + 1;
        }

        var c = new NewsListComplexSearchResult
        {
            RecordCount = resultSearch.RecordCount,
            Results = resultSearch.Results,
            sm = sm
        };
        return View(c);
    }
}