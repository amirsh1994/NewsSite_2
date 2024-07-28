using System.ComponentModel.DataAnnotations;

namespace DomainModel.ViewModel.News;

public class NewsCategoryAddEditModel
{
    public int NewsCategoryId { get; set; }

    [Required(ErrorMessage = "نام را وارد کنید")]
    [Display(Name = "نام رده ")]
    [EmailAddress(ErrorMessage = "")]

    public string CategoryName { get; set; } = string.Empty;

    [MinLength(5, ErrorMessage = "حداقل 5 کاراکتر را وارد کنید"), MaxLength(255, ErrorMessage = "حداکثر 250 کاراکتر را وترد کنید")]
    public string SmallDescription { get; set; } = string.Empty;


    public string Slug { get; set; } = string.Empty;

    public int? ParentId { get; set; }

}