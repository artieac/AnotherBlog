using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.Web.Code.Filters;

/// <summary>
/// Authorization filter attribute for blog MVC actions using .NET's TypeFilter pattern.
/// </summary>
public class BlogMVCAuthorizationAttribute : TypeFilterAttribute
{
    public BlogMVCAuthorizationAttribute(string requiredRoles = "")
        : base(typeof(BlogMVCAuthorizationFilter))
    {
        Arguments = [requiredRoles];
    }
}

/// <summary>
/// The actual authorization filter implementation with DI support.
/// </summary>
public class BlogMVCAuthorizationFilter : IAsyncAuthorizationFilter
{
    private readonly ServiceManagerBuilder _serviceManagerBuilder;
    private readonly string _requiredRoles;

    public BlogMVCAuthorizationFilter(
        ServiceManagerBuilder serviceManagerBuilder,
        string requiredRoles)
    {
        _serviceManagerBuilder = serviceManagerBuilder;
        _requiredRoles = requiredRoles;
    }

    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        bool isAuthorized = false;

        var currentPrincipal = context.HttpContext.Items["CurrentPrincipal"] as SecurityPrincipal;

        try
        {
            if (!string.IsNullOrEmpty(_requiredRoles))
            {
                Blog targetBlog = GetTargetBlog(context);

                if (currentPrincipal != null)
                {
                    string[] roleList = _requiredRoles.Split(',');
                    isAuthorized = currentPrincipal.IsInRole(roleList, targetBlog);
                }
            }
            else
            {
                if (currentPrincipal != null && currentPrincipal.IsAuthenticated)
                {
                    isAuthorized = true;
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
