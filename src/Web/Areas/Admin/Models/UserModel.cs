using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PucksAndProgramming.Common.Utilities;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;

namespace PucksAndProgramming.AnotherBlog.Web.Areas.Admin.Models
{
    public class UserModel 
    {
        public AdminCommon Common { get; set; }
        public IPagedList<AnotherBlogUser> Users { get; set; }
        public IDictionary<RoleType.Id, string> Roles { get; set; }
        public IDictionary<int, Blog> Blogs { get; set; }
        public AnotherBlogUser CurrentUser { get; set; }
        public IList<Blog> BlogsUserCanAccess { get; set; }
    }
}
