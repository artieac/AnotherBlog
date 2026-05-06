using Microsoft.AspNetCore.Mvc;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers.API;

[Route("api/[controller]")]
public class SiteInfoController : BaseApiController
{
    public SiteInfoController(ServiceManagerBuilder serviceManagerBuilder)
        : base(serviceManagerBuilder)
    {
    }

    [HttpGet]
    public SiteInfo Get()
    {
        return this.Services.SiteInfoService.GetSiteInfo();
    }

    [HttpPost]
    [WebAPIAuthorizationAttribute(RoleType.Names.SiteAdministrator, true)]
    public SiteInfo Post([FromBody] SiteInfo input)
    {
        SiteInfo retVal = null;

        if (this.CurrentPrincipal.CurrentUser.IsSiteAdministrator == true)
        {
            using (this.Services.UnitOfWork.BeginTransaction())
            {
                try
                {
                    retVal = Services.SiteInfoService.Save(input.Name, input.About, input.ContactEmail, input.DefaultTheme, input.SiteAnalyticsId, input.DefaultAuthor, input.DefaultKeywords);
                    WebApplicationState.SiteInfo = retVal;
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
}
