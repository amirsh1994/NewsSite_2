using System.ComponentModel.DataAnnotations;

namespace NewSite.ViewModels.News;

public class AdvertisementAddViewModel
{
    public int Id { get; set; }

    [Display(Name = "عنوان تبلیغ")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string Title { get; set; } = "";

    [Display(Name = "عکس")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public IFormFile Picture { get; set; }

    [Display(Name = "لینک")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string LinkUrl { get; set; } = "";

    public string Alt { get; set; } = "";

    [Display(Name = "تبلیغ پیشفرض")]
    public bool IsDefault { get; set; }
}

public class AdvertisementEditViewModel
{
    public int Id { get; set; }

    [Display(Name = "عنوان تبلیغ")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string Title { get; set; }

    [Display(Name = "عکس")]
    public IFormFile? Picture { get; set; }

    [Display(Name = "لینک")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string LinkUrl { get; set; }

    [Display(Name = "alt")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string Alt { get; set; }

    public bool IsDefault { get; set; }
}