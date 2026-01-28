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
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.OAuth.Client;
using AlwaysMoveForward.OAuth.Client.Configuration;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.Factories;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;
using AlwaysMoveForward.AnotherBlog.Web.Models;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers;

public class UserController : PublicController
{
    private const string AuthCookieName = ".ASPXAUTH";
    private readonly IDataProtectionProvider _dataProtectionProvider;

    public UserController(IDataProtectionProvider dataProtectionProvider)
    {
        _dataProtectionProvider = dataProtectionProvider;
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

            Response.Cookies.Append(AuthCookieName, encryptedValue, cookieOptions);

            this.CurrentPrincipal = currentPrincipal;
        }
    }

    private void EliminateUserCookie()
    {
        try
        {
            Response.Cookies.Delete(AuthCookieName);
        }
        catch (Exception e)
        {
            LogManager.GetLogger().Error(e);
        }
    }

    private Realm GenerateRealm()
    {
        Realm retVal = new Realm();
        retVal.Area = "AlwaysMoveForward";
        retVal.Service = "Blog";
        return retVal;
    }

    [Route("User/Login")]
    public void Login(string blogSubFolder)
    {
        EndpointConfiguration oauthEndpoints = EndpointConfiguration.GetInstance();
        OAuthKeyConfiguration keyConfiguration = OAuthKeyConfiguration.GetInstance();

        IOAuthToken requestToken = this.Services.OAuthClient.GetRequestToken(this.GenerateRealm(), this.Request.Scheme + "://" + this.Request.Host.Value + "/User/OAuthCallback");

        if (requestToken != null)
        {
            HttpContext.Session.SetString(requestToken.Token, System.Text.Json.JsonSerializer.Serialize(requestToken));

            string authorizationUrl = this.Services.OAuthClient.GetUserAuthorizationUrl(requestToken);

            this.Response.Redirect(authorizationUrl);
        }
    }

    [Route("User/Logout")]
    public void Logout()
    {
        this.EliminateUserCookie();
        this.CurrentPrincipal = new SecurityPrincipal(UserFactory.CreateGuestUser());
    }

    [Route("User/Preferences")]
    [HttpGet]
    [BlogMVCAuthorization]
    public IActionResult Preferences(string blogSubFolder)
    {
        UserModel model = this.InitializeUserModel(blogSubFolder);
        model.Common.ContentTitle = "My Account";
        model.CurrentUser = this.CurrentPrincipal.CurrentUser;
        return this.View(model);
    }

    [Route("User/SavePreferences")]
    [BlogMVCAuthorization]
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
    [BlogMVCAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator)]
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
        string requestTokenString = Request.Query[OAuth.Client.Constants.TokenParameter];
        string verifier = Request.Query[OAuth.Client.Constants.VerifierCodeParameter];

        var storedTokenJson = HttpContext.Session.GetString(requestTokenString);
        IOAuthToken storedRequestToken = null;
        if (!string.IsNullOrEmpty(storedTokenJson))
        {
            storedRequestToken = System.Text.Json.JsonSerializer.Deserialize<OAuthToken>(storedTokenJson);
        }

        OAuthKeyConfiguration oauthConfiguration = OAuthKeyConfiguration.GetInstance();
        EndpointConfiguration endpointConfiguration = EndpointConfiguration.GetInstance();

        if (string.IsNullOrEmpty(verifier))
        {
            throw new Exception("Expected a non-empty verifier value");
        }

        IOAuthToken accessToken;

        try
        {
            accessToken = this.Services.OAuthClient.ExchangeRequestTokenForAccessToken(storedRequestToken, verifier);

            AnotherBlogUser amfUser = this.Services.UserService.GetFromAMFUser(accessToken);

            if (amfUser == null)
            {
                this.CurrentPrincipal = new SecurityPrincipal(UserFactory.CreateGuestUser());
                ModelState.AddModelError("loginError", "Invalid login.");
            }
            else
            {
                this.CurrentPrincipal = new SecurityPrincipal(amfUser, true);
                this.EstablishCurrentUserCookie(this.CurrentPrincipal);
            }
        }
        catch (Exception authEx)
        {
            LogManager.GetLogger().Error(authEx);
            return Redirect("AccessDenied.aspx");
        }

        return this.RedirectToAction("Index", "Home");
    }
}

public class OAuthToken : IOAuthToken
{
    public string Token { get; set; }
    public string TokenSecret { get; set; }
}
