using Microsoft.AspNetCore.Mvc;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers.API;

[Route("api/[controller]")]
public class UsersController : BaseApiController
{
    public UsersController(ServiceManagerBuilder serviceManagerBuilder)
        : base(serviceManagerBuilder)
    {
    }

    [HttpGet]
    [WebAPIAuthorizationAttribute(RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator, true)]
    public IEnumerable<AnotherBlogUser> Get()
    {
        return this.Services.UserService.GetAll();
    }

    [HttpGet("{id:int}")]
    [WebAPIAuthorizationAttribute(RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator, true)]
    public AnotherBlogUser GetById(int id)
    {
        return this.Services.UserService.GetById(id);
    }

    [HttpPost("{id:int}")]
    [WebAPIAuthorizationAttribute(RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator, true)]
    public AnotherBlogUser Post(int id, [FromBody] AnotherBlogUser input)
    {
        AnotherBlogUser retVal = null;

        if (this.CurrentPrincipal.CurrentUser.IsSiteAdministrator == true)
        {
            using (this.Services.UnitOfWork.BeginTransaction())
            {
                try
                {
                    retVal = Services.UserService.Save(id, input.IsSiteAdministrator, input.ApprovedCommenter, input.About);
                    this.Services.UnitOfWork.EndTransaction(true);
                }
                catch (Exception e)
                {
                    this.Services.UnitOfWork.EndTransaction(false);
                }
            }
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
