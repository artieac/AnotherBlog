using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using PucksAndProgramming.Common.DataLayer;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Common.Factories;
using PucksAndProgramming.AnotherBlog.BusinessLayer.Service;
using PucksAndProgramming.AnotherBlog.BusinessLayer.Utilities;
using PucksAndProgramming.Common.Utilities;
using System.Security.Claims;

namespace PucksAndProgramming.AnotherBlog.Web.Code.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CookieAuthenticationParser : FilterAttribute, System.Web.Mvc.IAuthorizationFilter
    {
        public static HttpCookie GetFormsAuthenticationCookie(HttpCookieCollection cookies)
        {
            return GetCookie(cookies, FormsAuthentication.FormsCookieName);
        }
        public static HttpCookie GetRemoteOAuthCookie(HttpCookieCollection cookies)
        {
            return GetCookie(cookies, SecurityPrincipal.OAuthCookieName);
        }

        public static HttpCookie GetCookie(HttpCookieCollection cookies, string cookieName)
        {
            // Get the authentication cookie0
            HttpCookie retVal = cookies[cookieName];

            if (retVal != null)
            {
                if (retVal.Value == string.Empty)
                {
                    retVal = null;
                }
            }

            return retVal;
        }
        public static SecurityPrincipal ParseCookie(HttpCookieCollection cookies)
        {
            SecurityPrincipal retVal = null;
            
            ClaimsIdentity claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;

            if(claimsIdentity != null)
            {
                ServiceManager serviceManager = ServiceManagerBuilder.BuildServiceManager();
                AnotherBlogUser anotherBlogUser = null;

                string anotherBlogId = claimsIdentity?.FindFirst(c => c.Type == SecurityPrincipal.ClaimNames.AnotherBlogUserId)?.Value;

                if(!String.IsNullOrEmpty(anotherBlogId))
                {
                    anotherBlogUser = serviceManager.UserService.GetById(long.Parse(anotherBlogId));
                }
                else
                {
                    string remoteId = SecurityPrincipal.GetRemoteUserId(claimsIdentity);
                    anotherBlogUser = serviceManager.UserService.GetByOAuthServiceUserId(remoteId);
                }

                if(anotherBlogUser != null)
                {
                    retVal = new SecurityPrincipal(anotherBlogUser, claimsIdentity);
                    System.Threading.Thread.CurrentPrincipal = retVal;
                    HttpContext.Current.User = retVal;
                }

            }
 
            return retVal;
        }
        
        public virtual void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            CookieAuthenticationParser.ParseCookie(filterContext.RequestContext.HttpContext.Request.Cookies);
        }
    }

}