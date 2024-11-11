using DomainModel.Models;

namespace DataAccess.Services;

public interface IMenuVisitorsRepository
{
    Task<List<NewsCategory>> GetAllWithChildren();
}