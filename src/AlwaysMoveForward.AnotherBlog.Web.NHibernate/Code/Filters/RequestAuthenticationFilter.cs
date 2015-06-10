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
using System.Web.Routing;
using System.Web.Security;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;

namespace AlwaysMoveForward.AnotherBlog.Web.Code.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class RequestAuthenticationFilter : FilterAttribute, IAuthorizationFilter
    {
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            // Get the authentication cookie
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = filterContext.RequestContext.HttpContext.Request.Cookies[cookieName];
            SecurityPrincipal currentPrincipal = new SecurityPrincipal(null, false);

            ServiceManager serviceManager = ServiceManagerBuilder.BuildServiceManager();

            if (authCookie != null)
            {
                if (authCookie.Value != string.Empty)
                {
                    // Get the authentication ticket 
                    // and rebuild the principal & identity
                    FormsAuthenticationTicket authTicket =
                    FormsAuthentication.Decrypt(authCookie.Value);

                    AnotherBlogUser currentUser = serviceManager.UserService.GetById(int.Parse(authTicket.Name));

                    if (currentUser == null)
                    {
                        currentPrincipal = new SecurityPrincipal(serviceManager.UserService.GetDefaultUser(), false);
                    }
                    else
                    {

                        currentPrincipal = new SecurityPrincipal(currentUser, true);
                    }
                }
            }
            else
            {
                currentPrincipal = new SecurityPrincipal(serviceManager.UserService.GetDefaultUser(), false);
            }

            System.Threading.Thread.CurrentPrincipal = filterContext.RequestContext.HttpContext.User = currentPrincipal;
        }
    }
}