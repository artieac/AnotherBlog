﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.IO;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.Web.Models.BlogModels;
using AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Models;
using AlwaysMoveForward.AnotherBlog.Web.Code.Utilities;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;

namespace AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Controllers
{
    public class ManageBlogController : AdminBaseController
    {
        [AdminAuthorizationFilter(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = false)]
        public ActionResult Index()
        {
            ManageBlogModel model = new ManageBlogModel();
            model.Common = this.InitializeCommonModel();

            if(model.Common.TargetBlog == null)
            {
                // in this case just default to the first one
                model.Common.TargetBlog = this.Services.BlogService.GetAll()[0];
            }

            return this.View(model);
        }

        [BlogMVCAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator)]
        public ActionResult GetAll()
        {
            SiteModel model = new SiteModel();
            model.Blogs = Services.BlogService.GetAll();

            return this.View(model);
        }

        [BlogMVCAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator)]
        public ActionResult EditBlog(int id)
        {
            ManageBlogModel model = new ManageBlogModel();

            model.Common.UserBlogs = this.Services.BlogService.GetByUserId(this.CurrentPrincipal.CurrentUser.Id);
            model.Common.TargetBlog = Services.BlogService.GetById(id);

            if (model.Common.TargetBlog == null)
            {
                model.Common.TargetBlog = Services.BlogService.Create();
            }

            return this.View(model);
        }

        [AdminAuthorizationFilter(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator, IsBlogSpecific = true)]
        public ActionResult Preferences(string id, string description, string about, string blogWelcome, bool? performSave, string blogTheme)
        {
            ManageBlogModel model = new ManageBlogModel();
            model.Common = this.InitializeCommonModel(id);

            if (model.Common.TargetBlog != null)
            {
                if (performSave.HasValue)
                {
                    if (performSave.Value == true)
                    {
                        if (string.IsNullOrEmpty(description))
                        {
                            ViewData.ModelState.AddModelError("description", "Please enter a description.");
                        }

                        if (ViewData.ModelState.IsValid)
                        {
                            using (this.Services.UnitOfWork.BeginTransaction())
                            {
                                try
                                {
                                    model.Common.TargetBlog = Services.BlogService.Save(model.Common.TargetBlog.Id, model.Common.TargetBlog.Name, model.Common.TargetBlog.SubFolder, description, about, blogWelcome, blogTheme);
                                    this.Services.UnitOfWork.EndTransaction(true);
                                }
                                catch (Exception e)
                                {
                                    LogManager.GetLogger().Error(e);
                                    this.Services.UnitOfWork.EndTransaction(false);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                model.Common.TargetBlog = model.Common.UserBlogs[0];
            }

            return this.View(model);
        }

        [AdminAuthorizationFilter(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public ActionResult ManagePosts(string id, string filterType, string filterValue, int? page, string sortColumn, bool? sortAscending)
        {
            ManageBlogModel model = new ManageBlogModel();
            model.Common = this.InitializeCommonModel(id);

            if (sortAscending.HasValue)
            {
                model.Common.SortAscending = sortAscending.Value;
            }
            else
            {
                model.Common.SortAscending = false;
            }

            if (sortColumn != null)
            {
                model.Common.SortColumn = sortColumn;
            }
            else
            {
                model.Common.SortColumn = "DateCreated";
            }

            IList<BlogPost> foundPosts = null;
            int currentPageIndex = 0;

            if (model.Common.TargetBlog != null)
            {
                if (page.HasValue == true)
                {
                    currentPageIndex = page.Value - 1;
                }

                if (filterType == "tag")
                {
                    foundPosts = Services.BlogEntryService.GetByTag(model.Common.TargetBlog, filterValue, false);
                }
                else if (filterType == "month")
                {
                    DateTime filterDate = DateTime.ParseExact(filterValue, "MM-dd-yyyy", System.Threading.Thread.CurrentThread.CurrentCulture);
                    foundPosts = Services.BlogEntryService.GetByMonth(model.Common.TargetBlog, filterDate, false);
                }
                else
                {
                    foundPosts = Services.BlogEntryService.GetAllByBlog(model.Common.TargetBlog, false, -1, model.Common.SortColumn, model.Common.SortAscending);
                }
            }
            else
            {
                foundPosts = new PagedList<BlogPost>();
            }

            model.EntryList = this.PopulateBlogPostInfo(foundPosts, currentPageIndex);
            return this.View(model);
        }

        [AdminAuthorizationFilter(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public ActionResult EditPost(string blogSubFolder, bool? performSave, int? id, string title, string entryText, string tagInput, string isPublished)
        {
            ManageBlogModel model = new ManageBlogModel();
            model.Common = this.InitializeCommonModel(blogSubFolder);

            if (model.Common.TargetBlog != null)
            {
                int blogEntryId = 0;

                if (id.HasValue)
                {
                    blogEntryId = id.Value;
                }

                BlogPostModel blogPost = new BlogPostModel();
                blogPost.Author = this.CurrentPrincipal.CurrentUser;
                blogPost.Post = Services.BlogEntryService.GetById(model.Common.TargetBlog, blogEntryId);

                if (blogPost.Post == null)
                {
                    blogPost.Post = new BlogPost();
                }

                if (performSave.HasValue)
                {
                    if (performSave.Value == true)
                    {
                        bool isEntryPublished = false;

                        if (isPublished != null)
                        {
                            if (isPublished == "on")
                            {
                                isEntryPublished = true;
                            }
                        }

                        using (this.Services.UnitOfWork.BeginTransaction())
                        {
                            try
                            {
                                blogPost.Post = Services.BlogEntryService.Save(model.Common.TargetBlog, title, entryText, blogEntryId, isEntryPublished, tagInput.Split(','));
                                blogPost.Tags = blogPost.Post.Tags;
                                this.Services.UnitOfWork.EndTransaction(true);
                            }
                            catch (Exception e)
                            {
                                LogManager.GetLogger().Error(e);
                                this.Services.UnitOfWork.EndTransaction(false);
                            }
                        }
                    }
                }

                blogPost.Tags = blogPost.Post.Tags;
                model.EntryList = new PagedList<BlogPostModel>();
                model.EntryList.Add(blogPost);
            }
            else
            {
                this.RedirectToAction("Index");
            }

            return this.View(model);
        }

        #region Comment Management

        [AdminAuthorizationFilter(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public ActionResult ManageComments(string id, string commentFilter)
        {
            ManageBlogModel model = new ManageBlogModel();
            model.Common = this.InitializeCommonModel(id);
            model.CommentFilter = commentFilter;

            return this.View(model);
        }

        #endregion

        [AdminAuthorizationFilter(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public ActionResult FileUpload(string blogSubFolder)
        {
            AdminCommon model = this.InitializeCommonModel(blogSubFolder);

            foreach (string file in Request.Files)
            {
                HttpPostedFileBase uploadedFile = Request.Files[file] as HttpPostedFileBase;

                if (uploadedFile.ContentLength > 0)
                {
                    string targetPath = Services.UploadedFiles.GeneratePath(model.TargetBlog);

                    if (!Directory.Exists(targetPath))
                    {
                        Directory.CreateDirectory(targetPath);
                    }

                    string savedFileName = Path.Combine(targetPath, Path.GetFileName(uploadedFile.FileName));
                    uploadedFile.SaveAs(savedFileName);
                }
            }

            return this.View(model);
        }
    }
}
