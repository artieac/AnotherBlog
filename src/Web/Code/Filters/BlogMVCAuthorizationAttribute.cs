using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.Web.Code.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class BlogMVCAuthorizationAttribute : Attribute, IAuthorizationFilter
{
    public BlogMVCAuthorizationAttribute()
    {
        this.RequiredRoles = string.Empty;
    }

    public string RequiredRoles { get; set; }

    protected Blog GetTargetBlog(AuthorizationFilterContext filterContext)
    {
        Blog retVal = null;

        try
        {
            var request = filterContext.HttpContext.Request;
            var pathSegments = request.Path.Value?.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (pathSegments != null && pathSegments.Length >= 2)
            {
                ServiceManager serviceManager = ServiceManagerBuilder.BuildServiceManager();
                retVal = serviceManager.BlogService.GetByName(pathSegments[1]);
            }
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
            if (this.RequiredRoles != null)
            {
                if (this.RequiredRoles == string.Empty)
                {
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

                    if (currentPrincipal != null)
                    {
                        string[] roleList = this.RequiredRoles.Split(',');
                        isAuthorized = currentPrincipal.IsInRole(roleList, targetBlog);
                    }
                }
            }
            else
            {
                if (currentPrincipal != null)
                {
                    if (currentPrincipal.IsAuthenticated == true)
                    {
                        isAuthorized = true;
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
