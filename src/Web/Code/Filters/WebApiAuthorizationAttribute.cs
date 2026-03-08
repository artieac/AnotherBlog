using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.Web.Code.Filters;

/// <summary>
/// Authorization filter attribute for Web API actions using .NET's TypeFilter pattern.
/// </summary>
public class WebAPIAuthorizationAttribute : TypeFilterAttribute
{
    public WebAPIAuthorizationAttribute(string requiredRoles = "", bool isBlogSpecific = true)
        : base(typeof(WebAPIAuthorizationFilter))
    {
        Arguments = [requiredRoles, isBlogSpecific];
    }
}

/// <summary>
/// The actual authorization filter implementation with DI support.
/// </summary>
public class WebAPIAuthorizationFilter : IAsyncAuthorizationFilter
{
    private readonly ServiceManagerBuilder _serviceManagerBuilder;
    private readonly string _requiredRoles;
    private readonly bool _isBlogSpecific;

    public WebAPIAuthorizationFilter(
        ServiceManagerBuilder serviceManagerBuilder,
        string requiredRoles,
        bool isBlogSpecific)
    {
        _serviceManagerBuilder = serviceManagerBuilder;
        _requiredRoles = requiredRoles;
        _isBlogSpecific = isBlogSpecific;
    }

    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        bool isAuthorized = false;

        var currentPrincipal = context.HttpContext.Items["CurrentPrincipal"] as SecurityPrincipal;

        try
        {
            if (currentPrincipal != null)
            {
                if (string.IsNullOrEmpty(_requiredRoles))
                {
                    if (currentPrincipal.IsAuthenticated)
                    {
                        isAuthorized = true;
                    }
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
                        Blog targetBlog = GetTargetBlog(context);
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
            context.Result = new UnauthorizedResult();
        }

        return Task.CompletedTask;
    }

    private Blog GetTargetBlog(AuthorizationFilterContext context)
    {
        Blog result = null;

        try
        {
            var request = context.HttpContext.Request;
            var pathSegments = request.Path.Value?.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (pathSegments != null && pathSegments.Length >= 2)
            {
                var serviceManager = _serviceManagerBuilder.CreateServiceManager();
                result = serviceManager.BlogService.GetBySubFolder(pathSegments[1]);
            }
        }
        catch (Exception e)
        {
            LogManager.GetLogger().Error(e);
        }

        return result;
    }
}
