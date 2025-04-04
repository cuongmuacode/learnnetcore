namespace LearnNetCore.Web.Models;
public class EditUserRolesViewModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }

    public List<RoleCheckbox> Roles { get; set; }
}

public class RoleCheckbox
{
    public string RoleName { get; set; }
    public bool IsSelected { get; set; }
}

