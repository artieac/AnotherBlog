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

using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities
{
    public class SecurityPrincipal : IPrincipal, IIdentity
    {
        private ServiceManager serviceManager = null;

        public SecurityPrincipal(AnotherBlogUser currentUser) : this(currentUser, false) { }

        public SecurityPrincipal(AnotherBlogUser currentUser, bool isAuthenticated)
        {
            this.IsAuthenticated = isAuthenticated;
            this.CurrentUser = currentUser;
        }

        public AnotherBlogUser CurrentUser { get; private set; }

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
        /// Implement the IIDentity interface so that it can be used with built in .Net security methods
        /// </summary>
        #region IIdentity

        public bool IsAuthenticated { get; set; }

        public string AuthenticationType
        {
            get { return string.Empty; }
        }

        public string Name
        {
            get 
            {
                string retVal = string.Empty;
                
                if (this.CurrentUser != null)
                {
                    retVal = this.CurrentUser.GetDisplayName();
                }

                return retVal;
            }
        }

        #endregion
        /// <summary>
        /// Implement the IPrincipal interface so that the current user can be thrown onto the current Threads
        /// principal placeholder and passed around cleanly.
        /// </summary>
        #region IPrincipal

        public IIdentity Identity
        {
            get { return this; }
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
                    foreach(int blogId in this.CurrentUser.Roles.Keys)
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

                    if(targetBlog != null)
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
