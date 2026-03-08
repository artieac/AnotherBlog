using X.PagedList;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Web.Models.BlogModels;

namespace AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Models
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
