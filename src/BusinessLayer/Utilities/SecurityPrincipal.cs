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
using System.Text;
using System.Security.Principal;

using PucksAndProgramming.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.BusinessLayer.Service;
using System.Security.Claims;

namespace PucksAndProgramming.AnotherBlog.BusinessLayer.Utilities
{
    public class SecurityPrincipal : IPrincipal
    {
        public const string OAuthCookieName = "OAuthTokenCookie";

        public class ClaimNames
        {
            public const string AnotherBlogUserId = "anotherBlogId";
            public const string IdToken = "id_token";
            public const string AccessToken = "access_token";
            public const string OAuthUserId = "https://www.pucksandprogramming.com/userId";
            public const string Email = "https://www.pucksandprogramming.com/email";
        }

        private ServiceManager serviceManager = null;

        public SecurityPrincipal(AnotherBlogUser currentUser) : this(currentUser, null) { }

        public SecurityPrincipal(AnotherBlogUser currentUser, ClaimsIdentity claimsIdentity)
        {
            this.ClaimsIdentity = claimsIdentity;
            this.CurrentUser = currentUser;
        }

        public AnotherBlogUser CurrentUser { get; private set; }

        public ClaimsIdentity ClaimsIdentity { get; private set; }

        private ServiceManager ServiceManager
        {
            get
            {
                if (this.serviceManager == null)
                {
                    this.serviceManager = ServiceManagerBuilder.BuildServiceManager();
                }

                return this.serviceManager;
            }
        }

        /// <summary>
        /// Implement the IPrincipal interface so that the current user can be thrown onto the current Threads
        /// principal placeholder and passed around cleanly.
        /// </summary>
        #region IPrincipal

        public IIdentity Identity
        {
            get
            {
                if (this.ClaimsIdentity == null)
                {
                    this.ClaimsIdentity = new ClaimsIdentity();
                }

                return this.ClaimsIdentity;
            }
        }
        /// <summary>
        /// Is in role is not really used.  Originally I wanted to use the built in .Net security features (so it was used)
        /// however with the multple blogs/user roles implementation that didn't fit this model anymore so its not used.
        /// </summary>
        /// <param name="targetRole"></param>
        /// <returns></returns>
        public bool IsInRole(string targetRole)
        {
            bool retVal = false;

            if (this.CurrentUser != null)
            {
                if (targetRole == RoleType.Names.SiteAdministrator)
                {
                    retVal = this.CurrentUser.IsSiteAdministrator;
                }

                if (retVal == false)
                {
                    foreach (int blogId in this.CurrentUser.Roles.Keys)
                    {
                        if (this.CurrentUser.Roles[blogId].ToString() == targetRole)
                        {
                            retVal = true;
                            break;
                        }
                    }
                }
            }

            return retVal;
        }

        #endregion
        /// <summary>
        /// The replacement method for the IPrincipal.IsInRole method.  It determines if the user is in a specific
        /// role for a specific blog
        /// </summary>
        /// <param name="targetRole">What role to check the user against.</param>
        /// <param name="blogSubFolder">What blog to check the user against.</param>
        /// <returns></returns>
        public bool IsInRole(string targetRole, string blogSubFolder)
        {
            bool retVal = false;

            if (this.CurrentUser != null)
            {
                if (targetRole.Contains(RoleType.Names.SiteAdministrator))
                {
                    if (this.CurrentUser.IsSiteAdministrator)
                    {
                        retVal = true;
                    }
                }

                if (retVal == false)
                {
                    Blog targetBlog = this.ServiceManager.BlogService.GetBySubFolder(blogSubFolder);

                    if (targetBlog != null)
                    {
                        if (this.CurrentUser.Roles.ContainsKey(targetBlog.Id))
                        {
                            if (this.CurrentUser.Roles[targetBlog.Id].ToString() == targetRole)
                            {
                                retVal = true;
                            }
                        }
                    }
                }
            }

            return retVal;
        }
        /// <summary>
        /// Another version of the IsInRole method.  This one allows the caller to check if the user is in 
        /// any one of the passed in roles.
        /// </summary>
        /// <param name="targetRole"></param>
        /// <param name="targetBlog"></param>
        /// <returns></returns>
        public bool IsInRole(string[] targetRole, Blog targetBlog)
        {
            bool retVal = false;

            if (this.CurrentUser != null)
            {
                if (targetRole != null)
                {
                    if (targetRole.Contains(RoleType.Names.SiteAdministrator))
                    {
                        if (this.CurrentUser.IsSiteAdministrator)
                        {
                            retVal = true;
                        }
                    }
                }

                if (retVal == false)
                {
                    if (targetBlog != null)
                    {
                        if (this.CurrentUser.Roles.ContainsKey(targetBlog.Id))
                        {
                            if (targetRole.Contains(this.CurrentUser.Roles[targetBlog.Id].ToString()))
                            {
                                retVal = true;
                            }
                        }

                    }
                }
            }

            return retVal;
        }
    }
}
