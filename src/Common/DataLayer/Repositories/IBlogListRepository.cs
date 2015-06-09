using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories
{
    public interface IBlogListRepository : IRepository<BlogList, int>
    {
        IList<BlogList> GetByBlog(int blogId);

        BlogList GetByIdAndBlogId(int listId, int blogId);

        BlogList GetByNameAndBlogId(string name, int blogId);
    }
}
