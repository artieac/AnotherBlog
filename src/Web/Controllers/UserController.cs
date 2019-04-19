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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Security.Principal;
using System.Web.Security;
using PucksAndProgramming.Common.Utilities;
using PucksAndProgramming.Common.DomainModel;
using PucksAndProgramming.OAuth.Client.Configuration;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Common.Factories;
using PucksAndProgramming.AnotherBlog.BusinessLayer.Service;
using PucksAndProgramming.AnotherBlog.BusinessLayer.Utilities;
using PucksAndProgramming.AnotherBlog.Web.Models;
using PucksAndProgramming.AnotherBlog.Web.Code.Filters;
using PucksAndProgramming.OAuth.Client;
using Auth0.AuthenticationApi.Models;
using Auth0.AuthenticationApi;
using Microsoft.Owin.Security;
using System.Security.Claims;
using PucksAndProgramming.AnotherBlog.Web.Code;

namespace PucksAndProgramming.AnotherBlog.Web.Controllers
{
    public class UserController : PublicController
    {
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
                // I'm not sure I like having the cookie here, but I'm having a problem passing
                // this user back to the view (even though it worked fine in my Edit method)
                FormsAuthenticationTicket authTicket =
                new FormsAuthenticationTicket(1, currentPrincipal.CurrentUser.Id.ToString(), DateTime.Now, DateTime.Now.AddYears(1), false, string.Empty);

                string encTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie authenticationCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                this.HttpContext.Response.Cookies.Add(authenticationCookie);

                this.CurrentPrincipal = currentPrincipal;                
            }
        }

        private void EliminateUserCookie()
        {
            this.EliminateUserCookie(FormsAuthentication.FormsCookieName);
        }

        private void EliminateUserCookie(string cookieName)
        {
            try
            {
                HttpCookie authCookie = this.Response.Cookies[cookieName];

                if (authCookie != null)
                {
                    authCookie.Expires = DateTime.Now.AddDays(-1);
                }
            }
            catch (Exception e)
            {
                LogManager.GetLogger().Error(e);
            }
        }

        private Realm GenerateRealm()
        {
            Realm retVal = new Realm();
            retVal.Area = "PucksAndProgramming";
            retVal.Service = "Blog";
            return retVal;
        }

        [Route("User/Login")]
        public ActionResult Login(string blogSubFolder)
        {
            string redirectUrl = this.Request.Url.Scheme + "://" + this.Request.Url.Host + ":" + this.Request.Url.Port + "/User/OAuthCallback";

            HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties
                {
                    RedirectUri = redirectUrl
                },
                "Auth0");
            return new HttpUnauthorizedResult();
        }

        [Route("User/Logout")]
        public void Logout()
        {
            this.EliminateUserCookie(SecurityPrincipal.OAuthCookieName);
            this.CurrentPrincipal = new SecurityPrincipal(UserFactory.CreateGuestUser());
        }

        [Route("User/Preferences"), HttpGet()]
        [BlogMVCAuthorization]
        public ActionResult Preferences(string blogSubFolder)
        {
            UserModel model = this.InitializeUserModel(blogSubFolder);
            model.Common.ContentTitle = "My Account";
            model.CurrentUser = this.CurrentPrincipal.CurrentUser;
            return this.View(model);
        }

        [Route("User/SavePreferences")]
        [BlogMVCAuthorization]
        public ActionResult SavePreferences(string blogSubFolder, string userAbout)
        {
            UserModel model = this.InitializeUserModel(blogSubFolder);
            model.Common.ContentTitle = "My Account";

            AnotherBlogUser userToSave = this.CurrentPrincipal.CurrentUser;

            model.CurrentUser = Services.UserService.Save(userToSave.Id, userToSave.IsSiteAdministrator, userToSave.ApprovedCommenter, userAbout);

            return this.View("Preferences", model);
        }

        [Route("User/BlogNavMenu"), HttpGet()]
        public ActionResult BlogNavMenu()
        {
            UserModel model = this.InitializeUserModel();
            return this.View(model);
        }

        [Route("User/ViewUserSocial"), HttpGet()]
        [BlogMVCAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator)]
        public ActionResult ViewUserSocial(string blogSubFolder, string userId)
        {
            UserModel model = this.InitializeUserModel(blogSubFolder);

            int targetUser = int.Parse(userId);
            model.CurrentUser = Services.UserService.GetById(targetUser);

            return this.View(model);
        }

        [Route("User/OAuthCallback"), HttpGet()]
        public ActionResult OAuthCallback()
        {

            var claimsIdentity = User.Identity as ClaimsIdentity;

            // Extract tokens
            string anotherBlogUserId = claimsIdentity?.FindFirst(c => c.Type == SecurityPrincipal.ClaimNames.AnotherBlogUserId)?.Value;
            string accessTokenA = claimsIdentity?.FindFirst(c => c.Type == SecurityPrincipal.ClaimNames.AccessToken)?.Value;
            string auth0Id = SecurityPrincipal.GetRemoteUserId(claimsIdentity);

            AnotherBlogUser amfUser = null;
            long amfUserId = 0;

            if (!String.IsNullOrEmpty(anotherBlogUserId))
            {
                if (!long.TryParse(anotherBlogUserId, out amfUserId))
                {
                    LogManager.GetLogger().Error("Error parsing anotherBlogUserId");
                }
            }
            
            amfUser = this.Services.UserService.GetById(amfUserId);

            if(amfUser != null)
            {
                amfUser.OAuthServiceUserId = auth0Id;
                amfUser.AccessToken = accessTokenA;
                amfUser = this.Services.UserService.Save(amfUser);
                this.CurrentPrincipal = new SecurityPrincipal(amfUser, claimsIdentity);
                this.EstablishCurrentUserCookie(this.CurrentPrincipal);

            }
            else
            {
                string email = claimsIdentity?.FindFirst(c => c.Type == SecurityPrincipal.ClaimNames.Email)?.Value;

                if(!String.IsNullOrEmpty(accessTokenA) && !String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(auth0Id))
                {
                    String firstName = claimsIdentity?.FindFirst(c => c.Type == SecurityPrincipal.ClaimNames.GivenName)?.Value;
                    String lastName = claimsIdentity?.FindFirst(c => c.Type == SecurityPrincipal.ClaimNames.Surname)?.Value;

                    amfUser = new AnotherBlogUser();
                    amfUser.IsSiteAdministrator = false;
                    amfUser.AccessToken = accessTokenA;
                    amfUser.AccessTokenSecret = "";
                    amfUser.OAuthServiceUserId = auth0Id;
                    amfUser.AccessToken = accessTokenA;
                    amfUser.ApprovedCommenter = false;
                    amfUser.Email = email;
                    amfUser.FirstName = firstName;
                    amfUser.LastName = lastName;
                    amfUser = this.Services.UserService.Save(amfUser);
                    this.CurrentPrincipal = new SecurityPrincipal(amfUser, claimsIdentity);
                    this.EstablishCurrentUserCookie(this.CurrentPrincipal);
                }
                else
                {
                    this.CurrentPrincipal = new SecurityPrincipal(UserFactory.CreateGuestUser());
                    ViewData.ModelState.AddModelError("loginError", "Invalid login.");
                }
            }
            return this.RedirectToAction("Index", "Home");
        }
    }
}
