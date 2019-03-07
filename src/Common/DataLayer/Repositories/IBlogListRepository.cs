using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.Common.DataLayer;

namespace PucksAndProgramming.AnotherBlog.Common.DataLayer.Repositories
{
    public interface IBlogListRepository : IRepository<BlogList, int>
    {
        IList<BlogList> GetByBlog(int blogId);

        BlogList GetByIdAndBlogId(int listId, int blogId);

        BlogList GetByNameAndBlogId(string name, int blogId);
    }
}
