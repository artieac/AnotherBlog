/**
 * Copyright (c) 2009 Arthur Correa.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Common Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/cpl1.0.php
 *
 * Contributors:
 *    Arthur Correa – initial contribution
 */
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.Factories;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;
using AlwaysMoveForward.AnotherBlog.Web.Models;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;
using AlwaysMoveForward.AnotherBlog.Web.Configuration;
using AlwaysMoveForward.AnotherBlog.Web.Code;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers;

public class UserController : PublicController
{
    private const string Auth0StateSessionKey = "Auth0State";
    private readonly IDataProtectionProvider _dataProtectionProvider;
    private readonly Auth0Settings _auth0Settings;
    private readonly IHttpClientFactory _httpClientFactory;

    public UserController(
        ServiceManagerBuilder serviceManagerBuilder,
        IDataProtectionProvider dataProtectionProvider,
        IOptions<Auth0Settings> auth0Settings,
        IHttpClientFactory httpClientFactory)
        : base(serviceManagerBuilder)
    {
        _dataProtectionProvider = dataProtectionProvider;
        _auth0Settings = auth0Settings.Value;
        _httpClientFactory = httpClientFactory;
    }

    public UserModel InitializeUserModel()
    {
        UserModel retVal = new UserModel();
        retVal.Common = this.InitializeCommonModel();
        retVal.Common.Calendar = this.InitializeCalendarModel(retVal.Common.TargetMonth);
        return retVal;
    }

    public UserModel InitializeUserModel(string blogSubFolder)
    {
        UserModel retVal = new UserModel();
        retVal.Common = this.InitializeCommonModel(this.Services.BlogService.GetBySubFolder(blogSubFolder));
        retVal.Common.Calendar = this.InitializeCalendarModel(retVal.Common.TargetMonth);
        return retVal;
    }

    private void EstablishCurrentUserCookie(SecurityPrincipal currentPrincipal)
    {
        if (currentPrincipal != null && currentPrincipal.CurrentUser != null)
        {
            var protector = _dataProtectionProvider.CreateProtector("AuthCookie");
            var encryptedValue = protector.Protect(currentPrincipal.CurrentUser.Id.ToString());

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTimeOffset.Now.AddYears(1)
            };

            Response.Cookies.Append(CookieManager.CookieNames.UserCookie, encryptedValue, cookieOptions);

            this.CurrentPrincipal = currentPrincipal;
        }
    }

    private void EliminateUserCookie()
    {
        try
        {
            Response.Cookies.Delete(CookieManager.CookieNames.UserCookie);
        }
        catch (Exception e)
        {
            LogManager.GetLogger().Error(e);
        }
    }

    [HttpPost]
    [Route("User/Login")]
    public IActionResult Login()
    {
        // Generate a random state for CSRF protection
        var state = GenerateRandomState();
        HttpContext.Session.SetString(Auth0StateSessionKey, state);

        var redirectUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{_auth0Settings.CallbackPath}";

        var authorizeUrl = $"https://{_auth0Settings.Domain}/authorize" +
            $"?response_type=code" +
            $"&client_id={Uri.EscapeDataString(_auth0Settings.ClientId)}" +
            $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
            $"&scope={Uri.EscapeDataString("openid profile email")}" +
            $"&state={Uri.EscapeDataString(state)}";

        return Redirect(authorizeUrl);
    }

    [HttpGet]
    [Route("User/Callback")]
    public async Task<IActionResult> Callback(string code, string state, string error, string error_description)
    {
        // Check for errors from Auth0
        if (!string.IsNullOrEmpty(error))
        {
            LogManager.GetLogger().Error($"Auth0 error: {error} - {error_description}");
            return RedirectToAction("Index", "Home");
        }

        // Verify state to prevent CSRF
        var savedState = HttpContext.Session.GetString(Auth0StateSessionKey);
        if (string.IsNullOrEmpty(savedState) || savedState != state)
        {
            LogManager.GetLogger().Error("Auth0 callback: Invalid state parameter");
            return RedirectToAction("Index", "Home");
        }

        // Clear the state from session
        HttpContext.Session.Remove(Auth0StateSessionKey);

        if (string.IsNullOrEmpty(code))
        {
            LogManager.GetLogger().Error("Auth0 callback: No authorization code received");
            return RedirectToAction("Index", "Home");
        }

        try
        {
            // Exchange authorization code for tokens
            var tokenResponse = await ExchangeCodeForTokens(code);
            if (tokenResponse == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Get user info from Auth0
            var userInfo = await GetUserInfo(tokenResponse.AccessToken);
            if (userInfo == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Find or create local user
            AnotherBlogUser currentUser = null;

            if (!string.IsNullOrEmpty(userInfo.Email))
            {
                currentUser = Services.UserService.GetByEmail(userInfo.Email);
            }

            if (currentUser == null && !string.IsNullOrEmpty(userInfo.Sub))
            {
                currentUser = Services.UserService.GetByExternalId(userInfo.Sub);
            }

            if (currentUser == null)
            {
                currentUser = Services.UserService.CreateFromAuth0(userInfo.Email, userInfo.Name, userInfo.Sub);
            }
            else if (!string.IsNullOrEmpty(userInfo.Sub) && currentUser.OAuthServiceUserId != userInfo.Sub)
            {
                currentUser = Services.UserService.UpdateExternalId(currentUser.Id, userInfo.Sub);
            }

            if (currentUser != null)
            {
                var principal = new SecurityPrincipal(Services, currentUser, true);
                EstablishCurrentUserCookie(principal);
                HttpContext.User = principal;
            }

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            LogManager.GetLogger().Error(ex);
            return RedirectToAction("Index", "Home");
        }
    }

    private async Task<Auth0TokenResponse> ExchangeCodeForTokens(string code)
    {
        var httpClient = _httpClientFactory.CreateClient();

        var redirectUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{_auth0Settings.CallbackPath}";

        var tokenRequest = new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = _auth0Settings.ClientId,
            ["client_secret"] = _auth0Settings.ClientSecret,
            ["code"] = code,
            ["redirect_uri"] = redirectUri
        };

        var response = await httpClient.PostAsync(
            $"https://{_auth0Settings.Domain}/oauth/token",
            new FormUrlEncodedContent(tokenRequest));

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            LogManager.GetLogger().Error($"Auth0 token exchange failed: {errorContent}");
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Auth0TokenResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    private async Task<Auth0UserInfo> GetUserInfo(string accessToken)
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var response = await httpClient.GetAsync($"https://{_auth0Settings.Domain}/userinfo");

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            LogManager.GetLogger().Error($"Auth0 userinfo failed: {errorContent}");
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Auth0UserInfo>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    private static string GenerateRandomState()
    {
        var bytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }
        return Convert.ToBase64String(bytes);
    }

    [HttpPost]
    [Route("User/Logout")]
    public IActionResult Logout()
    {
        EliminateUserCookie();
        this.CurrentPrincipal = new SecurityPrincipal(Services, UserFactory.CreateGuestUser());
        HttpContext.User = this.CurrentPrincipal;

        return this.RedirectToAction("Index", "Home");
    }

    [Route("User/Preferences")]
    [HttpGet]
    [BlogMVCAuthorizationAttribute]
    public IActionResult Preferences(string blogSubFolder)
    {
        UserModel model = this.InitializeUserModel(blogSubFolder);
        model.Common.ContentTitle = "My Account";
        model.CurrentUser = this.CurrentPrincipal.CurrentUser;
        return this.View(model);
    }

    [Route("User/SavePreferences")]
    [BlogMVCAuthorizationAttribute]
    public IActionResult SavePreferences(string blogSubFolder, string userAbout)
    {
        UserModel model = this.InitializeUserModel(blogSubFolder);
        model.Common.ContentTitle = "My Account";

        AnotherBlogUser userToSave = this.CurrentPrincipal.CurrentUser;

        model.CurrentUser = Services.UserService.Save(userToSave.Id, userToSave.IsSiteAdministrator, userToSave.ApprovedCommenter, userAbout);

        return this.View("Preferences", model);
    }

    [Route("User/BlogNavMenu")]
    [HttpGet]
    public IActionResult BlogNavMenu()
    {
        UserModel model = this.InitializeUserModel();
        return this.View(model);
    }

    [Route("User/ViewUserSocial")]
    [HttpGet]
    [BlogMVCAuthorizationAttribute(RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator)]
    public IActionResult ViewUserSocial(string blogSubFolder, string userId)
    {
        UserModel model = this.InitializeUserModel(blogSubFolder);

        int targetUser = int.Parse(userId);
        model.CurrentUser = Services.UserService.GetById(targetUser);

        return this.View(model);
    }

    [Route("User/OAuthCallback")]
    [HttpGet]
    public IActionResult OAuthCallback(string oauth_token, string oauth_verifier)
    {
        return this.RedirectToAction("Index", "Home");
    }

    // Helper classes for Auth0 responses
    private class Auth0TokenResponse
    {
        public string Access_Token { get; set; }
        public string AccessToken => Access_Token;
        public string Id_Token { get; set; }
        public string Token_Type { get; set; }
        public int Expires_In { get; set; }
    }

    private class Auth0UserInfo
    {
        public string Sub { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
    }
}
