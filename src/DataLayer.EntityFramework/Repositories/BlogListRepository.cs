using System;
using System.Collections.Generic;
using System.Linq;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class BlogListRepository : EntityFrameworkRepository<BlogList, long>, IBlogListRepository
    {
        internal BlogListRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public override string IdPropertyName
        {
            get { return "Id"; }
        }

        public IList<BlogList> GetByBlog(long blogId)
        {
            IQueryable<BlogList> dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogLists
                                           where foundItem.Blog.Id == blogId
                                           select foundItem;

            return dtoList.ToList();
        }

        public BlogList GetByIdAndBlogId(long listId, long blogId)
        {
            return (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogLists
                    where foundItem.Id == listId && foundItem.Blog.Id == blogId
                    select foundItem).SingleOrDefault();
        }

        public BlogList GetByNameAndBlogId(string name, long blogId)
        {
            return (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogLists
                    where foundItem.Name == name && foundItem.Blog.Id == blogId
                    select foundItem).SingleOrDefault();
        }
    }
}
