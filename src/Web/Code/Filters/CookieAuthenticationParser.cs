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

namespace PucksAndProgramming.AnotherBlog.Web.Code.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CookieAuthenticationParser : FilterAttribute, IAuthorizationFilter
    {
        public static SecurityPrincipal ParseCookie(HttpCookieCollection cookies)
        {
            // Get the authentication cookie
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = cookies[cookieName];
            SecurityPrincipal retVal = new SecurityPrincipal(null, false);

            ServiceManager serviceManager = ServiceManagerBuilder.BuildServiceManager();

            if (authCookie != null)
            {
                if (authCookie.Value != string.Empty)
                {
                    try
                    {
                        // Get the authentication ticket 
                        // and rebuild the principal & identity
                        FormsAuthenticationTicket authTicket =
                        FormsAuthentication.Decrypt(authCookie.Value);

                        AnotherBlogUser currentUser = serviceManager.UserService.GetById(int.Parse(authTicket.Name));

                        if (currentUser == null)
                        {
                            retVal = new SecurityPrincipal(UserFactory.CreateGuestUser(), false);
                        }
                        else
                        {
                            retVal = new SecurityPrincipal(currentUser, true);
                        }
                    }
                    catch (Exception e)
                    {
                        retVal = new SecurityPrincipal(UserFactory.CreateGuestUser(), false);
                    }
                }
            }
            else
            {
                retVal = new SecurityPrincipal(UserFactory.CreateGuestUser(), false);
            }

            System.Threading.Thread.CurrentPrincipal = retVal;
            HttpContext.Current.User = retVal;

            return retVal;
        }

        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            CookieAuthenticationParser.ParseCookie(filterContext.RequestContext.HttpContext.Request.Cookies);
        }
    }

}