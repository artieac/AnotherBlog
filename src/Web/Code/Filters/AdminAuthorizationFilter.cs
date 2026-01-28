/* Copyright (c) 2009 Arthur Correa.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Common Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/cpl1.0.php
 *
 * Contributors:
 *    Arthur Correa – initial contribution
 */
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;

namespace AlwaysMoveForward.AnotherBlog.Web.Code.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class AdminAuthorizationFilter : Attribute, IAuthorizationFilter
{
    public AdminAuthorizationFilter()
    {
        this.RequiredRoles = string.Empty;
        this.IsBlogSpecific = true;
    }

    public string RequiredRoles { get; set; }
    public bool IsBlogSpecific { get; set; }

    protected Blog GetTargetBlog(AuthorizationFilterContext filterContext)
    {
        Blog retVal = null;

        try
        {
            string blogSubFolder = string.Empty;
            var request = filterContext.HttpContext.Request;

            if (request.HasFormContentType && request.Form.ContainsKey("blogSubFolder"))
            {
                blogSubFolder = request.Form["blogSubFolder"];
            }
            else if (request.Query.ContainsKey("blogSubFolder"))
            {
                blogSubFolder = request.Query["blogSubFolder"];
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

    public void OnAuthorization(AuthorizationFilterContext filterContext)
    {
        bool isAuthorized = false;

        SecurityPrincipal currentPrincipal = filterContext.HttpContext.Items["CurrentPrincipal"] as SecurityPrincipal;

        try
        {
            if (currentPrincipal != null)
            {
                if (string.IsNullOrEmpty(this.RequiredRoles))
                {
                    isAuthorized = false;
                }
                else
                {
                    string[] roleList = this.RequiredRoles.Split(',');

                    if (this.IsBlogSpecific == false)
                    {
                        for (int i = 0; i < roleList.Length; i++)
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
                        isAuthorized = currentPrincipal.IsInRole(roleList, targetBlog);
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
            var request = filterContext.HttpContext.Request;
            var scheme = request.Scheme;
            var host = request.Host.Value;
            filterContext.Result = new RedirectResult($"{scheme}://{host}");
        }
    }
}
