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
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.NHibernate;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.DataLayer.DTO;
using AlwaysMoveForward.AnotherBlog.DataLayer.DataMapper;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    /// <summary>
    /// This class contains all the code to extract AnotherBlogUser data from the repository using LINQ
    /// </summary>
    /// <param name="dataContext"></param>
    public class UserRepository : NHibernateRepository<AnotherBlogUser, UserDTO, long>, IUserRepository
    {
        public UserRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        protected override UserDTO GetDTOById(AnotherBlogUser domainInstance)
        {
            return this.GetDTOById(domainInstance.Id);
        }

        protected override UserDTO GetDTOById(long idSource)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<UserDTO>();
            criteria.Add(Expression.Eq("Id", idSource));
            return criteria.UniqueResult<UserDTO>();
        }

        protected override DataMapBase<AnotherBlogUser, UserDTO> GetDataMapper()
        {
            return new UserDataMap(); 
        }
     
        /// <summary>
        /// Get all users that have the Administrator or Blogger role for the specific blog.
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public IList<AnotherBlogUser> GetBlogWriters(int blogId)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<UserDTO>();
            criteria.CreateCriteria("UserBlogs")
                .CreateCriteria("Blog").Add(Expression.Eq("Id", blogId));
            return this.GetDataMapper().Map(criteria.List<UserDTO>());
        }

        public AnotherBlogUser GetByOAuthServiceUserId(long userId)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<UserDTO>();
            criteria.Add(Expression.Eq("OAuthServiceUserId", userId));
            return this.GetDataMapper().Map(criteria.UniqueResult<UserDTO>());
        }
    }
}
