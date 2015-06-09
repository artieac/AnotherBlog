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

using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.Web.Models.BlogModels;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers
{
    public class RSSController : PublicController
    {
        public RSSModel InitializeRSSModel(string blogSubFolder)
        {
            RSSModel retVal = new RSSModel();
            retVal.BlogCommon = new CommonBlogModel();
            retVal.BlogCommon.TargetBlog = Services.BlogService.GetBySubFolder(blogSubFolder);
            retVal.BlogCommon.Common = this.InitializeCommonModel(retVal.BlogCommon.TargetBlog);
            return retVal;
        }

        public ActionResult Index()
        {
            // Add action logic here
            throw new NotImplementedException();
        }

        public ActionResult Posts(string blogSubFolder)
        {
            RSSModel model = this.InitializeRSSModel(blogSubFolder);
            model.BlogEntries = new Dictionary<Blog, IList<BlogPost>>();

            model.Scheme = this.Request.Url.Scheme;
            model.Authority = this.Request.Url.Authority;

            Blog targetBlog = Services.BlogService.GetBySubFolder(blogSubFolder);

            if (targetBlog == null)
            {
                IList<Blog> allBlogs = this.Services.BlogService.GetAll();

                for (int i = 0; i < allBlogs.Count; i++)
                {
                    model.BlogEntries[allBlogs[i]] = Services.BlogEntryService.GetAllByBlog(allBlogs[i], true, 10);
                }
            }
            else
            {
                model.BlogEntries[targetBlog] = Services.BlogEntryService.GetAllByBlog(targetBlog, true, 10);
            }

            return this.View(model);
        }

        public ActionResult Atom(string blogSubFolder)
        {
            RSSModel model = this.InitializeRSSModel(blogSubFolder);
            model.BlogEntries = new Dictionary<Blog, IList<BlogPost>>();
            model.MostRecentPosts = new Dictionary<Blog, DateTime>();

            Blog targetBlog = Services.BlogService.GetByName(blogSubFolder);

            if (targetBlog == null)
            {
                IList<Blog> allBlogs = this.Services.BlogService.GetAll();

                for (int i = 0; i < allBlogs.Count; i++)
                {
                    IList<BlogPost> blogEntries = Services.BlogEntryService.GetAllByBlog(allBlogs[i], true);
                    model.BlogEntries[allBlogs[i]] = blogEntries;

                    DateTime mostRecent = DateTime.MinValue;

                    if (blogEntries != null)
                    {
                        if (blogEntries.Count > 0)
                        {
                            mostRecent = blogEntries[0].DatePosted;
                        }
                    }

                    model.MostRecentPosts[allBlogs[i]] = mostRecent;
                }
            }
            else
            {
                IList<BlogPost> blogEntries = Services.BlogEntryService.GetAllByBlog(targetBlog, true);
                model.BlogEntries[targetBlog] = blogEntries;

                DateTime mostRecent = DateTime.MinValue;

                if (blogEntries != null)
                {
                    if (blogEntries.Count > 0)
                    {
                        mostRecent = blogEntries[0].DatePosted;
                    }
                }

                model.MostRecentPosts[targetBlog] = mostRecent;
            }

            return this.View(model);
        }
    }
}
