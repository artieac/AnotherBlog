/**
 * Copyright (c) 2009 Arthur Correa.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Common Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/cpl1.0.php
 *
 * Contributors:
 *    Arthur Correa – initial contribution
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
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
    public class BlogRepository : NHibernateRepository<Blog, BlogDTO, int>, IBlogRepository
    {

        /// <summary>
        /// This class contains all the code to extract data from the repository using LINQ
        /// </summary>
        /// <param name="dataContext"></param>
        public BlogRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        protected override BlogDTO GetDTOById(Blog domainInstance)
        {
            return this.GetDTOById(domainInstance.Id);
        }

        protected override BlogDTO GetDTOById(int idSource)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogDTO>();
            criteria.Add(Expression.Eq("Id", idSource));

            return criteria.UniqueResult<BlogDTO>();
        }

        protected override DataMapBase<Blog, BlogDTO> GetDataMapper()
        {
            return new BlogDataMap(); 
        }

        /// <summary>
        /// Get a blog as specified by the name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Blog GetByName(string name)
        {
            return this.GetByProperty("Name", name); 
        }
        /// <summary>
        /// Get a blog specified by the site subfolder that contains it.
        /// </summary>
        /// <param name="subFolder"></param>
        /// <returns></returns>
        public Blog GetBySubFolder(string subFolder)
        {
            return this.GetByProperty("SubFolder", subFolder); 
        }
        /// <summary>
        /// Get all blogs that a user is associated with (i.e. ones that the user has security access specifations for it)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<Blog> GetByUserId(long userId)
        {
            DetachedCriteria blogRoles = DetachedCriteria.For<BlogUserDTO>();
            blogRoles.CreateCriteria("User").Add(Expression.Eq("Id", userId));
            blogRoles.SetProjection(Projections.Distinct(Projections.Property("BlogId")));

            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogDTO>();
            criteria.Add(Subqueries.PropertyIn("Id", blogRoles));
            return this.GetDataMapper().Map(criteria.List<BlogDTO>());
        }
    }
}
