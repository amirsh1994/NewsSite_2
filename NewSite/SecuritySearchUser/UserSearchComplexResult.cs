using NewSite.ViewModels.Security;

namespace NewSite.SecuritySearchUser;

public class UserSearchComplexResult
{
    public List<UserListItem> Items { get; set; } = [];

    public int RecordCount { get; set; }
}