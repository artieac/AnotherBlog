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

/// <summary>
/// Authorization filter attribute for admin actions using .NET's TypeFilter pattern.
/// </summary>
public class AdminAuthorizationFilterAttribute : TypeFilterAttribute
{
    public AdminAuthorizationFilterAttribute(string requiredRoles = "", bool isBlogSpecific = true)
        : base(typeof(AdminAuthorizationFilter))
    {
        Arguments = [requiredRoles, isBlogSpecific];
    }
}

/// <summary>
/// The actual authorization filter implementation with DI support.
/// </summary>
public class AdminAuthorizationFilter : IAsyncAuthorizationFilter
{
    private readonly ServiceManagerBuilder _serviceManagerBuilder;
    private readonly string _requiredRoles;
    private readonly bool _isBlogSpecific;

    public AdminAuthorizationFilter(
        ServiceManagerBuilder serviceManagerBuilder,
        string requiredRoles,
        bool isBlogSpecific)
    {
        _serviceManagerBuilder = serviceManagerBuilder;
        _requiredRoles = requiredRoles;
        _isBlogSpecific = isBlogSpecific;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        bool isAuthorized = false;

        var currentPrincipal = context.HttpContext.Items["CurrentPrincipal"] as SecurityPrincipal;

        try
        {
            if (currentPrincipal != null)
            {
                if (string.IsNullOrEmpty(_requiredRoles))
                {
                    isAuthorized = false;
                }
                else
                {
                    string[] roleList = _requiredRoles.Split(',');

                    if (!_isBlogSpecific)
                    {
                        foreach (var role in roleList)
                        {
                            if (currentPrincipal.IsInRole(role))
                            {
                                isAuthorized = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Blog targetBlog = await GetTargetBlogAsync(context);
                        isAuthorized = currentPrincipal.IsInRole(roleList, targetBlog);
                    }
                }
            }
        }
        catch (Exception e)
        {
            LogManager.GetLogger().Error(e);
        }

        if (!isAuthorized)
        {
            var request = context.HttpContext.Request;
            var scheme = request.Scheme;
            var host = request.Host.Value;
            context.Result = new RedirectResult($"{scheme}://{host}");
        }
    }

    private Task<Blog> GetTargetBlogAsync(AuthorizationFilterContext context)
    {
        Blog result = null;

        try
        {
            string blogSubFolder = string.Empty;
            var request = context.HttpContext.Request;

            if (request.RouteValues.TryGetValue("blogSubFolder", out var routeValue))
            {
                blogSubFolder = routeValue?.ToString() ?? string.Empty;
            }
            else if (request.RouteValues.TryGetValue("id", out var idValue))
            {
                blogSubFolder = idValue?.ToString() ?? string.Empty;
            }
            else if (request.HasFormContentType && request.Form.ContainsKey("blogSubFolder"))
            {
                blogSubFolder = request.Form["blogSubFolder"].ToString();
            }
            else if (request.Query.ContainsKey("blogSubFolder"))
            {
                blogSubFolder = request.Query["blogSubFolder"].ToString();
            }

            if (!string.IsNullOrEmpty(blogSubFolder))
            {
                var serviceManager = _serviceManagerBuilder.CreateServiceManager();
                result = serviceManager.BlogService.GetBySubFolder(blogSubFolder);
            }
        }
        catch (Exception e)
        {
            LogManager.GetLogger().Error(e);
        }

        return Task.FromResult(result);
    }
}
