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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Security.Permissions;

using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;
using AlwaysMoveForward.AnotherBlog.Web.Models;
using AlwaysMoveForward.AnotherBlog.Web.Models.BlogModels;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers
{
    [HandleError]
    [ValidateInput(false)]
    public abstract class BaseController : Controller
    {
        private ServiceManager serviceManager;

        public ServiceManager Services
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

        public SecurityPrincipal CurrentPrincipal
        {
            get 
            {
                SecurityPrincipal retVal = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;

                if (retVal == null)
                {
                    try
                    {
                        retVal = new SecurityPrincipal(this.Services.UserService.GetDefaultUser());
                        System.Threading.Thread.CurrentPrincipal = retVal;
                    }
                    catch (Exception e)
                    {
                        LogManager.GetLogger().Error(e);
                    }
                }

                return retVal;
            }
            set
            {
                System.Threading.Thread.CurrentPrincipal = value;
                this.HttpContext.User = value;
            }
        }

        public IPagedList<BlogPostModel> PopulateBlogPostInfo(IList<BlogPost> blogPosts, int currentPageIndex)
        {
            return new PagedList<BlogPostModel>(this.PopulateBlogPostInfo(blogPosts), currentPageIndex, Constants.PageSize);
        }

        public IList<BlogPostModel> PopulateBlogPostInfo(IList<BlogPost> blogPosts)
        {
            IList<BlogPostModel> retVal = new List<BlogPostModel>();

            if (blogPosts != null)
            {
                for (int i = 0; i < blogPosts.Count; i++)
                {
                    BlogPostModel blogPost = new BlogPostModel();
                    blogPost.Post = blogPosts[i];
                    blogPost.Author = blogPosts[i].Author;
                    blogPost.Tags = blogPosts[i].Tags;
                    retVal.Add(blogPost);
                }
            }

            return retVal;
        }

    }
}
