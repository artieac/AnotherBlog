using Microsoft.AspNetCore.Mvc;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;
using AlwaysMoveForward.AnotherBlog.Web.Models;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers.API;

[Route("api/[controller]")]
public class UsersController : BaseApiController
{
    public UsersController(ServiceManagerBuilder serviceManagerBuilder)
        : base(serviceManagerBuilder)
    {
    }

    [HttpGet]
    [WebAPIAuthorizationAttribute(RoleType.Names.SiteAdministrator, true)]
    public IEnumerable<AnotherBlogUser> Get()
    {
        return this.Services.UserService.GetAll();
    }

    [HttpGet("{id:int}")]
    [WebAPIAuthorizationAttribute(RoleType.Names.SiteAdministrator, true)]
    public AnotherBlogUser GetById(int id)
    {
        return this.Services.UserService.GetById(id);
    }

    [HttpGet("Current")]
    public AnotherBlogUser GetCurrent()
    {
        return this.CurrentPrincipal.CurrentUser;
    }

    [HttpPost("{id:int}")]
    [WebAPIAuthorizationAttribute(RoleType.Names.SiteAdministrator, true)]
    public AnotherBlogUser Post(int id, [FromBody] UserUpdateModel input)
    {
        AnotherBlogUser retVal = null;

        if (this.CurrentPrincipal.CurrentUser.IsSiteAdministrator == true)
        {
            retVal = Services.UserService.GetById(id);

            if (retVal == null)
            {
                retVal = new AnotherBlogUser();
            }

            retVal.IsSiteAdministrator = input.IsSiteAdministrator;
            retVal.ApprovedCommenter = input.ApprovedCommenter;
            retVal.About = AlwaysMoveForward.Common.Utilities.Utils.StripJavascript(input.About);
            retVal.FirstName = input.FirstName;
            retVal.LastName = input.LastName;
            retVal.DisplayName = input.DisplayName;

            if (input.Roles != null)
            {
                retVal.Roles = input.Roles;
            }

            retVal = Services.UserService.Save(retVal);
        }

        return retVal;
    }

    [HttpDelete("{id:int}")]
    [WebAPIAuthorizationAttribute(RoleType.Names.SiteAdministrator, true)]
    public void Delete(int id)
    {
        if (this.CurrentPrincipal.CurrentUser.IsSiteAdministrator == true)
        {
            this.Services.UserService.Delete(id);
        }
    }
}
