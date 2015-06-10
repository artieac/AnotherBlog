using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Web.Controllers;
using AlwaysMoveForward.AnotherBlog.Web.Areas.API.Models;

namespace AlwaysMoveForward.AnotherBlog.Web.Areas.API.Controllers
{
    public class BlogAPIController : PublicController
    {
        public JsonResult GetAll()
        {
            ListControlModel model = new ListControlModel();
            model.Title = "Blogs";
            model.ListItems = new List<BlogListItem>();

            IList<Blog> allBlogs = this.Services.BlogService.GetAll();

            for (int i = 0; i < allBlogs.Count; i++)
            {
                BlogListItem newItem = new BlogListItem();
                newItem.Id = allBlogs[i].Id;
                newItem.Name = allBlogs[i].Name;
                newItem.RelatedLink = allBlogs[i].SubFolder;
                model.ListItems.Add(newItem);
            }

            return this.Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTags(string blogSubFolder)
        {
            System.Collections.IList model = new System.Collections.ArrayList();

            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);

            if (targetBlog != null)
            {
                model = this.Services.TagService.GetAllWithCount(targetBlog);
            }

            return this.Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetArchiveDates(string blogSubFolder)
        {
            ListControlModel model = new ListControlModel();
            model.Title = "Archive Dates";
            model.ListItems = new List<BlogListItem>();

            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);

            string urlRoot = "/Home/Month";

            if (targetBlog != null)
            {
                urlRoot = "/" + targetBlog.SubFolder + "/Month";
            }

            System.Collections.IList foundItems = this.Services.BlogEntryService.GetArchiveDates(targetBlog);
            model.ListItems = new List<BlogListItem>();

            if (foundItems != null)
            {
                for (int i = 0; i < foundItems.Count; i++)
                {
                    BlogPostCount currentItem = foundItems[i] as BlogPostCount;

                    if (currentItem != null)
                    {
                        BlogListItem newItem = new BlogListItem();
                        newItem.Name = currentItem.MaxDate.ToString("MMMM") + " " + currentItem.MaxDate.ToString("yyyy");
                        newItem.RelatedLink = urlRoot + "/" + currentItem.MaxDate.ToString("yyyy") + "/" + currentItem.MaxDate.ToString("MM");
                        model.ListItems.Add(newItem);
                    }
                }
            }

            return this.Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetComments(string blogSubFolder, int id)
        {
            IList<Comment> model = new List<Comment>();

            Blog targetBlog = this.GetTargetBlog(blogSubFolder);

            if (targetBlog != null)
            {
                BlogPost targetEntry = Services.BlogEntryService.GetById(targetBlog, id);

                if (targetEntry != null)
                {
                    model = targetEntry.Comments.Where(comment => comment.Status == Comment.CommentStatus.Approved).ToList();
                }
            }

            return this.Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
