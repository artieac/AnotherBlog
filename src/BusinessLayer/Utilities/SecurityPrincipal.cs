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
using System.Security.Claims;

using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities
{
    public class SecurityPrincipal : ClaimsPrincipal
    {
        private ServiceManager ServiceManager { get; set; }

        public SecurityPrincipal(ServiceManager serviceManager, AnotherBlogUser currentUser)
            : this(serviceManager, currentUser, false) { }

        public SecurityPrincipal(ServiceManager serviceManager, AnotherBlogUser currentUser, bool isAuthenticated)
            : base(CreateClaimsIdentity(currentUser, isAuthenticated))
        {
            this.ServiceManager = serviceManager;
            this.CurrentUser = currentUser;
        }

        private static ClaimsIdentity CreateClaimsIdentity(AnotherBlogUser user, bool isAuthenticated)
        {
            var claims = new List<Claim>();

            if (user != null)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, user.GetDisplayName() ?? string.Empty));

                if (!string.IsNullOrEmpty(user.Email))
                {
                    claims.Add(new Claim(ClaimTypes.Email, user.Email));
                }

                if (user.IsSiteAdministrator)
                {
                    claims.Add(new Claim(ClaimTypes.Role, RoleType.Names.SiteAdministrator));
                }

                if (user.Roles != null)
                {
                    foreach (var role in user.Roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Value.ToString()));
                    }
                }
            }

            return new ClaimsIdentity(claims, isAuthenticated ? "Cookie" : null);
        }

        public AnotherBlogUser CurrentUser { get; private set; }

        public bool IsAuthenticated => Identity?.IsAuthenticated ?? false;

        public string Name => Identity?.Name ?? string.Empty;

        /// <summary>
        /// Override IsInRole to check against the current user's roles
        /// </summary>
        public override bool IsInRole(string targetRole)
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
                    foreach (long blogId in this.CurrentUser.Roles.Keys)
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

        /// <summary>
        /// Determines if the user is in a specific role for a specific blog
        /// </summary>
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
        /// Checks if the user is in any one of the passed in roles for a specific blog
        /// </summary>
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
