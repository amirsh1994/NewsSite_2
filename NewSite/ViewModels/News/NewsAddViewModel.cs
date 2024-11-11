using System.ComponentModel.DataAnnotations;
using Framework.FileValidation;

namespace NewSite.ViewModels.News;

public class NewsAddViewModel
{
    [Display(Name = "عنوان خبر")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string NewsTitle { get; set; } = string.Empty;


    [Display(Name = "اسلاگ خبر")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "توضیحات خبر")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string SmallDescription { get; set; } = string.Empty;



    [Display(Name = "عکس محصول")]
    [FileImage(ErrorMessage = "عکس نامعتبر می باشد")]
    public IFormFile?  Picture { get; set; }


    public string NewsText { get; set; } = string.Empty;


    public int NewsCategoryId { get; set; }


    public DateTime PublishedAt { get; set; }=DateTime.Now;

    public bool IsSpecial { get; set; }
}

public class NewsEditViewModel
{
    public int NewsId { get; set; }

    public string NewsTitle { get; set; } = string.Empty;

    [Display(Name = "اسلاگ خبر")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string Slug { get; set; } = string.Empty;

    public string SmallDescription { get; set; } = string.Empty;


    [Display(Name = "عکس محصول")]
    [FileImage(ErrorMessage = "عکس نامعتبر می باشد")]
    public IFormFile? Picture { get; set; }

    public string NewsText { get; set; } = string.Empty;

    public int NewsCategoryId { get; set; }

    public DateTime PublishedAt { get; set; }

    public bool IsSpecial { get; set; }
}