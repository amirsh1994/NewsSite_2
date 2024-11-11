using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewSite.ViewModels.Security;
using Security;

namespace NewSite.Areas.Admin.Controllers;

public class RoleManagementController(RoleManager<ApplicationRole> roleManager, SecurityDbContext db) : BaseAdminController
{
    public IActionResult Index()
    {
        var roles = roleManager.Roles.ToList();
        return View(roles);
    }

    public async Task<IActionResult> AddNewRole()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddNewRole(RoleAddEditModel rolVm)
    {
        var roleApplication = new ApplicationRole()
        {
            Description = rolVm.Description,
            Name = rolVm.RoleName
        };
        IdentityResult createRoleResult = await roleManager.CreateAsync(roleApplication);

        if (createRoleResult.Succeeded)
        {
            return RedirectToAction("Index");
        }
        foreach (var error in createRoleResult.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(rolVm);

    }


    public async Task<IActionResult> DeleteRole(int roleId)
    {
        var roleName = db.Roles.SingleOrDefault(x => x.Id == roleId)!.Name;
        var applicationRole = await roleManager.FindByNameAsync(roleName!);
        IdentityResult deleteRoleResult = await roleManager.DeleteAsync(applicationRole!);
        if (deleteRoleResult.Succeeded)
        {
            return RedirectToAction("Index", "RoleManagement");
        }
        foreach (var error in deleteRoleResult.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View();
    }



    public async Task<IActionResult> EditRole(int roleId)
    {
        var roleName = db.Roles.SingleOrDefault(x => x.Id == roleId)!.Name;
        var roleApplication = await roleManager.FindByNameAsync(roleName!);
        var roleAddEditModel = new RoleAddEditModel
        {
            RoleId = roleApplication!.Id,
            RoleName = roleApplication.Name!,
            Description = roleApplication.Description
        };
        return View(roleAddEditModel);
    }


    [HttpPost]
    public async Task<IActionResult> EditRole(RoleAddEditModel newRoleAddEditModel)
    {
        var oldRoleName = db.Roles.SingleOrDefault(x => x.Id == newRoleAddEditModel.RoleId)!.Name;
        var oldRole = await roleManager.FindByNameAsync(oldRoleName!);
        oldRole!.Name=newRoleAddEditModel.RoleName;
        oldRole.Description=newRoleAddEditModel.Description;

        IdentityResult updateRoleResult = await roleManager.UpdateAsync(oldRole);
        if (updateRoleResult.Succeeded)
        {
            return RedirectToAction("Index", "RoleManagement");
        }
        foreach (var error in updateRoleResult.Errors)
        {
            ModelState.AddModelError("",error.Description);
        }
        return View(newRoleAddEditModel);
    }

}

