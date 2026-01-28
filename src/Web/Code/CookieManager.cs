using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;
using AlwaysMoveForward.Common.Utilities;
using Azure;
using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;
using static AlwaysMoveForward.AnotherBlog.Web.Code.CookieManager;

namespace AlwaysMoveForward.AnotherBlog.Web.Code
{
    public class CookieManager
    {
        public struct CookieNames
        {
            public static string UserCookie = "AMFUser";
            public static string ValidatedCookie = "Validated";
        }

        public static CookieOptions CreateDefaultCookieOptions()
        {
            return new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(14),
//                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

        }
        public static void EliminateCookie(HttpRequest request, HttpResponse response, string cookieName)
        {
            try
            {
                if (request.Cookies.ContainsKey(cookieName))
                {
                    CookieOptions expiredCookie = CookieManager.CreateDefaultCookieOptions();
                    expiredCookie.Expires = DateTime.Now.AddDays(-1);
                    response.Cookies.Append(cookieName, request.Cookies[cookieName], expiredCookie);
                }
            }
            catch (Exception e)
            {
                LogManager.GetLogger().Error(e);
            }

        }
    }
}