using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Models
{
    public class BlogListModel 
    {
        public AdminCommon Common { get; set; }
        public IList<BlogList> BlogLists { get; set; }
        public BlogList CurrentList { get; set; }
        public IList<BlogListItem> CurrentListItems { get; set; }
    }
}