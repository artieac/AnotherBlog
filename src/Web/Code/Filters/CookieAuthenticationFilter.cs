using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.DataProtection;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.Factories;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;

namespace AlwaysMoveForward.AnotherBlog.Web.Code.Filters;

public class CookieAuthenticationFilter : IAuthorizationFilter
{
    private const string AuthCookieName = ".ASPXAUTH";
    private readonly IDataProtectionProvider _dataProtectionProvider;
    private readonly ServiceManagerBuilder _serviceManagerBuilder;

    public CookieAuthenticationFilter(
        IDataProtectionProvider dataProtectionProvider,
        ServiceManagerBuilder serviceManagerBuilder)
    {
        _dataProtectionProvider = dataProtectionProvider;
        _serviceManagerBuilder = serviceManagerBuilder;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var principal = ParseCookie(context.HttpContext);
        context.HttpContext.Items["CurrentPrincipal"] = principal;
    }

    public SecurityPrincipal ParseCookie(HttpContext httpContext)
    {
        ServiceManager serviceManager = _serviceManagerBuilder.CreateServiceManager();
        SecurityPrincipal retVal = new SecurityPrincipal(serviceManager, UserFactory.CreateGuestUser(), false);

        try
        {
            var authCookie = httpContext.Request.Cookies[AuthCookieName];

            if (!string.IsNullOrEmpty(authCookie))
            {
                try
                {
                    var protector = _dataProtectionProvider.CreateProtector("AuthCookie");
                    var decryptedValue = protector.Unprotect(authCookie);

                    if (int.TryParse(decryptedValue, out int userId))
                    {
                        AnotherBlogUser currentUser = serviceManager.UserService.GetById(userId);

                        if (currentUser != null)
                        {
                            retVal = new SecurityPrincipal(serviceManager, currentUser, true);
                        }
                        else
                        {
                            retVal = new SecurityPrincipal(serviceManager, UserFactory.CreateGuestUser(), false);
                        }
                    }
                }
                catch
                {
                    retVal = new SecurityPrincipal(serviceManager, UserFactory.CreateGuestUser(), false);
                }
            }
        }
        catch
        {
            // Keep the default guest principal
        }

        return retVal;
    }
}
