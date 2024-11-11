using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewSite.ViewModels.Security;
using Security;

namespace NewSite.Areas.Admin.Controllers;

public class UserManagementController(
      UserManager<ApplicationUser> userManager
    , RoleManager<ApplicationRole> roleManager
    , SignInManager<ApplicationUser> signInManager
    , SecurityDbContext db
    ):BaseAdminController

{
    public async Task<IActionResult> Index()
    {
        var q = from u in db.Users
                join ur in db.UserRoles
                    on u.Id equals ur.UserId
                join r in db.Roles
                    on ur.RoleId equals r.Id
                select new UserListItem
                {
                    UserId = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserName = u.UserName,
                    RoleName = r.Name
                };
        var userListItems = await q.ToListAsync();
        return View(userListItems);
    }


    public async Task<IActionResult> AddNewUser()
    {
        InflateRole();
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> AddNewUser(UserAddEditModel userAddEditModel)
    {
        var applicationUser = new ApplicationUser
        {
            FirstName = userAddEditModel.FirstName,
            LastName = userAddEditModel.LastName,
            UserName = userAddEditModel.Email,
            Email = userAddEditModel.Email
        };
        IdentityResult userRegistrationResult = await userManager.CreateAsync(applicationUser, userAddEditModel.Password);
        IdentityResult? makeUserMemberOfRoleResult = null;
        if (userRegistrationResult.Succeeded)
        {
            var user = await userManager.FindByNameAsync(userAddEditModel.Email);
            var roleName = db.Roles.SingleOrDefault(x => x.Id == userAddEditModel.RoleId)!.Name;
            makeUserMemberOfRoleResult = await userManager.AddToRoleAsync(user!, roleName!);
        }
        if (userRegistrationResult.Succeeded && makeUserMemberOfRoleResult is { Succeeded: true })
        {
            return RedirectToAction("Index", "UserManagement");
        }
        foreach (var error in userRegistrationResult.Errors)
        {
            ModelState.AddModelError("", error.Description);
            InflateRole();
            return View(userAddEditModel);
        }
        if (makeUserMemberOfRoleResult is { Succeeded: false })
        {
            foreach (var error in makeUserMemberOfRoleResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
                InflateRole();
                return View(userAddEditModel);
            }
        }
        return View(userAddEditModel);
    }


   
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var userName = db.Users.SingleOrDefault(x => x.Id == userId)?.UserName;

        var currentUser = await userManager.FindByNameAsync(userName!);

        var userRole = await (from u in db.Users
                              join ur in db.UserRoles
                                  on u.Id equals ur.UserId
                              join r in db.Roles
                                  on ur.RoleId equals r.Id
                              select new { roleName = r.Name, userName = u.UserName }).ToListAsync();
        // [
        //   { roleName = "Admin", userName = "user1" },
        //   { roleName = "Editor", userName = "user1" },
        //...
        // ]
        foreach (var item in userRole)
        {
            await userManager.RemoveFromRoleAsync(currentUser!, item.roleName);
        }

        IdentityResult removeResult = await userManager.DeleteAsync(currentUser!);

        if (removeResult.Succeeded)
        {
            return RedirectToAction("Index", "UserManagement");
        }
        foreach (var error in removeResult.Errors)
        {
            ModelState.AddModelError("", error.Description);
            InflateRole();
        }
        return RedirectToAction("Index", "UserManagement");
    }


    public async Task<IActionResult> EditUser(int userId)
    {
        var user = await (from u in db.Users
                          join ur in db.UserRoles
                              on u.Id equals ur.UserId
                          join r in db.Roles
                              on ur.RoleId equals r.Id
                          select new UserAddEditModel
                          {
                              UserId = u.Id,
                              FirstName = u.FirstName,
                              LastName = u.LastName,
                              Email = u.Email,
                              RoleId = r.Id,
                          }).SingleOrDefaultAsync(x => x.UserId == userId);
        InflateRole();
        return View(user);
    }


    [HttpPost]
    public async Task<IActionResult> EditUser(UserAddEditModel newUseraddEditModel)
    {
        var oldApplicationUser = await userManager.FindByNameAsync(newUseraddEditModel.Email);

        var oldUseraddEditModel = await (from u in db.Users
            join ur in db.UserRoles
                on u.Id equals ur.UserId
            join r in db.Roles
                on ur.RoleId equals r.Id
            select new UserAddEditModel
            {
                UserId = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                RoleId = r.Id
            }).SingleOrDefaultAsync(x => x.UserId == newUseraddEditModel.UserId);

        if (newUseraddEditModel.RoleId!=oldUseraddEditModel?.RoleId)
        {
            var oldRoleName = GetRoleNameByRoleId(oldUseraddEditModel!.RoleId);
            var newRoleNAme = GetRoleNameByRoleId(newUseraddEditModel.RoleId);
            await userManager.RemoveFromRoleAsync(oldApplicationUser!, oldRoleName!);
            await userManager.AddToRoleAsync(oldApplicationUser!, newRoleNAme!);
        }

        oldApplicationUser!.FirstName = newUseraddEditModel.FirstName;
        oldApplicationUser.LastName = newUseraddEditModel.LastName;
        oldApplicationUser.Email = newUseraddEditModel.Email;
       // var checkPasswordAsync = await userManager.CheckPasswordAsync(oldApplicationUser,newUseraddEditModel.Password);
        IdentityResult updateUserIdentityResult = await userManager.UpdateAsync(oldApplicationUser);
        if (updateUserIdentityResult.Succeeded )
        {
            return RedirectToAction("Index", "UserManagement");
        }
        foreach (var error in updateUserIdentityResult.Errors)
        {
                ModelState.AddModelError("",error.Description);
        }

        return View(newUseraddEditModel);
    }
    #region PrivateMethodes

    private void InflateRole()
    {
        var roles = db.Roles.Select(x => new RoleListItem
        {
            RoleId = x.Id,
            RoleName = x.Name!
        }).ToList();
        roles.Insert(0, new RoleListItem
        {
            RoleId = -1,
            RoleName = "نقش کاربر را انتخاب نمایید"
        });
        var selectList = new SelectList(roles, "RoleId", "RoleName");
        ViewBag.roleList = selectList;
    }

    private string? GetRoleNameByRoleId(int roleId)
    {
        return db.Roles.SingleOrDefault(x => x.Id == roleId)?.Name;
    }


    #endregion

}

