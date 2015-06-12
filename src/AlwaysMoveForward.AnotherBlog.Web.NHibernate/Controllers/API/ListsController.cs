using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Web.Models.API;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers.API
{
    public class ListsController : BaseApiController
    {
        private ListControlModel GetMostViewed(string blogSubFolder, int numberToGet)
        {
            ListControlModel retVal = new ListControlModel();
            IList<BlogPost> postList = new List<BlogPost>();
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);

            if (targetBlog == null)
            {
                postList = Services.BlogEntryService.GetMostRead(numberToGet);
            }
            else
            {
                postList = Services.BlogEntryService.GetMostRead(targetBlog.Id, numberToGet);
            }

            retVal.OpenLinkInNewWindow = false;
            retVal.Title = "Most Viewed";
            retVal.ShowOrdered = true;
            retVal.ListItems = new List<BlogListItem>();

            for (int i = 0; i < postList.Count; i++)
            {
                BlogListItem newItem = new BlogListItem();
                newItem.Name = postList[i].Title;
                newItem.RelatedLink = AlwaysMoveForward.AnotherBlog.Web.Code.Utilities.Utils.GenerateBlogEntryLink(postList[i].Blog.SubFolder, postList[i]);
                retVal.ListItems.Add(newItem);
            }

            return retVal;
        }

        [Route("api/Lists/Blogs/All")]
        [HttpGet]
        public ListControlModel Blogs()
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

            return model;
        }

        [Route("api/Lists/BlogPosts/MostViewed")]
        [HttpGet]
        public ListControlModel MostViewed()
        {
            return this.GetMostViewed(string.Empty, 5);
        }

        [Route("api/Blog/{blogSubFolder}/Lists")]
        [HttpGet]
        public IList<ListControlModel> Get(string blogSubFolder)
        {
            IList<ListControlModel> model = new List<ListControlModel>();

            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            IList<BlogList> blogLists = this.Services.BlogListService.GetByBlog(targetBlog);

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

            return model;
        }


        [Route("api/Blog/{blogSubFolder}/Lists/MostViewed")]
        [HttpGet]
        public ListControlModel MostViewed(string blogSubFolder)
        {
            return this.GetMostViewed(blogSubFolder, 5);
        }

        [Route("api/Blog/{blogSubFolder}/Lists/Tags"), HttpGet()]
        public System.Collections.IList GetTags(string blogSubFolder)
        {
            System.Collections.IList model = new System.Collections.ArrayList();

            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);

            if (targetBlog != null)
            {
                model = this.Services.TagService.GetAllWithCount(targetBlog);
            }

            return model;
        }


        [Route("api/Blog/{blogSubFolder}/Lists/ArchiveDates"), HttpGet()]
        public ListControlModel GetArchiveDates(string blogSubFolder)
        {
            ListControlModel model = new ListControlModel();
            model.Title = "Archive Dates";
            model.ListItems = new List<BlogListItem>();

            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);

            string urlRoot = "/";

            if (targetBlog != null)
            {
                urlRoot = "Blog/" + targetBlog.SubFolder + "/";
            }

            urlRoot += "BlogPost/";

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

            return model;
        }
    }
}
