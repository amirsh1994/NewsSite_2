using System.ComponentModel.DataAnnotations;
using DomainModel.Models;

namespace DomainModel.ViewModel.News;

public class NewsAddEditModel
{
    public int NewsId { get; set; }

    [Display(Name = "عنوان خبر")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [MaxLength(20,ErrorMessage = "عنوان خبر حدااکثر 20 میباشد")]
    public string NewsTitle { get; set; } = string.Empty;


    [Display(Name = "اسلاگ خبر")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string Slug { get; set; } = string.Empty;


    public string SmallDescription { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public string NewsText { get; set; } = string.Empty;

    public int NewsCategoryId { get; set; }

    public DateTime PublishedAt { get; set; }

    public int SortOrder { get; set; }

    public int VisitCount { get; set; }

    public int VoteSummation { get; set; }

    public int VoteCount { get; set; }
}