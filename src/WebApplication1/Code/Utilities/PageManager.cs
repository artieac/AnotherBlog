using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.Web.Code.Utilities
{
    public class PageManager
    {
        public static bool IsSiteAdministrator()
        {
            bool retVal = false;

            SecurityPrincipal currentPrincipal = HttpContext.Current.User as SecurityPrincipal;

            if (currentPrincipal != null)
            {
                retVal = currentPrincipal.IsInRole(RoleType.Names.SiteAdministrator);
            }

            return retVal;
        }

        public static bool CanAccessAdminTool()
        {
            bool retVal = false;

            SecurityPrincipal currentPrincipal = HttpContext.Current.User as SecurityPrincipal;

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

        public static string GetCurrentTheme(AlwaysMoveForward.AnotherBlog.Web.Models.CommonModel commonModel)
        {
            string retVal = "default";

            SiteInfo siteInfo = MvcApplication.SiteInfo;

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