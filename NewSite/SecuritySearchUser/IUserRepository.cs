namespace NewSite.SecuritySearchUser;

public interface IUserRepository
{
    Task<UserSearchComplexResult> SearchUser(UserSearchModel sm);
}