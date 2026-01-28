using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.Web.Code.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class WebAPIAuthorizationAttribute : Attribute, IAuthorizationFilter
{
    public WebAPIAuthorizationAttribute()
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
            if (currentPrincipal != null)
            {
                if (string.IsNullOrEmpty(this.RequiredRoles))
                {
                    if (currentPrincipal.IsAuthenticated == true)
                    {
                        isAuthorized = true;
                    }
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

        if (!isAuthorized)
        {
            filterContext.Result = new UnauthorizedResult();
        }
    }
}
