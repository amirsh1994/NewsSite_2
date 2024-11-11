using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewSite.SecuritySearchUser;
using NewSite.ViewModels.Security;
using Security;

namespace NewSite.Controllers;

[Authorize(Roles = "UserVisitor")]
public class UserProfileController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var userName = User.Identity!.Name;
        var currentUser = await userManager.FindByNameAsync(userName!);

        return View(currentUser);
    }


    public async Task<IActionResult> ChangePassword()
    {

        return View();
    }


    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel cpVml)
    {
        var userName = User.Identity!.Name;
        var currentUser = await userManager.FindByNameAsync(userName!);
        IdentityResult changePasswordResult = await userManager.ChangePasswordAsync(currentUser!, cpVml.CurrentPassword, cpVml.NewPassword);
        if (changePasswordResult.Succeeded)
        {
            TempData["successChangePassword"] = $"Password changed successfully. Please log in again.";

            await signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }
        foreach (var error in changePasswordResult.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
        return View(cpVml);
    }


    public async Task<IActionResult> EditProfile()
    {
        var userName = User.Identity!.Name;
        var currentUser = await userManager.FindByNameAsync(userName!);
        var userProfileModel = new UserProfileAddEditModel
        {
            FirstName = currentUser!.FirstName,
            LastName = currentUser.LastName,
            NationalCode = currentUser!.NationalCode,
            phoneNumber = currentUser.PhoneNumber!,
            UserAddress = currentUser.UserAddress
        };
        return View(userProfileModel);
    }


    [HttpPost]
    public async Task<IActionResult> EditProfile(UserProfileAddEditModel newUserProfileModel)
    {
        var currentUser = await userManager.FindByNameAsync(User.Identity.Name);

        currentUser!.FirstName = newUserProfileModel.FirstName;
        currentUser.LastName = newUserProfileModel.LastName;
        currentUser.PhoneNumber = newUserProfileModel.phoneNumber;
        currentUser.NationalCode = newUserProfileModel.NationalCode;
        currentUser.UserAddress=newUserProfileModel.UserAddress;
        

        IdentityResult updateUserProfileResult = await userManager.UpdateAsync(currentUser);
        if (updateUserProfileResult.Succeeded)
        {
            return RedirectToAction("Index", "UserProfile");
        }
        foreach (var error in updateUserProfileResult.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
        return View(newUserProfileModel);
    }


    public async Task<IActionResult> Exit()
    {
        return View();
    }

}

