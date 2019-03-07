using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;

namespace PucksAndProgramming.AnotherBlog.Web.Models.API
{
    public class ExternalBlogPostModel
    {
        public ExternalBlogPostModel(BlogPost source)
        {
            this.Title = source.Title;
            this.AuthorName = source.Author.GetDisplayName();
            this.DatePosted = source.DatePosted;
            this.BlogPostUrl = AnotherBlog.Web.Code.Utilities.Utils.GenerateBlogEntryLink(source.Blog.SubFolder, source);
            this.ShortEntryText = source.ShortEntryText;
        }

        public string Title { get; set; }
        public string AuthorName { get; set; }
        public DateTime DatePosted { get; set; }
        public string BlogPostUrl { get; set; }
        public string ShortEntryText { get; set; }
    }
}