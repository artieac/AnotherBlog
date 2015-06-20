using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlwaysMoveForward.AnotherBlog.Web.Models.API
{
    public class BlogInputModel
    {
        public string Name { get; set; }
        public string SubFolder { get; set; }
        public string About { get; set; }
        public string Description { get; set; }
        public string Welcome { get; set; }
        public string Theme { get; set; }
    }
}