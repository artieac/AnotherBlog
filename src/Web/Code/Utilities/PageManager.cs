using Microsoft.AspNetCore.Http;
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;

namespace AlwaysMoveForward.AnotherBlog.Web.Code.Utilities
{
    public class PageManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PageManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsSiteAdministrator()
        {
            bool retVal = false;

            var currentPrincipal = _httpContextAccessor.HttpContext?.Items["CurrentPrincipal"] as SecurityPrincipal;

            if (currentPrincipal != null)
            {
                retVal = currentPrincipal.IsInRole(RoleType.Names.SiteAdministrator);
            }

            return retVal;
        }

        public bool CanAccessAdminTool()
        {
            bool retVal = false;

            var currentPrincipal = _httpContextAccessor.HttpContext?.Items["CurrentPrincipal"] as SecurityPrincipal;

            if (currentPrincipal != null)
            {
                if (currentPrincipal.IsInRole(RoleType.Names.SiteAdministrator) ||
                    currentPrincipal.IsInRole(RoleType.Names.Administrator))
                {
                    retVal = true;
                }
            }

            return retVal;
        }

        public string GetCurrentTheme(AlwaysMoveForward.AnotherBlog.Web.Models.CommonModel commonModel)
        {
            string retVal = "default";

            SiteInfo siteInfo = WebApplicationState.SiteInfo;

            if (siteInfo != null)
            {
                if (!string.IsNullOrEmpty(siteInfo.DefaultTheme))
                {
                    retVal = siteInfo.DefaultTheme;
                }
            }

            return retVal;
        }
    }
}
