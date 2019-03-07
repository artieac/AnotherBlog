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

using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.BusinessLayer.Service;
using PucksAndProgramming.AnotherBlog.Web.Models.RSS;

namespace PucksAndProgramming.AnotherBlog.Web.Controllers
{
    public class RSSController : PublicController
    {
        private RSSModel GetBlogPosts(Blog targetBlog, RSSModel rssModel)
        {
            IList<Blog> blogList = new List<Blog>();
            blogList.Add(targetBlog);
            return this.GetBlogPosts(blogList, rssModel);
        }

        private RSSModel GetBlogPosts(IList<Blog> targetBlogs, RSSModel rssModel)
        {
            RSSModel retVal = rssModel;

            for (int i = 0; i < targetBlogs.Count; i++)
            {
                IList<BlogPost> blogEntries = Services.BlogEntryService.GetAllByBlog(targetBlogs[i], true);
                retVal.BlogEntries[targetBlogs[i]] = blogEntries;

                DateTime mostRecent = DateTime.MinValue;

                if (blogEntries != null)
                {
                    if (blogEntries.Count > 0)
                    {
                        mostRecent = blogEntries[0].DatePosted;
                    }
                }

                retVal.MostRecentPosts[targetBlogs[i]] = mostRecent;
            }

            return retVal;
        }

        [Route("Rss/Posts"), HttpGet()]
        public ActionResult Posts()
        {
            RSSModel model = new RSSModel();
            model.BlogEntries = new Dictionary<Blog, IList<BlogPost>>();
            model.MostRecentPosts = new Dictionary<Blog, DateTime>();

            model.Scheme = this.Request.Url.Scheme;
            model.Authority = this.Request.Url.Authority;

            IList<Blog> allBlogs = this.Services.BlogService.GetAll();

            model = this.GetBlogPosts(allBlogs, model);

            return this.View(model);
        }

        [Route("Rss/Atom"), HttpGet()]
        public ActionResult Atom()
        {
            RSSModel model = new RSSModel();
            model.BlogEntries = new Dictionary<Blog, IList<BlogPost>>();
            model.MostRecentPosts = new Dictionary<Blog, DateTime>();

            model.Scheme = this.Request.Url.Scheme;
            model.Authority = this.Request.Url.Authority;

            IList<Blog> allBlogs = this.Services.BlogService.GetAll();
            model = this.GetBlogPosts(allBlogs, model);

            DateTime mostRecent = DateTime.MinValue;

            for (int i = 0; i < allBlogs.Count; i++ )
            {
                if (model.BlogEntries[allBlogs[i]] != null)
                {
                    if (model.BlogEntries[allBlogs[i]].Count > 0)
                    {
                        mostRecent = model.BlogEntries[allBlogs[i]][0].DatePosted;
                    }
                }

                model.MostRecentPosts[allBlogs[i]] = mostRecent;
            }

            return this.View(model);
        }

        [Route("Blog/{blogSubFolder}/Rss/Posts"), HttpGet()]
        public ActionResult Posts(string blogSubFolder)
        {
            RSSModel model = new RSSModel();
            model.BlogEntries = new Dictionary<Blog, IList<BlogPost>>();
            model.MostRecentPosts = new Dictionary<Blog, DateTime>();

            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            model.Scheme = this.Request.Url.Scheme;
            model.Authority = this.Request.Url.Authority;
            model = this.GetBlogPosts(targetBlog, model);
            return this.View("Posts", model);
        }

        [Route("Blog/{blogSubFolder}/Rss/Atom"), HttpGet()]
        public ActionResult Atom(string blogSubFolder)
        {
            RSSModel model = new RSSModel();
            model.BlogEntries = new Dictionary<Blog, IList<BlogPost>>();
            model.MostRecentPosts = new Dictionary<Blog, DateTime>();

            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            model.MostRecentPosts = new Dictionary<Blog, DateTime>();
            model = this.GetBlogPosts(targetBlog, model);

            return this.View("Atom", model);
        }
    }
}
