using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PucksAndProgramming.Common.Utilities;

using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Web.Models.BlogModels;

namespace PucksAndProgramming.AnotherBlog.Web.Areas.Admin.Models
{
    public class ManageBlogModel 
    {
        public ManageBlogModel()
        {
            this.Common = new AdminCommon();
        }

        public AdminCommon Common { get; set; }
        public IPagedList<BlogPostModel> EntryList { get; set; }
        public string CommentFilter { get; set; }
    }
}
