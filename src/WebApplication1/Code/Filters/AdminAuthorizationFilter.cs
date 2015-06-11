 /* Copyright (c) 2009 Arthur Correa.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Common Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/cpl1.0.php
 *
 * Contributors:
 *    Arthur Correa – initial contribution
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;

namespace AlwaysMoveForward.AnotherBlog.Web.Code.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AdminAuthorizationFilter : FilterAttribute, IAuthorizationFilter
    {
        public AdminAuthorizationFilter()
            : base()
        {
            this.RequiredRoles = string.Empty;
            this.IsBlogSpecific = true;
        }

        public string RequiredRoles { get; set; }
        public bool IsBlogSpecific { get; set; }

        protected Blog GetTargetBlog(AuthorizationContext filterContext)
        {
            Blog retVal = null;

            try
            {
                string blogSubFolder = string.Empty;

                if (filterContext.RequestContext.HttpContext.Request.Form["blogSubFolder"] != null)
                {
                    blogSubFolder = filterContext.RequestContext.HttpContext.Request.Form["blogSubFolder"];
                }
                else if (filterContext.RequestContext.HttpContext.Request.QueryString["blogSubFolder"] != null)
                {
                    blogSubFolder = filterContext.RequestContext.HttpContext.Request.QueryString["blogSubFolder"];
                }

                ServiceManager serviceManager = ServiceManagerBuilder.BuildServiceManager();
                retVal = serviceManager.BlogService.GetByName(blogSubFolder);
            }
            catch (Exception e)
            {
                LogManager.GetLogger().Error(e);
            }

            return retVal;
        }

        #region IAuthorizationFilter Members

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            bool isAuthorized = false;

            try
            {
                if (System.Threading.Thread.CurrentPrincipal != null)
                {
                    SecurityPrincipal currentPrincipal = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;

                    if (string.IsNullOrEmpty(this.RequiredRoles))
                    {
                        // Admin section needs at least one role specified.
                        isAuthorized = false;
                    }
                    else
                    {
                        string[] roleList = this.RequiredRoles.Split(',');

                        if (this.IsBlogSpecific == false)
                        {
                            for (int i = 0; i < roleList.Count(); i++)
                            {
                                if (currentPrincipal.IsInRole(roleList[i]))
                                {
                                    isAuthorized = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Blog targetBlog = this.GetTargetBlog(filterContext);

                            // If no currentUser then they can't have the desired roles
                            if (currentPrincipal != null)
                            {
                                isAuthorized = currentPrincipal.IsInRole(roleList, targetBlog);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogManager.GetLogger().Error(e);
            }

            if (isAuthorized == false)
            {
                // not allowed to proceed
                filterContext.Result = new RedirectResult("http://" + HttpContext.Current.Request.Url.Authority);
            }
        }

        #endregion    

    }
}