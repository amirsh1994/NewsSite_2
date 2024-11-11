using System.ComponentModel.DataAnnotations;

namespace DomainModel.ViewModel.Categories;

public class NewsCategoryAddEditModel
{
    public int NewsCategoryId { get; set; }

    [Required(ErrorMessage = "نام را وارد کنید")]
    [Display(Name = "نام رده ")]
    public string CategoryName { get; set; } = string.Empty;

    [Display(Name = "توضیحات")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [MinLength(5, ErrorMessage = "حداقل 5 کاراکتر را وارد کنید"), MaxLength(255, ErrorMessage = "حداکثر 250 کاراکتر را وترد کنید")]
    public string SmallDescription { get; set; } = string.Empty;

    [Display(Name = "اسلاگ")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "دسته بندی اصلی")]
    public int? ParentId { get; set; }

}