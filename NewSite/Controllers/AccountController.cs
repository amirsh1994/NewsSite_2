using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewSite.ViewModels.Security;
using Security;

namespace NewSite.Controllers;

public class AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : Controller
{
    public IActionResult Index()
    {
        return View();
    }


    public async Task<IActionResult> Login(string returnUrl = "")
    {
        var loginViewModel = new LoginViewModel()
        {
            ReturnUrl = returnUrl
        };
        return View(loginViewModel);
    }


    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel lvm)
    {
        if (ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(lvm.UserName, lvm.Password, lvm.IsRememberMe, false);

            if (result.Succeeded)
            {
                var currentUser = await userManager.FindByNameAsync(lvm.UserName);

                if (currentUser != null)
                {
                    var claims = new List<Claim> { new(ClaimTypes.Name, currentUser.UserName!) };
                    var roles = await userManager.GetRolesAsync(currentUser);
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                  
                    var claimIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(claimIdentity));
                }
                if (string.IsNullOrWhiteSpace(lvm.ReturnUrl) == false && Url.IsLocalUrl(lvm.ReturnUrl))
                {
                    return Redirect(lvm.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
        }
        ModelState.AddModelError("", "invalid login attempt");
        return View(lvm);
    }


    [HttpPost]
    public async Task<IActionResult> SignOut()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }


    public async Task<IActionResult> Register()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Register(RegisterUserViewModel rUvm)
    {
        var applicationUser = new ApplicationUser()
        {
            FirstName = rUvm.FirstName,
            LastName = rUvm.LastName,
            Email = rUvm.Email,
            UserName = rUvm.Email,
            NationalCode = rUvm.Nationalcode,
            UserAddress = rUvm.UserAddress,
            PhoneNumber = rUvm.MobileNumber
        };
        IdentityResult userRegistrationResult = await userManager.CreateAsync(applicationUser, rUvm.Password);

        if (userRegistrationResult.Succeeded)
        {
            var currentUser = await userManager.FindByNameAsync(rUvm.Email);
            await userManager.AddToRoleAsync(currentUser!, "UserVisitor");
            return RedirectToAction("Login", "Account");
        }
        foreach (var error in userRegistrationResult.Errors)
        {
            ModelState.AddModelError("", error.Description);
            return View(rUvm);
        }
        return View(rUvm);
    }



}

