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

    public CookieAuthenticationFilter(IDataProtectionProvider dataProtectionProvider)
    {
        _dataProtectionProvider = dataProtectionProvider;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var principal = ParseCookie(context.HttpContext);
        context.HttpContext.Items["CurrentPrincipal"] = principal;
    }

    public SecurityPrincipal ParseCookie(HttpContext httpContext)
    {
        SecurityPrincipal retVal = new SecurityPrincipal(null, false);

        try
        {
            ServiceManager serviceManager = ServiceManagerBuilder.BuildServiceManager();
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
                            retVal = new SecurityPrincipal(currentUser, true);
                        }
                        else
                        {
                            retVal = new SecurityPrincipal(UserFactory.CreateGuestUser(), false);
                        }
                    }
                }
                catch
                {
                    retVal = new SecurityPrincipal(UserFactory.CreateGuestUser(), false);
                }
            }
            else
            {
                retVal = new SecurityPrincipal(UserFactory.CreateGuestUser(), false);
            }
        }
        catch
        {
            retVal = new SecurityPrincipal(UserFactory.CreateGuestUser(), false);
        }

        return retVal;
    }
}
