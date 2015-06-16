using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlwaysMoveForward.AnotherBlog.Web.Models.API
{
    public class BlogPostInput
    {
        public bool IsPublished { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Tags { get; set; }
    }
}