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
using System.IO;
using System.Xml.Linq;

using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Web.Code.Utilities
{
    public class Utils
    {
        public static string GenerateBlogEntryLink(string blogSubFolder, BlogPost blogPost)
        {
            return Utils.GenerateBlogEntryLink(blogSubFolder, blogPost, string.Empty);
        }

        public static string GenerateBlogEntryLink(string blogSubFolder, BlogPost blogPost, string authority)
        {
            string retVal = string.Empty;

            if (blogPost != null)
            {
                if(!string.IsNullOrEmpty(authority))
                {
                    retVal = "http://" + authority;
                }

                retVal += "/Blog/" + blogSubFolder;
                retVal += "/BlogPost/";

                retVal += blogPost.DatePosted.Year + "/";
                retVal += blogPost.DatePosted.Month + "/";
                retVal += blogPost.DatePosted.Day + "/";
                retVal += HttpUtility.UrlEncode(blogPost.Title.Replace(" ", "_"));
            }

            return retVal;
        }

        public static string GetSecureURL(string blogSubFolder, string targetUrl, string siteAuthority)
        {
            string retVal = string.Empty;

            if (MvcApplication.WebSiteConfiguration.EnableSSL == true)
            {
                retVal = "https://" + siteAuthority;
            }
            else
            {
                retVal = "http://";
            }

            retVal += siteAuthority;

            if (!string.IsNullOrEmpty(blogSubFolder))
            {
                retVal += "/Blog/" + blogSubFolder;
            }

            if (!targetUrl.StartsWith("/"))
            {
                retVal += "/";
            }

            return retVal + targetUrl;
        }

        public static string GetInSecureURL(string blogSubFolder, string targetUrl, string siteAuthority)
        {
            string retVal = "http://" + siteAuthority;

            if (!string.IsNullOrEmpty(blogSubFolder))
            {
                retVal += "/Blog/" + blogSubFolder;
            }

            if (!targetUrl.StartsWith("/"))
            {
                retVal += "/";
            }

            return retVal + targetUrl;
        }

        public static List<string> GetThemeDirectories()
        {
            List<string> retVal = new List<string>();

            string themePath = System.Web.HttpContext.Current.Server.MapPath("~");
            themePath += "/Content/Themes";

            DirectoryInfo themeDirectory = new DirectoryInfo(themePath);
            DirectoryInfo[] themes = themeDirectory.GetDirectories();

            for (int i = 0; i < themes.Length; i++)
            {
                retVal.Add(themes[i].Name);
            }

            return retVal;
        }

        public static bool IsUserInRole(System.Security.Principal.IPrincipal contextUser, string targetSubFolder, string targetRole)
        {
            bool retVal = false;

            AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities.SecurityPrincipal currentPrincipal = contextUser as AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities.SecurityPrincipal;

            if (currentPrincipal != null)
            {
                retVal = currentPrincipal.IsInRole(targetRole, targetSubFolder);
            }

            return retVal;
        }

        public static bool IsUserInRole(System.Security.Principal.IPrincipal contextUser, Blog targetBlog, string targetRole)
        {
            bool retVal = false;

            AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities.SecurityPrincipal currentPrincipal = contextUser as AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities.SecurityPrincipal;

            if (currentPrincipal != null)
            {
                if (targetBlog == null)
                {
                    retVal = currentPrincipal.IsInRole(targetRole);
                }
                else
                {
                    retVal = currentPrincipal.IsInRole(targetRole, targetBlog.SubFolder);
                }
            }

            return retVal;
        }
    }
}
