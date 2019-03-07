using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Transform;
using NHibernate.Criterion;
using PucksAndProgramming.Common.DataLayer;
using PucksAndProgramming.Common.DataLayer.NHibernate;
using PucksAndProgramming.Common.DataLayer.Repositories;
using PucksAndProgramming.AnotherBlog.Common.DataLayer;
using PucksAndProgramming.AnotherBlog.Common.DataLayer.Map;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Common.DataLayer.Repositories;
using PucksAndProgramming.AnotherBlog.DataLayer.DTO;
using PucksAndProgramming.AnotherBlog.DataLayer.DataMapper;

namespace PucksAndProgramming.AnotherBlog.DataLayer.Repositories
{
    public class BlogListRepository : NHibernateRepository<BlogList, BlogListDTO, int>, IBlogListRepository
    {
        public BlogListRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        protected override BlogListDTO GetDTOById(BlogList domainInstance)
        {
            return this.GetDTOById(domainInstance.Id);
        }

        protected override BlogListDTO GetDTOById(int idSource)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogListDTO>();
            criteria.Add(Expression.Eq("Id", idSource));

            return criteria.UniqueResult<BlogListDTO>();
        }

        protected override DataMapBase<BlogList, BlogListDTO> GetDataMapper()
        {
            return new ListDataMap();
        }

        public BlogList GetByIdAndBlogId(int listId, int blogId)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogListDTO>();
            criteria.Add(Expression.Eq("Id", listId));
            criteria.Add(Expression.Eq("BlogId", blogId));
            return this.GetDataMapper().Map(criteria.UniqueResult<BlogListDTO>());
        }

        public IList<BlogList> GetByBlog(int blogId)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogListDTO>();
            criteria.Add(Expression.Eq("BlogId", blogId));
            return this.GetDataMapper().Map(criteria.List<BlogListDTO>());
        }

        public BlogList GetByNameAndBlogId(string name, int blogId)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogListDTO>();
            criteria.Add(Expression.Eq("Name", name));
            criteria.Add(Expression.Eq("BlogId", blogId));
            return this.GetDataMapper().Map(criteria.UniqueResult<BlogListDTO>());
        }
    }
}
