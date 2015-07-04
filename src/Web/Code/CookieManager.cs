using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Security.Principal;
using System.Web.Security;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.Common.Encryption;
using AlwaysMoveForward.OAuth.Client;
using AlwaysMoveForward.OAuth.Client.Configuration;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;
using AlwaysMoveForward.AnotherBlog.Web.Models;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;

namespace AlwaysMoveForward.AnotherBlog.Web.Code
{
    public class CookieManager
    {
        public struct CookieNames
        {
            public static string UserCookie = "AMFUser";
            public static string ValidatedCookie = "Validated";
        }

        public static void EliminateCookie(HttpResponseBase response, string cookieName)
        {
            try
            {
                HttpCookie targetCookie = response.Cookies[cookieName];

                if (targetCookie != null)
                {
                    targetCookie.Expires = DateTime.Now.AddDays(-1);
                }
            }
            catch (Exception e)
            {
                LogManager.GetLogger().Error(e);
            }

        }
        public static void EstablishUserCookie(SecurityPrincipal currentPrincipal, HttpContextBase httpContext)
        {
            if (currentPrincipal != null && currentPrincipal.CurrentUser != null)
            {
                FormsAuthenticationTicket authTicket =
                    new FormsAuthenticationTicket(1, currentPrincipal.CurrentUser.Id.ToString(), DateTime.Now, DateTime.Now.AddYears(5), true, string.Empty);

                string encryptedValue = FormsAuthentication.Encrypt(authTicket);

                HttpCookie encryptedCookie = new HttpCookie(CookieManager.CookieNames.UserCookie, encryptedValue);
                httpContext.Response.Cookies.Add(encryptedCookie);
            }
        }

        public static string GetDecryptedCookie(HttpCookieCollection cookies, string cookieName)
        {
            string retVal = string.Empty;

            // Get the authentication cookie
            HttpCookie authCookie = cookies[cookieName];

            if (authCookie != null)
            {
                if (authCookie.Value != string.Empty)
                {
                    try
                    {
                        // Get the authentication ticket 
                        // and rebuild the principal & identity
                        FormsAuthenticationTicket authTicket =
                           FormsAuthentication.Decrypt(authCookie.Value);

                        retVal = authTicket.Name;
                    }
                    catch (Exception e)
                    {
                        LogManager.GetLogger().Error(e);
                    }
                }
            }

            return retVal;
        }

        public static void EstablishValidatedCookie(bool isValidated, HttpContext httpContext)
        {
            FormsAuthenticationTicket authTicket =
                new FormsAuthenticationTicket(1, isValidated.ToString(), DateTime.Now, DateTime.Now.AddDays(14), false, string.Empty);

            string encryptedValue = FormsAuthentication.Encrypt(authTicket);

            HttpCookie encryptedCookie = new HttpCookie(CookieManager.CookieNames.ValidatedCookie, encryptedValue);
            encryptedCookie.Expires = DateTime.Now.AddDays(14);
            httpContext.Response.Cookies.Add(encryptedCookie);
        }
    }
}