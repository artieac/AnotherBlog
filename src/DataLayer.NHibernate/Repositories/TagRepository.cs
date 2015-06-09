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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Transform;
using NHibernate.Criterion;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.NHibernate;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer.DTO;
using AlwaysMoveForward.AnotherBlog.DataLayer.DataMapper;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    /// <summary>
    /// This class contains all the code to extract Tag data from the repository using LINQ
    /// </summary>
    /// <param name="dataContext"></param>
    public class TagRepository : NHibernateRepository<Tag, TagDTO, int>, ITagRepository
    {
        public TagRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        protected override TagDTO GetDTOById(Tag domainInstance)
        {
            return this.GetDTOById(domainInstance.Id);
        }

        protected override TagDTO GetDTOById(int idSource)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<TagDTO>();
            criteria.Add(Expression.Eq("Id", idSource));

            return criteria.UniqueResult<TagDTO>();
        }

        protected override DataMapBase<Tag, TagDTO> GetDataMapper()
        {
            return new TagDataMap(); 
        }

        public IList<Tag> GetAll(int blogId)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<TagDTO>();
            criteria.Add(Expression.Eq("BlogId", blogId));
            return this.GetDataMapper().Map(criteria.List<TagDTO>());
        }

        /// <summary>
        /// Get all tags related to a specific blog
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public IList GetAllWithCount(int? blogId)
        {
            string queryString = "SELECT  COUNT(bet.Id) AS Count, t.name as TagName";
            queryString += " FROM Tags t, BlogEntryTags as bet";
            queryString += " WHERE (bet.TagId = t.id)";

            if (blogId.HasValue)
            {
                queryString += " AND (t.BlogId = :targetBlog)";
            }

            queryString += " GROUP BY t.Name";

            ISQLQuery query = ((UnitOfWork)this.UnitOfWork).CurrentSession.CreateSQLQuery(queryString);
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.AddScalar("TagName", NHibernateUtil.String);

            if (blogId.HasValue)
            {
                query.SetParameter("targetBlog", blogId);
            }
            query.SetResultTransformer(new AliasToBeanResultTransformer(typeof(TagCount)));
            return query.List();
        }
        
        /// <summary>
        /// Get a specific tag.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public Tag GetByName(string name, int blogId)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<TagDTO>();
            criteria.Add(Expression.Eq("Name", name));
            criteria.Add(Expression.Eq("BlogId", blogId));
            return this.GetDataMapper().Map(criteria.UniqueResult<TagDTO>());
        }
        /// <summary>
        /// Get multiple tag records.
        /// </summary>
        /// <param name="names"></param>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public IList<Tag> GetByNames(string[] names, int blogId)
        {
            return this.GetDataMapper().Map(this.GetDTOByNames(names, blogId));
        }

        public IList<TagDTO> GetDTOByNames(string[] names, int blogId)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<TagDTO>();
            criteria.Add(Expression.In("Name", names));
            criteria.Add(Expression.Eq("BlogId", blogId));
            return criteria.List<TagDTO>();
        }

        public IList<Tag> GetByBlogEntryId(int blogEntryId)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<TagDTO>();
            criteria.CreateCriteria("BlogEntries").Add(Expression.Eq("Id", blogEntryId));
            return this.GetDataMapper().Map(criteria.List<TagDTO>());
        }
    }
}
