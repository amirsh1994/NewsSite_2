using Microsoft.AspNetCore.Identity;

namespace Security;

public class ApplicationUser:IdentityUser<int>
{

    public string FirstName { get; set; } 

    public string LastName { get; set; }

    public string NationalCode { get; set; }

    public string UserAddress { get; set; }

    public List<ApplicationUserRole> ApplicationUserRoles { get; set; } = [];
}

public class ApplicationRole:IdentityRole<int>
{
    public string Description { get; set; }

    public List<ApplicationUserRole> ApplicationUserRoles { get; set; } = [];
}

public class ApplicationUserRole
{

    public int ApplicationUserId { get; set; }

    public int ApplicationRoleId { get; set; }

    public ApplicationUser ApplicationUser { get; set; }

    public ApplicationRole ApplicationRole { get; set; }

}