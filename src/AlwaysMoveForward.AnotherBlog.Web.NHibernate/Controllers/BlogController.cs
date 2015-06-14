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

using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.Web.Models;
using AlwaysMoveForward.AnotherBlog.Web.Models.BlogModels;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers
{
    public class BlogController : PublicController
    {
        public CommonBlogModel InitializeCommonModel(string blogSubFolder)
        {
            ViewBag.BlogSubFolder = blogSubFolder;

            CommonBlogModel retVal = new CommonBlogModel();
            retVal = new CommonBlogModel();
            retVal.TargetBlog = Services.BlogService.GetBySubFolder(blogSubFolder);
            retVal.Common = this.InitializeCommonModel(retVal.TargetBlog);
            return retVal;
        }

        public CalendarModel InitializeCalendarModel(Blog targetBlog, DateTime targetMonth)
        {
            CalendarModel retVal = new CalendarModel();
            retVal.RouteInformation = "/Blog/" + targetBlog.SubFolder + "/BlogPost";
            retVal.TargetBlog = targetBlog;
            retVal.TargetMonth = targetMonth;
            retVal.CurrentMonthBlogDates = new List<DateTime>();

            IList<BlogPost> blogDates = Services.BlogEntryService.GetByMonth(retVal.TargetBlog, targetMonth, true);

            for (int i = 0; i < blogDates.Count; i++)
            {
                retVal.CurrentMonthBlogDates.Add(blogDates[i].DatePosted.Date);
            }

            return retVal;
        }

        [Route("Blog/{blogSubFolder}/About"), HttpGet()]
        public ActionResult About(string blogSubFolder)
        {
            BlogModel model = new BlogModel();
            model.BlogCommon = this.InitializeCommonModel(blogSubFolder);
            model.BlogCommon.Common.Calendar = this.InitializeCalendarModel(model.BlogCommon.TargetBlog, model.BlogCommon.Common.TargetMonth);
            model.BlogCommon.Common.ContentTitle = "About " + model.BlogCommon.TargetBlog.Name;

            if (model.BlogCommon.TargetBlog != null)
            {
                model.BlogWriters = Services.UserService.GetBlogWriters(model.BlogCommon.TargetBlog);
            }
            else
            {
                model.BlogWriters = new List<AnotherBlogUser>();
            }

            return this.View(model);
        }

        [Route("Blog/{blogSubFolder}/BlogPosts"), HttpGet()]
        public ActionResult Index(string blogSubFolder, int? page)
        {
            BlogModel model = new BlogModel();
            model.BlogCommon = this.InitializeCommonModel(blogSubFolder);

            IList<BlogPost> foundPosts = null;
            int currentPageIndex = 0;

            if (model.BlogCommon.TargetBlog != null)
            {
                if (page.HasValue == true)
                {
                    currentPageIndex = page.Value - 1;
                }

                if (model.BlogCommon.TargetBlog != null)
                {
                    foundPosts = Services.BlogEntryService.GetAllByBlog(model.BlogCommon.TargetBlog, true);
                }
            }
            else
            {
                foundPosts = new PagedList<BlogPost>();
                model.BlogCommon.Common.ContentTitle = string.Empty;
            }

            model.BlogEntries = this.PopulateBlogPostInfo(foundPosts, currentPageIndex);
            model.BlogCommon.Common.Calendar = this.InitializeCalendarModel(model.BlogCommon.TargetBlog, model.BlogCommon.Common.TargetMonth);
            return this.View(model);
        }

     
        [Route("Blog/{blogSubFolder}/BlogPost/{year}/{month}"), HttpGet()]
        public ActionResult Month(string blogSubFolder, int year, int month, int? page)
        {
            BlogModel model = new BlogModel();
            model.BlogCommon = this.InitializeCommonModel(blogSubFolder);

            IList<BlogPost> foundPosts = null;
            int currentPageIndex = 0;

            if (model.BlogCommon.TargetBlog != null)
            {
                if (page.HasValue == true)
                {
                    currentPageIndex = page.Value - 1;
                }

                if (model.BlogCommon.TargetBlog != null)
                {
                    DateTime filterDate = new DateTime(year, month, 1);
                    model.BlogCommon.Common.TargetMonth = filterDate;

                    foundPosts = Services.BlogEntryService.GetByMonth(model.BlogCommon.TargetBlog, filterDate, true);
                    model.BlogCommon.Common.ContentTitle = "Blog entries for " + filterDate.ToString("MMMM") + " " + filterDate.ToString("yyyy");
                }
            }
            else
            {
                foundPosts = new PagedList<BlogPost>();
                model.BlogCommon.Common.ContentTitle = string.Empty;
            }

            model.BlogEntries = this.PopulateBlogPostInfo(foundPosts, currentPageIndex);
            model.BlogCommon.Common.Calendar = this.InitializeCalendarModel(model.BlogCommon.TargetBlog, model.BlogCommon.Common.TargetMonth);
            return this.View("Index", model);
        }

        [Route("Blog/{blogSubFolder}/BlogPost/{year}/{month}/{day}"), HttpGet()]
        public ActionResult Day(string blogSubFolder, int year, int month, int day, int? page)
        {
            BlogModel model = new BlogModel();
            model.BlogCommon = this.InitializeCommonModel(blogSubFolder);

            IList<BlogPost> foundPosts = null;
            int currentPageIndex = 0;

            if (model.BlogCommon.TargetBlog != null)
            {
                if (page.HasValue == true)
                {
                    currentPageIndex = page.Value - 1;
                }

                if (model.BlogCommon.TargetBlog != null)
                {
                    DateTime filterDate = new DateTime(year, month, day);
                    model.BlogCommon.Common.TargetMonth = filterDate;

                    foundPosts = Services.BlogEntryService.GetByDate(model.BlogCommon.TargetBlog, filterDate, true);
                    model.BlogCommon.Common.ContentTitle = "Blog entries for " + filterDate.ToString("D");
                }
            }
            else
            {
                foundPosts = new List<BlogPost>();
                model.BlogCommon.Common.ContentTitle = string.Empty;
            }

            model.BlogEntries = this.PopulateBlogPostInfo(foundPosts, currentPageIndex);
            model.BlogCommon.Common.Calendar = this.InitializeCalendarModel(model.BlogCommon.TargetBlog, model.BlogCommon.Common.TargetMonth);
            return this.View("Index", model);
        }

        [Route("Blog/{blogSubFolder}/BlogPosts/Tag/{targetTag}"), HttpGet()]
        public ActionResult Tag(string blogSubFolder, string targetTag, int? page)
        {
            BlogModel model = new BlogModel();
            model.BlogCommon = this.InitializeCommonModel(blogSubFolder);

            IList<BlogPost> foundPosts = null;
            int currentPageIndex = 0;

            if (model.BlogCommon.TargetBlog != null)
            {
                if (page.HasValue == true)
                {
                    currentPageIndex = page.Value - 1;
                }

                if (model.BlogCommon.TargetBlog != null)
                {
                    targetTag = AlwaysMoveForward.Common.Utilities.Utils.DecodeFromUrl(targetTag);
                    foundPosts = Services.BlogEntryService.GetByTag(model.BlogCommon.TargetBlog, targetTag, true);
                    model.BlogCommon.Common.ContentTitle = "Blog entries for " + targetTag;
                }
            }
            else
            {
                foundPosts = new PagedList<BlogPost>();
                model.BlogCommon.Common.ContentTitle = string.Empty;
            }

            model.BlogEntries = this.PopulateBlogPostInfo(foundPosts, currentPageIndex);
            model.BlogCommon.Common.Calendar = this.InitializeCalendarModel(model.BlogCommon.TargetBlog, model.BlogCommon.Common.TargetMonth);
            return this.View("Index", model);
        }

        [Route("Blog/{blogSubFolder}/BlogPost/{year}/{month}/{day}/{title}"), HttpGet()]
        public ActionResult Post(string blogSubFolder, string year, string month, string day, string title)
        {
            BlogPostModel model = new BlogPostModel();
            model.BlogCommon = this.InitializeCommonModel(blogSubFolder);

            model.BlogCommon.Common.ContentTitle = "View blog entry";
            model.BlogCommon.Common.Calendar = this.InitializeCalendarModel(model.BlogCommon.TargetBlog, model.BlogCommon.Common.TargetMonth);

            if (model.BlogCommon.TargetBlog != null)
            {
                DateTime postDate = DateTime.Parse(month + "/" + day + "/" + year);
                model.Post = Services.BlogEntryService.GetByDateAndTitle(model.BlogCommon.TargetBlog, postDate, AlwaysMoveForward.Common.Utilities.Utils.DecodeFromUrl(title));

                using (this.Services.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        Services.BlogEntryService.UpdateTimesViewed(model.Post);
                        this.Services.UnitOfWork.EndTransaction(true);
                    }
                    catch (Exception e)
                    {
                        LogManager.GetLogger().Error(e);
                        this.Services.UnitOfWork.EndTransaction(false);
                    }
                }

                model.Author = model.Post.Author;
                model.Tags = model.Post.Tags;
                model.PreviousEntry = Services.BlogEntryService.GetPreviousEntry(model.BlogCommon.TargetBlog, model.Post);
                model.NextEntry = Services.BlogEntryService.GetNextEntry(model.BlogCommon.TargetBlog, model.Post);
            }
            else
            {
                model.Post = new BlogPost();
                model.Tags = new List<Tag>();
                model.Comments = new PagedList<Comment>();
            }

            return this.View(model);
        }
    }
}
