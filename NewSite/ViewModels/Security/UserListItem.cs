using System.ComponentModel.DataAnnotations;

namespace NewSite.ViewModels.Security;

public class UserListItem
{
    public int UserId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string UserName { get; set; }

    public string RoleName { get; set; }

}

public class UserAddEditModel
{
    public int UserId { get; set; }

    [Display(Name = "نام")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string FirstName { get; set; }


    [Display(Name = "فامیلی")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string LastName { get; set; }

    [Display(Name = "ایمیل")]
    [EmailAddress(ErrorMessage = "ایمیل نامعتبر می باشد")]
    public string Email { get; set; }


    [Display(Name = "رمز عبور")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string Password { get; set; }

    [Display(Name = "تکرار رمز عبور")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [Compare(nameof(Password), ErrorMessage = "رمز عبور و تکرار رمز عبور یکسان نیستند")]
    public string Repassword { get; set; }


    public int RoleId { get; set; }

}

public class RoleListItem
{
    public int RoleId { get; set; }

    public string RoleName { get; set; }

}

public class RoleAddEditModel
{
    public int RoleId { get; set; }

    [Display(Name = "نام نقش")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string RoleName { get; set; }

    [Display(Name = "توضیحات")]
    public string Description { get; set; }

}

public class LoginViewModel
{
    [Display(Name = "نام کاربری(ایمیل)")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string UserName { get; set; }


    [Display(Name = "رمز عبور")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string Password { get; set; }


    public bool IsRememberMe { get; set; }


    public string? ReturnUrl { get; set; }

}

public class RegisterUserViewModel
{
    [Display(Name = "نام")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string FirstName { get; set; }


    [Display(Name = "نام خانوادگی")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string LastName { get; set; }


    [Display(Name = "رمز عبور")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string Password { get; set; }


    [Display(Name = "تکرار رمز عبور")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [Compare(nameof(Password), ErrorMessage = "رمز عبور وتکرارا ان یکسان نیستند")]
    public string Repasword { get; set; }


    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [EmailAddress(ErrorMessage = "ایمیل نامعتبر میباشد")]
    public string Email { get; set; }

    [Display(Name = "کدملی")]
    public string Nationalcode { get; set; }

    [Display(Name = "ادرس")]
    public string UserAddress { get; set; }

    [Display(Name = "شماره تلفن همراه")]
    public string MobileNumber { get; set; }
}

public class ChangePasswordViewModel
{
    [Display(Name = "کلمه عبور فعلی")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string CurrentPassword { get; set; }


    [Display(Name = "کللمه عبور جدید")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string NewPassword { get; set; }


    [Display(Name = "تکرار کلمه عبور جدید")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [Compare(nameof(NewPassword),ErrorMessage = "کلمه عبور جدید با تکرار یکسان نیست")]
    public string ConfirmedNewPassword { get; set; }
}

public class UserProfileAddEditModel
{

    [Display(Name = "نام")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string FirstName { get; set; }


    [Display(Name = "نام خانوادگی")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string LastName { get; set; }



    [Display(Name = "کدملی")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string NationalCode { get; set; }



    [Display(Name = "موبایل")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string phoneNumber { get; set; }


    [Display(Name = "ادرس کاربر")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string UserAddress { get; set; }
}