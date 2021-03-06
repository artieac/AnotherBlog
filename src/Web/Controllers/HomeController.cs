﻿/**
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
using System.Reflection;
using System.IO;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.Web.Models;
using AlwaysMoveForward.AnotherBlog.Web.Models.Home;
using AlwaysMoveForward.AnotherBlog.Web.Models.BlogModels;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers
{
    public class HomeController : PublicController
    {
        [Route("Home/About"), HttpGet()]
        public ActionResult About(string blogSubFolder)
        {
            SiteModel model = new SiteModel();
            model.Common = this.InitializeCommonModel();
            model.Common.Calendar = this.InitializeCalendarModel(model.Common.TargetMonth);
            
            model.SiteInfo = Services.SiteInfoService.GetSiteInfo();

            if (model.SiteInfo == null)
            {
                model.SiteInfo = new SiteInfo();
            }

            return this.View(model);
        }

        [Route("~/", Name = "default")]
        [Route("Home/Index"), HttpGet()]
        public ActionResult Index()
        {
            IndexModel model = new IndexModel();
            model.Common = this.InitializeCommonModel();

            try
            {
                LogManager.GetLogger().Info("Index getting blogs");
                IList<Blog> allBlogs = Services.BlogService.GetAll();
                IList<BlogPost> foundBlogEntries = Services.BlogEntryService.GetMostRecent(10);

                model.BlogEntries = this.PopulateBlogPostInfo(foundBlogEntries);
                model.Common.Calendar = this.InitializeCalendarModel(model.Common.TargetMonth);
            }
            catch(Exception e)
            {
                LogManager.GetLogger().Error(e);
            }

            return this.View(model);
        }

        [Route("BlogPosts/{year}/{month}"), HttpGet()]
        public ActionResult Get(int year, int month)
        {
            IndexModel model = new IndexModel();
            model.Common = this.InitializeCommonModel();

            IList<Blog> allBlogs = Services.BlogService.GetAll();
            IList<BlogPost> foundBlogEntries = null;

            DateTime filterDate = new DateTime(year, month, 1);
            foundBlogEntries = Services.BlogEntryService.GetByMonth(filterDate, true);
            model.Common.ContentTitle = "Blog entries for " + filterDate.ToString("MMMM") + " " + filterDate.ToString("yyyy");
            model.Common.TargetMonth = filterDate;

            model.BlogEntries = this.PopulateBlogPostInfo(foundBlogEntries);
            model.Common.Calendar = this.InitializeCalendarModel(model.Common.TargetMonth);

            return this.View("Index", model);
        }

        [Route("BlogPosts/{year}/{month}/{day}"), HttpGet()]
        public ActionResult Get(int year, int month, int day)
        {
            IndexModel model = new IndexModel();
            model.Common = this.InitializeCommonModel();

            IList<Blog> allBlogs = Services.BlogService.GetAll();
            IList<BlogPost> foundBlogEntries = null;

            DateTime filterDate = new DateTime(year, month, day);
            foundBlogEntries = Services.BlogEntryService.GetByDate(filterDate, true);
            model.Common.ContentTitle = "Blog entries for " + filterDate.ToString("D");
            model.Common.TargetMonth = filterDate;

            model.BlogEntries = this.PopulateBlogPostInfo(foundBlogEntries);
            model.Common.Calendar = this.InitializeCalendarModel(model.Common.TargetMonth);

            return this.View("Index", model);
        }
    }
}
