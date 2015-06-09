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
using System.Text;
using NHibernate;
using NHibernate.Transform;
using NHibernate.Criterion;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.Common.DataLayer.NHibernate;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer.DTO;
using AlwaysMoveForward.AnotherBlog.DataLayer.DataMapper;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class BlogEntryRepository : NHibernateRepository<BlogPost, BlogPostDTO, int>, IBlogEntryRepository
    {
        public BlogEntryRepository(UnitOfWork unitOfWork, ITagRepository tagRepository) : base(unitOfWork)
        {
            this.TagRepository = tagRepository as TagRepository;    
        }

        protected TagRepository TagRepository { get; set; }

        protected override BlogPostDTO GetDTOById(BlogPost domainInstance)
        {
            return this.GetDTOById(domainInstance.Id);
        }

        protected override BlogPostDTO GetDTOById(int idSource)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogPostDTO>();
            criteria.Add(Expression.Eq("Id", idSource));
            return criteria.UniqueResult<BlogPostDTO>();
        }

        protected override DataMapBase<BlogPost, BlogPostDTO> GetDataMapper()
        {
            return new BlogPostDataMap(); 
        }

        public IList<BlogPost> GetAll(bool publishedOnly, int maxResults)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogPostDTO>();

            if (publishedOnly == true)
            {
                criteria.Add(Expression.Eq("IsPublished", true));
                criteria.AddOrder(Order.Desc("DatePosted"));
            }
            else
            {
                criteria.AddOrder(Order.Desc("DateCreated"));
            }

            if (maxResults > 0)
            {
                criteria.SetMaxResults(maxResults);
            }

            return this.GetDataMapper().Map(criteria.List<BlogPostDTO>());
        }

        public IList<BlogPost> GetMostRead(int maxResults)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogPostDTO>();
            criteria.Add(Expression.Eq("IsPublished", true));
            criteria.AddOrder(Order.Desc("TimesViewed"));

            if (maxResults > 0)
            {
                criteria.SetMaxResults(maxResults);
            }

            return this.GetDataMapper().Map(criteria.List<BlogPostDTO>());
        }

        public IList<BlogPost> GetMostRead(int blogId, int maxResults)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogPostDTO>();
            criteria.Add(Expression.Eq("IsPublished", true));
            criteria.CreateCriteria("Blog").Add(Expression.Eq("Id", blogId));
            criteria.AddOrder(Order.Desc("TimesViewed"));

            if (maxResults > 0)
            {
                criteria.SetMaxResults(maxResults);
            }

            return this.GetDataMapper().Map(criteria.List<BlogPostDTO>());
        }

        public IList<BlogPost> GetAllByBlog(int blogId, bool publishedOnly, int maxResults, string sortColumn, bool sortAscending)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogPostDTO>();
            criteria.CreateCriteria("Blog").Add(Expression.Eq("Id", blogId));

            if (publishedOnly == true)
            {
                criteria.Add(Expression.Eq("IsPublished", true));
            }

            if (sortAscending == true)
            {
                criteria.AddOrder(Order.Asc(sortColumn));
            }
            else
            {
                criteria.AddOrder(Order.Desc(sortColumn));
            }

            if (maxResults > 0)
            {
                criteria.SetMaxResults(maxResults);
            }

            return this.GetDataMapper().Map(criteria.List<BlogPostDTO>());
        }

        public BlogPost GetByTitle(string blogTitle, int blogId)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogPostDTO>();
            criteria.Add(Expression.Eq("Title", blogTitle));
            criteria.CreateCriteria("Blog").Add(Expression.Eq("Id", blogId));

            return this.GetDataMapper().Map(criteria.UniqueResult<BlogPostDTO>());
        }

        public BlogPost GetByDateAndTitle(string blogTitle, DateTime postDate, int blogId)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogPostDTO>();
            criteria.Add(Expression.Eq("Title", blogTitle));
            criteria.CreateCriteria("Blog").Add(Expression.Eq("Id", blogId));
            criteria.Add(Restrictions.Eq(Projections.SqlFunction("year", NHibernate.NHibernateUtil.DateTime, Projections.Property("DatePosted")), postDate.Date.Year));
            criteria.Add(Restrictions.Eq(Projections.SqlFunction("month", NHibernate.NHibernateUtil.DateTime, Projections.Property("DatePosted")), postDate.Date.Month));
            criteria.Add(Restrictions.Eq(Projections.SqlFunction("day", NHibernate.NHibernateUtil.DateTime, Projections.Property("DatePosted")), postDate.Date.Day));

            return this.GetDataMapper().Map(criteria.UniqueResult<BlogPostDTO>());
        }

        public IList<BlogPost> GetByTag(int tagId, bool publishedOnly)
        {
            return this.GetByTag(null, tagId, publishedOnly);
        }

        public IList<BlogPost> GetByTag(int? blogId, int tagId, bool publishedOnly)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogPostDTO>();

            if (blogId.HasValue)
            {
                criteria.CreateCriteria("Blog").Add(Expression.Eq("Id", blogId.Value));
            }

            if (publishedOnly == true)
            {
                criteria.Add(Expression.Eq("IsPublished", true));
            }

            criteria.CreateCriteria("Tags").Add(Expression.Eq("Id", tagId));
            criteria.AddOrder(Order.Desc("DatePosted"));

            return this.GetDataMapper().Map(criteria.List<BlogPostDTO>());
        }

        public IList<BlogPost> GetByTag(int blogId, string tagText, bool publishedOnly)
        {
            IList<BlogPost> retVal = new List<BlogPost>();

            ICriteria tagCriteria = this.UnitOfWork.CurrentSession.CreateCriteria<TagDTO>();
            tagCriteria.Add(Expression.Eq("Name", tagText));
            tagCriteria.Add(Expression.Eq("BlogId", blogId));

            TagDTO targetTag = tagCriteria.UniqueResult<TagDTO>();

            if (targetTag != null)
            {
                ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogPostDTO>();
                criteria.CreateCriteria("Blog").Add(Expression.Eq("Id", blogId));

                if (publishedOnly == true)
                {
                    criteria.Add(Expression.Eq("IsPublished", true));
                }

                criteria.CreateCriteria("Tags").Add(Expression.Eq("Id", targetTag.Id));
                criteria.AddOrder(Order.Desc("DatePosted"));
                retVal = this.GetDataMapper().Map(criteria.List<BlogPostDTO>());
            }

            return retVal;
        }

        public IList<BlogPost> GetByMonth(DateTime blogDate, bool publishedOnly)
        {
            return this.GetByMonth(blogDate, null, publishedOnly);
        }

        public IList<BlogPost> GetByMonth(DateTime blogDate, int? blogId, bool publishedOnly)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogPostDTO>();

            if (blogId.HasValue)
            {
                criteria.CreateCriteria("Blog").Add(Expression.Eq("Id", blogId.Value));
            }
        
            if (publishedOnly == true)
            {
                criteria.Add(Expression.Eq("IsPublished", true));
            }

            criteria.Add(Restrictions.Eq(Projections.SqlFunction("year", NHibernate.NHibernateUtil.DateTime, Projections.Property("DatePosted")), blogDate.Date.Year));
            criteria.Add(Restrictions.Eq(Projections.SqlFunction("month", NHibernate.NHibernateUtil.DateTime, Projections.Property("DatePosted")), blogDate.Date.Month));
            criteria.AddOrder(Order.Desc("DatePosted"));

            return this.GetDataMapper().Map(criteria.List<BlogPostDTO>());
        }

        public IList<BlogPost> GetByDate(DateTime blogDate, bool publishedOnly)
        {
            return this.GetByDate(blogDate, null, publishedOnly);
        }

        public IList<BlogPost> GetByDate(DateTime blogDate, int? blogId, bool publishedOnly)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogPostDTO>();

            if (blogId.HasValue)
            {
                criteria.CreateCriteria("Blog").Add(Expression.Eq("Id", blogId.Value));
            }

            if (publishedOnly == true)
            {
                criteria.Add(Expression.Eq("IsPublished", true));
            }

            criteria.Add(Restrictions.Eq(Projections.SqlFunction("year", NHibernate.NHibernateUtil.DateTime, Projections.Property("DatePosted")), blogDate.Date.Year));
            criteria.Add(Restrictions.Eq(Projections.SqlFunction("month", NHibernate.NHibernateUtil.DateTime, Projections.Property("DatePosted")), blogDate.Date.Month));
            criteria.Add(Restrictions.Eq(Projections.SqlFunction("day", NHibernate.NHibernateUtil.DateTime, Projections.Property("DatePosted")), blogDate.Date.Day));
            criteria.AddOrder(Order.Desc("DatePosted"));

            return this.GetDataMapper().Map(criteria.List<BlogPostDTO>());
        }

        public BlogPost GetMostRecent(int blogId, bool published)
        {
            DetachedCriteria getMaxEntryId = DetachedCriteria.For<BlogPostDTO>();
            getMaxEntryId.CreateCriteria("Blog").Add(Expression.Eq("Id", blogId));
            getMaxEntryId.Add(Expression.Eq("IsPublished", true));
            getMaxEntryId.SetProjection(Projections.Max("Id"));

            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogPostDTO>();
            criteria.CreateCriteria("Blog").Add(Expression.Eq("int", blogId));
            criteria.Add(Expression.Eq("IsPublished", true));
            criteria.Add(Subqueries.PropertyEq("Id", getMaxEntryId));

            return this.GetDataMapper().Map(criteria.UniqueResult<BlogPostDTO>());
        }

        public BlogPost GetPreviousEntry(int blogId, int currentPostId)
        {
            DetachedCriteria getMaxEntryId = DetachedCriteria.For<BlogPostDTO>();
            getMaxEntryId.CreateCriteria("Blog").Add(Expression.Eq("Id", blogId));
            getMaxEntryId.Add(Expression.Eq("IsPublished", true));
            getMaxEntryId.Add(Expression.Lt("Id", currentPostId));
            getMaxEntryId.SetProjection(Projections.Max("Id"));

            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogPostDTO>();
            criteria.CreateCriteria("Blog").Add(Expression.Eq("Id", blogId));
            criteria.Add(Expression.Eq("IsPublished", true));
            criteria.Add(Subqueries.PropertyEq("Id", getMaxEntryId));

            return this.GetDataMapper().Map(criteria.UniqueResult<BlogPostDTO>());
        }

        public BlogPost GetNextEntry(int blogId, int currentPostId)
        {
            DetachedCriteria getMaxEntryId = DetachedCriteria.For<BlogPostDTO>();
            getMaxEntryId.CreateCriteria("Blog").Add(Expression.Eq("Id", blogId));
            getMaxEntryId.Add(Expression.Eq("IsPublished", true));
            getMaxEntryId.Add(Expression.Gt("Id", currentPostId));
            getMaxEntryId.SetProjection(Projections.Min("Id"));

            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogPostDTO>();
            criteria.CreateCriteria("Blog").Add(Expression.Eq("Id", blogId));
            criteria.Add(Expression.Eq("IsPublished", true));
            criteria.Add(Subqueries.PropertyEq("Id", getMaxEntryId));

            return this.GetDataMapper().Map(criteria.UniqueResult<BlogPostDTO>());
        }

        public IList<DateTime> GetPublishedDatesByMonth(DateTime blogDate)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogPostDTO>();
            ProjectionList projections = Projections.ProjectionList();
            criteria.SetProjection(Projections.Distinct(Projections.Alias(Projections.Property("DatePosted"), "DatePosted")));
            criteria.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(BlogPostDTO)));

            IList<BlogPostDTO> foundDates = criteria.List<BlogPostDTO>();

            IList<DateTime> retVal = new List<DateTime>();

            for (int i = 0; i < foundDates.Count; i++)
            {
                retVal.Add(foundDates[i].DatePosted);
            }

            return retVal;
        }

        public IList GetArchiveDates(int? blogId)
        {
            string queryString = "SELECT  COUNT(*) AS PostCount, Max(DatePosted) AS MaxDate";
            queryString += " FROM BlogEntries";
            queryString += " WHERE (IsPublished = 1)";

            if (blogId.HasValue)
            {
                queryString += " AND (BlogId = :targetBlog)";
            }

            queryString += " GROUP BY YEAR(DatePosted), MONTH(DatePosted)" + " ORDER BY MaxDate";

            ISQLQuery query = this.UnitOfWork.CurrentSession.CreateSQLQuery(queryString);

            query.AddScalar("PostCount", NHibernateUtil.Int32);
            query.AddScalar("MaxDate", NHibernateUtil.DateTime);

            if (blogId.HasValue)
            {
                query.SetParameter("targetBlog", blogId.Value);
            }
            query.SetResultTransformer(new AliasToBeanResultTransformer(typeof(BlogPostCount)));
            return query.List();
        }

        public BlogPost GetByCommentId(int commentId)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<BlogPostDTO>();
            criteria.CreateCriteria("Comments").Add(Expression.Eq("Id", commentId));
            return this.GetDataMapper().Map(criteria.UniqueResult<BlogPostDTO>());
        }

        public override BlogPost Save(BlogPost itemToSave)
        {
            if (itemToSave != null && itemToSave.Tags != null)
            {
                BlogPostDTO dtoPost = this.GetDTOById(itemToSave.Id);

                if (dtoPost != null)
                {
                    foreach (Tag domainTag in itemToSave.Tags)
                    {
                        if (dtoPost.Tags.FirstOrDefault(t => t.Id == domainTag.Id) == null)
                        {
                            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<TagDTO>();
                            criteria.Add(Expression.Eq("Id", domainTag.Id));
                            TagDTO existsTest = criteria.UniqueResult<TagDTO>();

                            if (existsTest != null)
                            {
                                dtoPost.Tags.Add(existsTest);
                            }
                        }
                    }
                }
            }

            return base.Save(itemToSave);
        }
    }
}
