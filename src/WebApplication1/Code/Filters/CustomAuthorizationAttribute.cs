/**
 * Copyright (c) 2009 Arthur Correa.
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
    public class CustomAuthorizationAttribute : RequestAuthenticationFilter
    {
        public CustomAuthorizationAttribute()
            : base()
        {
            this.RequiredRoles = string.Empty;
        }

        public string RequiredRoles { get; set; }

        protected Blog GetTargetBlog(AuthorizationContext filterContext)
        {
            Blog retVal = null;

            try
            {
                string[] urlSegments = filterContext.HttpContext.Request.Url.Segments;

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

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            bool isAuthorized = false;

            try
            {
                if (System.Threading.Thread.CurrentPrincipal != null)
                {
                    SecurityPrincipal currentPrincipal = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;

                    if (this.RequiredRoles != null)
                    {
                        if (this.RequiredRoles == string.Empty)
                        {
                            // no required roles allow everyone.  But since this is being flagged at all
                            // we want to be sure that the useris at least logged in
                            if (currentPrincipal != null)
                            {
                                if (currentPrincipal.IsAuthenticated == true)
                                {
                                    isAuthorized = true;
                                }
                            }
                        }
                        else
                        {
                            Blog targetBlog = this.GetTargetBlog(filterContext);

                            // If no currentUser then they can't have the desired roles
                            if (currentPrincipal != null)
                            {
                                string[] roleList = this.RequiredRoles.Split(',');
                                isAuthorized = currentPrincipal.IsInRole(roleList, targetBlog);
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
                                isAuthorized = true;
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
