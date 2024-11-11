using Microsoft.EntityFrameworkCore;
using NewSite.ViewModels.Security;
using Security;

namespace NewSite.SecuritySearchUser;

public class UserRepository(SecurityDbContext db):IUserRepository
{
    public async Task<UserSearchComplexResult> SearchUser(UserSearchModel sm)
    {
        var complexModel = new UserSearchComplexResult();

        var q = from u in db.Users
                join ur in db.UserRoles
                    on u.Id equals ur.UserId
                join r in db.Roles
                    on ur.RoleId equals r.Id
                select new { User = u, userRole = ur, Role = r };

        if (string.IsNullOrWhiteSpace(sm.UserName) == false)
        {
            q = q.Where(x => x.User.UserName!.StartsWith(sm.UserName));
        }
        if (string.IsNullOrWhiteSpace(sm.FirstName) == false)
        {
            q = q.Where(x => x.User.FirstName!.StartsWith(sm.FirstName));
        }
        if (string.IsNullOrWhiteSpace(sm.LastName) == false)
        {
            q = q.Where(x => x.User.LastName!.StartsWith(sm.LastName));
        }
        if (string.IsNullOrWhiteSpace(sm.Email) == false)
        {
            q = q.Where(x => x.User.Email!.StartsWith(sm.Email));
        }
        complexModel.RecordCount = await q.CountAsync();
        complexModel.Items = await q.OrderByDescending(x => x.User.Id)
            .Skip(sm.PageSize * sm.PageIndex)
            .Take(sm.PageSize)
            .Select(x => new UserListItem
            {
                UserId = x.User.Id,
                FirstName = x.User.UserName,
                LastName = x.User.LastName,
                UserName = x.User.UserName,
                RoleName = x.Role.Name
            }).ToListAsync();
        return complexModel;
    }
}