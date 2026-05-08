using System.Collections.Generic;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Web.Models;

public class UserUpdateModel
{
    public bool IsSiteAdministrator { get; set; }
    public bool ApprovedCommenter { get; set; }
    public string About { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisplayName { get; set; }
    public IDictionary<long, RoleType.Id> Roles { get; set; }
}
