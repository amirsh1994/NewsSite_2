using DataAccess.Services;
using DomainModel.ViewModel.News;
using Microsoft.AspNetCore.Mvc;

namespace NewSite.Areas.Admin.ViewComponents;

[ViewComponent(Name="AdminNewsList")]
public class AdminNewsListViewComponent(INewsRepository repository):ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(NewsSearchModel sm)
    {
        var result = await repository.Search(sm);
        if (sm.PageSize == 0)
        {
            sm.PageSize = 1;
        }

        sm.RecordCount = result.RecordCount;
        if (sm.RecordCount % sm.PageSize == 0)
        {
            sm.PageCount = sm.RecordCount / sm.PageSize;
        }
        else
        {
            sm.PageCount = sm.RecordCount / sm.PageSize + 1;
        }
        var c = new NewsListComplexSearchResult
        {
            RecordCount = result.RecordCount,
            Results = result.Results,
            sm = sm
        };
        return View(c);
    }
}