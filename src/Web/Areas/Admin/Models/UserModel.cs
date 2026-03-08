using X.PagedList;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Models
{
    public class UserModel
    {
        public AdminCommon Common { get; set; }
        public IPagedList<AnotherBlogUser> Users { get; set; }
        public IDictionary<RoleType.Id, string> Roles { get; set; }
        public IDictionary<long, Blog> Blogs { get; set; }
        public AnotherBlogUser CurrentUser { get; set; }
        public IList<Blog> BlogsUserCanAccess { get; set; }
    }
}
