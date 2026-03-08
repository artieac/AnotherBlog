using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories
{
    public interface IBlogListRepository : IRepository<BlogList, long>
    {
        IList<BlogList> GetByBlog(long blogId);

        BlogList GetByIdAndBlogId(long listId, long blogId);

        BlogList GetByNameAndBlogId(string name, long blogId);
    }
}
