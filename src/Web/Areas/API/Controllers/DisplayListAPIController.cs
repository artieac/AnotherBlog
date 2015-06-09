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
    public class DisplayListAPIController : PublicController
    {
        public JsonResult GetMostViewed(string blogSubFolder)
        {
            ListControlModel retVal = new ListControlModel();
            IList<BlogPost> postList = new List<BlogPost>();
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);

            if (targetBlog == null)
            {
                postList = Services.BlogEntryService.GetMostRead(5);
            }
            else
            {
                postList = Services.BlogEntryService.GetMostRead(targetBlog.Id, 5);
            }

            retVal.OpenLinkInNewWindow = false;
            retVal.Title = "Most Viewed";
            retVal.ShowOrdered = true;
            retVal.ListItems = new List<BlogListItem>();

            for (int i = 0; i < postList.Count; i++)
            {
                BlogListItem newItem = new BlogListItem();
                newItem.Name = postList[i].Title;
                newItem.RelatedLink = AlwaysMoveForward.AnotherBlog.Web.Code.Utilities.Utils.GenerateBlogEntryLink(postList[i].Blog.SubFolder, postList[i], false);
                retVal.ListItems.Add(newItem);
            }

            return this.Json(retVal, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBlogLists(string blogSubFolder)
        {
            IList<ListControlModel> model = new List<ListControlModel>();

            IList<BlogList> blogLists = this.Services.BlogListService.GetByBlog(this.GetTargetBlog(blogSubFolder));

            for (int i = 0; i < blogLists.Count; i++)
            {
                ListControlModel newList = new ListControlModel();
                newList.Title = blogLists[i].Name;
                newList.ShowOrdered = blogLists[i].ShowOrdered;
                newList.OpenLinkInNewWindow = true;
                newList.ListItems = new List<BlogListItem>();

                for (int j = 0; j < blogLists[i].Items.Count; j++)
                {
                    BlogListItem newListItem = new BlogListItem();
                    newListItem.RelatedLink = blogLists[i].Items[j].RelatedLink;
                    newListItem.Name = blogLists[i].Items[j].Name;
                    newList.ListItems.Add(newListItem);
                }

                model.Add(newList);
            }

            return this.Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
