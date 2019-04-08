using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using PucksAndProgramming.OAuth.Client.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using System.Security.Claims;
using PucksAndProgramming.AnotherBlog.BusinessLayer.Utilities;

namespace PucksAndProgramming.AnotherBlog.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure Auth0 parameters
            EndpointConfiguration oauthEndpoints = EndpointConfiguration.GetInstance();
            OAuthKeyConfiguration keyConfiguration = OAuthKeyConfiguration.GetInstance();

            // Enable the Cookie saver middleware to work around a bug in the OWIN implementation
            app.UseKentorOwinCookieSaver();

            // Set Cookies as default authentication type
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                LoginPath = new PathString("/Account/Login"),
                CookieName = SecurityPrincipal.OAuthCookieName
            });

            // Configure Auth0 authentication
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = "Auth0",

                Authority = oauthEndpoints.ServiceUri,

                ClientId = keyConfiguration.ConsumerKey,
                ClientSecret = keyConfiguration.ConsumerSecret,

                RedirectUri = "http://localhost:57679/User/OAuthCallback",
                PostLogoutRedirectUri = "http://localhost:57679",

                ResponseType = OpenIdConnectResponseType.CodeIdTokenToken,
                Scope = "openid profile",

                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name"
                },

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = notification =>
                    {
                        notification.AuthenticationTicket.Identity.AddClaim(new Claim(SecurityPrincipal.ClaimNames.IdToken, notification.ProtocolMessage.IdToken));
                        notification.AuthenticationTicket.Identity.AddClaim(new Claim(SecurityPrincipal.ClaimNames.AccessToken, notification.ProtocolMessage.AccessToken));

                        return Task.FromResult(0);
                    },
                    RedirectToIdentityProvider = notification =>
                    {
                        if (notification.ProtocolMessage.RequestType == OpenIdConnectRequestType.Logout)
                        {
                            var logoutUri = oauthEndpoints.ServiceUri + "/v2/logout?client_id=" + keyConfiguration.ConsumerSecret;

                            if(notification.ProtocolMessage != null)
                            {
                                var postLogoutUri = notification.ProtocolMessage.PostLogoutRedirectUri;
                                if (!string.IsNullOrEmpty(postLogoutUri))
                                {
                                    if (postLogoutUri.StartsWith("/"))
                                    {
                                        // transform to absolute
                                        var request = notification.Request;
                                        postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                                    }
                                    logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
                                }
                            }

                            notification.Response.Redirect(logoutUri);
                            notification.HandleResponse();
                        }
                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}