using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.Entities;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer;
using AlwaysMoveForward.AnotherBlog.DataLayer.Entities;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class BlogListRepository : EntityFrameworkRepository<BlogList, BlogList>, IBlogListRepository
    {
        internal BlogListRepository(IUnitOfWork unitOfWork, RepositoryManager repositoryManager)
            : base(unitOfWork, repositoryManager)
        {
        }

        public override string IdPropertyName
        {
            get { return "Id"; }
        }

        public IList<BlogList> GetByBlog(int blogId)
        {
            IQueryable<BlogList> dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogLists
                                            where foundItem.Blog.BlogId == blogId
                                            select foundItem;        
        
            return dtoList.ToList();
        }
    }
}
