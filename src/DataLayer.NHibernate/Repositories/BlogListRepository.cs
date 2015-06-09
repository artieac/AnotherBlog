using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Transform;
using NHibernate.Criterion;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.NHibernate;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer.DTO;
using AlwaysMoveForward.AnotherBlog.DataLayer.DataMapper;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
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
