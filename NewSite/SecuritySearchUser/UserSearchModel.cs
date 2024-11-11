using DomainModel.ViewModel;

namespace NewSite.SecuritySearchUser;

public class UserSearchModel : PageModel
{
    public string? UserName { set; get; }

    public string? FirstName { set; get; }

    public string? LastName { set; get; }

    public string? Email { get; set; }
}