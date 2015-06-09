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
using System.Data.Objects;

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
    public class BlogEntryRepository : EntityFrameworkRepository<BlogPost, BlogPost>, IBlogEntryRepository
    {
        internal BlogEntryRepository(IUnitOfWork unitOfWork, RepositoryManager repositoryManager)
            : base(unitOfWork, repositoryManager)
        {

        }

        public override string IdPropertyName
        {
            get { return "EntryId"; }
        }

        public override string TableName
        {
            get{ return "BlogEntries";}
        }

        public IList<BlogPost> GetAll(bool publishedOnly, int maxResults)
        {
            IQueryable<BlogPost> dtoList = null;

            if (publishedOnly == true)
            {
                dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                          where foundItem.IsPublished == true
                          orderby foundItem.DatePosted descending
                          select foundItem;
            }
            else
            {
                dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                          orderby foundItem.DatePosted descending
                          select foundItem;
            }

            if (maxResults > 0)
            {
                //                dtoList.M
            }

            return dtoList.ToList<BlogPost>();
        }

        public IList<BlogPost> GetAllByBlog(int blogId, bool publishedOnly, int maxResults, string sortColumn, bool sortAscending)
        {
            IQueryable<BlogPost> dtoList = null;

            if (publishedOnly == true)
            {
                dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                          where foundItem.IsPublished == true &&
                          foundItem.Blog.BlogId == blogId
                          select foundItem;
            }
            else
            {
                dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                          where foundItem.Blog.BlogId == blogId
                          select foundItem;
            }

            if (sortAscending == true)
            {
                dtoList = this.ApplyOrder(dtoList, sortColumn, "OrderByAscending");
            }
            else
            {
                dtoList = this.ApplyOrder(dtoList, sortColumn, "OrderByDescending");
            }

            if (maxResults > 0)
            {
                //                dtoList.M
            }

            return dtoList.ToList();
        }

        public IList<BlogPost> GetMostRead(int maxResults)
        {
            IList<BlogPost> retVal = null;

            IQueryable<BlogPost> dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                                           where foundItem.IsPublished==true
                                           select foundItem;

//            dtoList = this.ApplyOrder(dtoList, "TimesViewed", "OrderByAscending");

            if (maxResults > 0)
            {
                retVal = dtoList.ToList<BlogPost>().Take(maxResults).ToList<BlogPost>();
            }
            else
            {
                retVal = dtoList.ToList<BlogPost>();
            }

            return retVal;
        }

        public IList<BlogPost> GetMostRead(int blogId, int maxResults)
        {
            IList<BlogPost> retVal = null;
       
            IQueryable<BlogPost> dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                                           where foundItem.IsPublished == true &&
                                           foundItem.Blog.BlogId == blogId
                                           select foundItem;

//            dtoList = this.ApplyOrder(dtoList, "TimesViewed", "OrderByAscending");

            if (maxResults > 0)
            {
                retVal = dtoList.ToList<BlogPost>().Take(maxResults).ToList<BlogPost>();
            }
            else
            {
                retVal = dtoList.ToList<BlogPost>();
            }

            return retVal;
        }

        public BlogPost GetByTitle(string blogTitle, int blogId)
        {
            return this.GetByProperty("Title", blogTitle, blogId);
        }

        public BlogPost GetByDateAndTitle(string blogTitle, DateTime postDate, int blogId)
        {
            BlogPost retVal = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                                  where foundItem.Blog.BlogId == blogId && 
                                  foundItem.IsPublished == true && 
                                  foundItem.Title == blogTitle && 
                                  foundItem.DatePosted.Year == postDate.Year && 
                                  foundItem.DatePosted.Month == postDate.Month &&
                                  foundItem.DatePosted.Day == postDate.Day
                                  orderby foundItem.DatePosted descending select foundItem).First();

            return retVal;
        }

        public IList<BlogPost> GetByTag(int tagId, bool publishedOnly)
        {
            return this.GetByTag(null, tagId, publishedOnly);
        }

        public IList<BlogPost> GetByTag(int? blogId, int tagId, bool publishedOnly)
        {
            IQueryable<BlogPost> dtoList = null;

            if (blogId.HasValue)
            {
                if (publishedOnly == true)
                {
                    dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                              join entryTag in ((UnitOfWork)this.UnitOfWork).DataContext.PostTags on foundItem.EntryId equals entryTag.Post.EntryId
                              join tagItem in ((UnitOfWork)this.UnitOfWork).DataContext.Tags on entryTag.Tag.Id equals tagItem.Id
                              where tagItem.Blog.BlogId == blogId.Value &&
                              foundItem.IsPublished == true &&
                              tagItem.Id == tagId
                              orderby foundItem.DatePosted descending
                              select foundItem;
                }
                else
                {
                    dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                              join entryTag in ((UnitOfWork)this.UnitOfWork).DataContext.PostTags on foundItem.EntryId equals entryTag.Post.EntryId
                              join tagItem in ((UnitOfWork)this.UnitOfWork).DataContext.Tags on entryTag.Tag.Id equals tagItem.Id
                              where tagItem.Blog.BlogId == blogId.Value &&
                              tagItem.Id == tagId
                              orderby foundItem.DatePosted descending
                              select foundItem;
                }
            }
            else
            {
                if (publishedOnly == true)
                {
                    dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                              join entryTag in ((UnitOfWork)this.UnitOfWork).DataContext.PostTags on foundItem.EntryId equals entryTag.Post.EntryId
                              join tagItem in ((UnitOfWork)this.UnitOfWork).DataContext.Tags on entryTag.Tag.Id equals tagItem.Id
                              where foundItem.IsPublished == true &&
                              tagItem.Id == tagId
                              orderby foundItem.DatePosted descending
                              select foundItem;

                }
                else
                {
                    dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                              join entryTag in ((UnitOfWork)this.UnitOfWork).DataContext.PostTags on foundItem.EntryId equals entryTag.Post.EntryId
                              join tagItem in ((UnitOfWork)this.UnitOfWork).DataContext.Tags on entryTag.Tag.Id equals tagItem.Id
                              where tagItem.Id == tagId
                              orderby foundItem.DatePosted descending
                              select foundItem;
                }
            }

            return dtoList.ToList<BlogPost>();
        }

        public IList<BlogPost> GetByMonth(DateTime blogDate, bool publishedOnly)
        {
            return this.GetByMonth(blogDate, null, publishedOnly);
        }

        public IList<BlogPost> GetByMonth(DateTime blogDate, int? blogId, bool publishedOnly)
        {
            IQueryable<BlogPost> dtoList = null;
            
            if(blogId.HasValue)
            {
                if(publishedOnly==true)
                {
                    dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                              where foundItem.Blog.BlogId == blogId.Value && 
                              foundItem.IsPublished == true && 
                              foundItem.DatePosted.Month == blogDate.Month && 
                              foundItem.DatePosted.Year == blogDate.Year 
                              select foundItem;
                }
                else
                {
                    dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                              where foundItem.Blog.BlogId == blogId.Value &&
                              foundItem.DatePosted.Month == blogDate.Month &&
                              foundItem.DatePosted.Year == blogDate.Year
                              select foundItem;
                }
            }
            else
            {
                if(publishedOnly==true)
                {
                    dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                              where foundItem.IsPublished == true &&
                              foundItem.DatePosted.Month == blogDate.Month &&
                              foundItem.DatePosted.Year == blogDate.Year
                              select foundItem;
                }
                else
                {
                    dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                              where foundItem.DatePosted.Month == blogDate.Month &&
                              foundItem.DatePosted.Year == blogDate.Year
                              select foundItem;
                }
            }

            return dtoList.ToList<BlogPost>();
        }

        public IList<BlogPost> GetByDate(DateTime blogDate, bool publishedOnly)
        {
            return this.GetByDate(blogDate, null, publishedOnly);
        }

        public IList<BlogPost> GetByDate(DateTime blogDate, int? blogId, bool publishedOnly)
        {
            IQueryable<BlogPost> dtoList = null;

            if (blogId.HasValue)
            {
                if (publishedOnly == true)
                {
                    dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                              where foundItem.Blog.BlogId == blogId.Value &&
                              foundItem.IsPublished == true &&
                              foundItem.DatePosted.Date == blogDate.Date
                              select foundItem;
                }
                else
                {
                    dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                              where foundItem.Blog.BlogId == blogId.Value &&
                              foundItem.DatePosted.Date == blogDate.Date
                              select foundItem;
                }
            }
            else
            {
                if (publishedOnly == true)
                {
                    dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                              where foundItem.IsPublished == true &&
                              foundItem.DatePosted.Date == blogDate.Date
                              select foundItem;
                }
                else
                {
                    dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                              where foundItem.DatePosted.Month == blogDate.Month &&
                              foundItem.DatePosted.Date == blogDate.Date
                              select foundItem;
                }
            }

            return dtoList.ToList<BlogPost>();
        }

        public BlogPost GetMostRecent(int blogId, bool published)
        {
            BlogPost retVal = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts 
                          where foundItem.Blog.BlogId == blogId && foundItem.IsPublished == true 
                          orderby foundItem.DatePosted descending select foundItem).First();

            return retVal;
        }

        public BlogPost GetPreviousEntry(int blogId, int currentPostId)
        {
            BlogPost retVal = null;

            BlogPost currentPost = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts 
                                        where foundItem.Blog.BlogId == blogId && 
                                        foundItem.EntryId == currentPostId
                                        select foundItem).First();

            IQueryable<BlogPost> previousItems = from previousItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                                                    where previousItem.Blog.BlogId == blogId && 
                                                    previousItem.IsPublished == true && 
                                                    previousItem.DatePosted < currentPost.DatePosted 
                                                    orderby previousItem.DatePosted descending select previousItem;

            if (previousItems != null && previousItems.Count() > 0)
            {
                retVal = previousItems.First();
            }

            return retVal;
        }

        public BlogPost GetNextEntry(int blogId, int currentPostId)
        {
            BlogPost retVal = null;

            BlogPost currentPost = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                                        where foundItem.Blog.BlogId == blogId &&
                                        foundItem.EntryId == currentPostId
                                        select foundItem).First();

            IQueryable<BlogPost> followingItems = from previousItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                                                        where previousItem.Blog.BlogId == blogId &&
                                                        previousItem.IsPublished == true &&
                                                        previousItem.DatePosted > currentPost.DatePosted
                                                        orderby previousItem.DatePosted ascending
                                                        select previousItem;

            if (followingItems.Count() > 0)
            {
                retVal = followingItems.First();
            }

            return retVal;
        }

        public IList<DateTime> GetPublishedDatesByMonth(DateTime blogDate)
        {
            string queryString = "SELECT  DatePosted";
            queryString += " FROM BlogEntries";
            queryString += " WHERE (IsPublished = 1)";
            queryString += " AND (YEAR(DatePosted) = " + blogDate.Year + ")";
            queryString += " AND (MONTH(DatePosted ) = " + blogDate.Month + ")";
            queryString += " ORDER BY DatePosted";

            IList<DateTime> retVal = new List<DateTime>();

            IEnumerable<BlogPost> foundPosts = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                                                     where foundItem.IsPublished == true &&
                                                     foundItem.DatePosted.Year == blogDate.Year &&
                                                     foundItem.DatePosted.Month == blogDate.Month
                                                     orderby foundItem.DatePosted
                                                     select foundItem;


            foreach (BlogPost foundPost in foundPosts)
            {
                retVal.Add(foundPost.DatePosted);
            }

            return retVal;
//            DetachedCriteria criteria = DetachedCriteria.For<NHBlogPost>();
//            ProjectionList projections = Projections.ProjectionList();
//            criteria.SetProjection(Projections.Distinct(Projections.Alias(Projections.Property("DatePosted"), "DatePosted")));
//            criteria.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(NHBlogPost)));

//            IList<BlogPost> foundDates = Castle.ActiveRecord.ActiveRecordMediator<NHBlogPost>.FindAll(criteria);

//            IList<DateTime> retVal = new List<DateTime>();

//            for (int i = 0; i < foundDates.Count; i++)
//            {
//                retVal.Add(foundDates[i].DatePosted);
//            }

//            return retVal;
        }

        public IList GetArchiveDates(int? blogId)
        {
            string queryString = "SELECT  COUNT(*) AS PostCount, Max(DatePosted) AS MaxDate";
            queryString += " FROM BlogEntries";
            queryString += " WHERE (IsPublished = 1)";

            IQueryable<IEnumerable<BlogPost>> foundPosts = null;

            if (blogId.HasValue)
            {
                foundPosts = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                             where foundItem.Blog.BlogId == blogId.Value
                             group foundItem by new { foundItem.DatePosted.Year, foundItem.DatePosted.Month } into dateGroup
                             let maxDate = dateGroup.Max(x => x.DatePosted)
                             select dateGroup.Where(x => x.DatePosted == maxDate);
            }
            else
            {
                foundPosts = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                             group foundItem by new{ foundItem.DatePosted.Year, foundItem.DatePosted.Month } into dateGroup
                             let maxDate = dateGroup.Max(x => x.DatePosted)
                             select dateGroup.Where(x => x.DatePosted == maxDate);
            }

            IList retVal = new ArrayList();

            foreach (IEnumerable<BlogPost> foundPost in foundPosts)
            {
                BlogPostCount newItem = new BlogPostCount();

                newItem.PostCount = foundPost.Count();
                newItem.MaxDate = foundPost.ElementAt(0).DatePosted;

                retVal.Add(newItem);
            }

            return retVal;
        }

        public BlogPost GetByCommentId(int commentId)
        {
            BlogPost retVal = null;

            Comment targetComment = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.Comments
                                    where foundItem.CommentId == commentId
                                    select foundItem).First();

            if (targetComment != null)
            {
                retVal = targetComment.Post;
            }

            return retVal;
        }

        public IList<BlogPost> GetByTag(int blogId, String tagName, bool publishedOnly)
        {
            IList<BlogPost> retVal = new List<BlogPost>();

            Tag targetTag = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.Tags
                                     where foundItem.Blog.BlogId == blogId &&
                                     foundItem.Name==tagName
                                     select foundItem).First();

            if (targetTag != null)
            {
                if (publishedOnly == true)
                {
                    retVal = targetTag.BlogEntries.Where(tag => tag.IsPublished == true).ToList();
                }
                else
                {
                    retVal = targetTag.BlogEntries;
                }
            }

            return retVal;
        }

    }
}
