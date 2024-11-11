using DataAccess.Services;
using DomainModel.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class MenuVisitorsRepository(NewsDbContext db):IMenuVisitorsRepository
{
    public async Task<List<NewsCategory>> GetAllWithChildren()
    {
        var result = await db.Categories
            .Where(x=>x.ParentId==null)
            .Include(x => x.Children)
            .AsSplitQuery()
            .ToListAsync();
        return result;
    }
}