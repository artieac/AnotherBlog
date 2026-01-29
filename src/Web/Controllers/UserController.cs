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

    public UserController(ServiceManagerBuilder serviceManagerBuilder, IDataProtectionProvider dataProtectionProvider)
        : base(serviceManagerBuilder)
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

    [Route("User/Login")]
    public void Login(string blogSubFolder)
    {

    }

    [Route("User/Logout")]
    public void Logout()
    {
        this.EliminateUserCookie();
        this.CurrentPrincipal = new SecurityPrincipal(Services, UserFactory.CreateGuestUser());
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
}
