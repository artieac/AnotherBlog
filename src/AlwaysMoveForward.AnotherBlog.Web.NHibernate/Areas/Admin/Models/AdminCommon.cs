using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Models
{
    public class AdminCommon
    {
        public Blog TargetBlog { get; set; }
        public bool SortAscending { get; set; }
        public IList<Blog> UserBlogs { get; set; }
        public string SortColumn { get; set; }
    }
}