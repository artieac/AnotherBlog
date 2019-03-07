using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PucksAndProgramming.Common.Utilities;
using PucksAndProgramming.Common.DataLayer;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.BusinessLayer.Utilities;
using PucksAndProgramming.AnotherBlog.BusinessLayer.Service;

namespace PucksAndProgramming.AnotherBlog.Web.Code.Filters
{
    public class WebAPIAuthorizationAttribute: System.Web.Http.AuthorizeAttribute
    {
        public WebAPIAuthorizationAttribute()
            : base()
        {
            this.RequiredRoles = string.Empty; 
            this.IsBlogSpecific = true;
        }

        public string RequiredRoles { get; set; }
        public bool IsBlogSpecific { get; set; }

        protected Blog GetTargetBlog(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            Blog retVal = null;

            try
            {
                string[] urlSegments = actionContext.Request.RequestUri.Segments;

                if (urlSegments.Length >= 2)
                {
                    ServiceManager serviceManager = ServiceManagerBuilder.BuildServiceManager();
                    retVal = serviceManager.BlogService.GetByName(urlSegments[1].Substring(0, urlSegments[1].Length - 1));
                }
            }
            catch (Exception e)
            {
                LogManager.GetLogger().Error(e);
            }

            return retVal;
        }

        #region IAuthorizationFilter Members

        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            bool retVal = false;

            SecurityPrincipal currentPrincipal = CookieAuthenticationParser.ParseCookie(HttpContext.Current.Request.Cookies);

            try
            {
                if (currentPrincipal != null)
                {
                    if (string.IsNullOrEmpty(this.RequiredRoles))
                    {
                        // no required roles allow everyone.  But since this is being flagged at all
                        // we want to be sure that the useris at least logged in
                        if (currentPrincipal != null)
                        {
                            if (currentPrincipal.IsAuthenticated == true)
                            {
                                retVal = true;
                            }
                        }
                    }
                    else
                    {
                        // If no currentUser then they can't have the desired roles
                        if (currentPrincipal != null)
                        {
                            string[] roleList = this.RequiredRoles.Split(',');

                            if (this.IsBlogSpecific == false)
                            {
                                for (int i = 0; i < roleList.Count(); i++)
                                {
                                    if (currentPrincipal.IsInRole(roleList[i]))
                                    {
                                        retVal = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                Blog targetBlog = this.GetTargetBlog(actionContext);

                                // If no currentUser then they can't have the desired roles
                                if (currentPrincipal != null)
                                {
                                    retVal = currentPrincipal.IsInRole(roleList, targetBlog);
                                }
                            }
                        }
                        else
                        {
                            // no required roles allow everyone.  But since this is being flagged at all
                            // we want to be sure that the useris at least logged in
                            if (currentPrincipal != null)
                            {
                                if (currentPrincipal.IsAuthenticated == true)
                                {
                                    retVal = true;
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception e)
            {
                LogManager.GetLogger().Error(e);
            }

            return retVal;
        }

        #endregion
    }
}