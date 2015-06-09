using System;
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
    [RequestAuthenticationFilter]
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

        [CustomAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator)]
        public ActionResult GetAll()
        {
            SiteModel model = new SiteModel();
            model.Blogs = Services.BlogService.GetAll();

            return this.View(model);
        }

        [CustomAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator)]
        public ActionResult EditBlog(int id, string blogName, string blogAbout, string blogDescription, string targetSubFolder, string blogWelcome, string savingBlog, string blogTheme)
        {
            ManageBlogModel model = new ManageBlogModel();

            model.Common.UserBlogs = this.Services.BlogService.GetByUserId(this.CurrentPrincipal.CurrentUser.Id);

            if (savingBlog != null)
            {
                if (string.IsNullOrEmpty(blogName))
                {
                    ViewData.ModelState.AddModelError("blogName", "Blog name is required.");
                }

                if (string.IsNullOrEmpty(targetSubFolder))
                {
                    ViewData.ModelState.AddModelError("targetSubFolder", "Sub folder is required.");
                }

                int targetBlogId = id;

                if (ViewData.ModelState.IsValid)
                {
                    using (this.Services.UnitOfWork.BeginTransaction())
                    {
                        try
                        {
                            model.Common.TargetBlog = Services.BlogService.Save(targetBlogId, blogName, targetSubFolder, blogDescription, blogAbout, blogWelcome, blogTheme);
                            this.Services.UnitOfWork.EndTransaction(true);
                        }
                        catch (Exception e)
                        {
                            model.Common.TargetBlog = Services.BlogService.Create();
                            model.Common.TargetBlog.Name = blogName;
                            model.Common.TargetBlog.About = blogAbout;
                            model.Common.TargetBlog.Description = blogDescription;
                            model.Common.TargetBlog.SubFolder = targetSubFolder;
                            model.Common.TargetBlog.WelcomeMessage = blogWelcome;

                            LogManager.GetLogger().Error(e);
                            this.Services.UnitOfWork.EndTransaction(false);
                        }
                    }
                }
                else
                {
                    model.Common.TargetBlog = Services.BlogService.GetById(targetBlogId);

                    if (model.Common.TargetBlog == null)
                    {
                        model.Common.TargetBlog = Services.BlogService.Create();
                        model.Common.TargetBlog.Name = blogName;
                        model.Common.TargetBlog.About = blogAbout;
                        model.Common.TargetBlog.Description = blogDescription;
                        model.Common.TargetBlog.SubFolder = targetSubFolder;
                        model.Common.TargetBlog.WelcomeMessage = blogWelcome;
                    }
                }
            }
            else
            {
                model.Common.TargetBlog = Services.BlogService.GetById(id);

                if(model.Common.TargetBlog == null)
                {
                    model.Common.TargetBlog = Services.BlogService.Create();
                }
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

        [AdminAuthorizationFilter(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public JsonResult AjaxPostSave(string blogSubFolder, string ajaxTitle, string ajaxEntryText, string ajaxEntryId, string ajaxTagInput, string ajaxIsPublished)
        {
            ManageBlogModel model = new ManageBlogModel();
            model.Common = this.InitializeCommonModel(blogSubFolder);

            BlogPost currentPost = new BlogPost();

            if (model.Common.TargetBlog != null)
            {
                int blogEntryId = 0;

                if (!string.IsNullOrEmpty(ajaxEntryId))
                {
                    blogEntryId = int.Parse(ajaxEntryId);
                }

                bool isEntryPublished = false;

                if (ajaxIsPublished != null)
                {
                    if (ajaxIsPublished == "true")
                    {
                        isEntryPublished = true;
                    }
                }

                using (this.Services.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        currentPost = Services.BlogEntryService.Save(model.Common.TargetBlog, ajaxTitle, ajaxEntryText, blogEntryId, isEntryPublished, ajaxTagInput.Split(','));
                        currentPost.Tags = currentPost.Tags;
                        this.Services.UnitOfWork.EndTransaction(true);
                    }
                    catch (Exception e)
                    {
                        LogManager.GetLogger().Error(e);
                        this.Services.UnitOfWork.EndTransaction(false);
                    }
                }
            }

            AjaxSaveModel retVal = new AjaxSaveModel();
            retVal.EntryId = currentPost.Id;
            retVal.BlogSubFolder = model.Common.TargetBlog.SubFolder;

            return this.Json(retVal);
        }

        #region Comment Management

        [AdminAuthorizationFilter(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public JsonResult GetComments(string blogSubFolder, string id)
        {
            IList<CommentItemModel> model = new List<CommentItemModel>();

            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);

            if (targetBlog != null)
            {
                IList<BlogPost> posts = this.Services.BlogEntryService.GetAllByBlog(targetBlog, true);

                if (id == null || string.Compare(id, "All", true) == 0)
                {
                    foreach (BlogPost post in posts)
                    {
                        foreach(Comment comment in post.Comments)
                        {
                            CommentItemModel newItem = new CommentItemModel(comment);
                            newItem.BlogPostId = post.Id;
                            model.Add(newItem);
                        }
                    }
                }
                else
                {
                    Comment.CommentStatus targetStatus = (Comment.CommentStatus)Enum.Parse(typeof(Comment.CommentStatus), id);

                    foreach (BlogPost post in posts)
                    {
                        foreach (Comment comment in post.FilteredComments(targetStatus))
                        {
                            CommentItemModel newItem = new CommentItemModel(comment);
                            newItem.BlogPostId = post.Id;
                            model.Add(newItem);
                        }
                    }
                }
            }

            return this.Json(model, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorizationFilter(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public ActionResult ApproveComment(string blogSubFolder, int blogPostId, int filter)
        {
            ManageBlogModel model = new ManageBlogModel();
            model.Common = this.InitializeCommonModel(blogSubFolder);

            if (model.Common.TargetBlog != null)
            {
                using (this.Services.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        BlogPost blogPost = this.Services.BlogEntryService.GetById(model.Common.TargetBlog, blogPostId);

                        if(blogPost!=null)
                        {
                            blogPost.UpdateCommentStatus(filter, Comment.CommentStatus.Approved);
                            this.Services.BlogEntryService.Save(blogPost);
                        }

                        this.Services.UnitOfWork.EndTransaction(true);
                    }
                    catch (Exception e)
                    {
                        LogManager.GetLogger().Error(e);
                        this.Services.UnitOfWork.EndTransaction(false);
                    }
                }
            }

            return this.RedirectToAction("ManageComments", new { blogSubFolder = blogSubFolder });
        }

        [AdminAuthorizationFilter(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public ActionResult DeleteComment(string blogSubFolder, int blogPostId, int filter)
        {
            using (this.Services.UnitOfWork.BeginTransaction())
            {
                try
                {
                    ManageBlogModel model = new ManageBlogModel();
                    model.Common = this.InitializeCommonModel(blogSubFolder);

                    if (model.Common.TargetBlog != null)
                    {
                        BlogPost blogPost = this.Services.BlogEntryService.GetById(model.Common.TargetBlog, blogPostId);

                        if(blogPost != null)
                        {
                            blogPost.UpdateCommentStatus(filter, Comment.CommentStatus.Deleted);
                            this.Services.BlogEntryService.Save(blogPost);
                        }

                        this.Services.UnitOfWork.EndTransaction(true);
                    }
                }
                catch (Exception e)
                {
                    LogManager.GetLogger().Error(e);
                    this.Services.UnitOfWork.EndTransaction(false);
                }
            }

            return this.RedirectToAction("ManageComments", new { blogSubFolder = blogSubFolder });
        }

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
