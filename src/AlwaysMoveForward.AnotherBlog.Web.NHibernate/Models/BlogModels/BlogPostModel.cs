using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Web.Models.BlogModels
{
    public class BlogPostModel
    {
        public CommonBlogModel BlogCommon { get; set; }
        public BlogPost Post { get; set; }
        public AnotherBlogUser Author { get; set; }
        public IList<Tag> Tags { get; set; }
        public IList<Comment> Comments { get; set; }
        public BlogPost PreviousEntry { get; set; }
        public BlogPost NextEntry { get; set; }
    }
}