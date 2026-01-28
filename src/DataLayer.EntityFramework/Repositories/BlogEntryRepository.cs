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

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class BlogEntryRepository : EntityFrameworkRepository<BlogPost, int>, IBlogEntryRepository
    {
        internal BlogEntryRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public override string IdPropertyName
        {
            get { return "Id"; }
        }

        public override string TableName
        {
            get { return "BlogEntries"; }
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
                return dtoList.Take(maxResults).ToList();
            }

            return dtoList.ToList();
        }

        public IList<BlogPost> GetAllByBlog(int blogId, bool publishedOnly, int maxResults, string sortColumn, bool sortAscending)
        {
            IQueryable<BlogPost> dtoList = null;

            if (publishedOnly == true)
            {
                dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                          where foundItem.IsPublished == true &&
                          foundItem.Blog.Id == blogId
                          select foundItem;
            }
            else
            {
                dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                          where foundItem.Blog.Id == blogId
                          select foundItem;
            }

            if (sortAscending == true)
            {
                dtoList = this.ApplyOrder(dtoList, sortColumn, "OrderBy");
            }
            else
            {
                dtoList = this.ApplyOrder(dtoList, sortColumn, "OrderByDescending");
            }

            if (maxResults > 0)
            {
                return dtoList.Take(maxResults).ToList();
            }

            return dtoList.ToList();
        }

        public IList<BlogPost> GetMostRead(int maxResults)
        {
            IQueryable<BlogPost> dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                                           where foundItem.IsPublished == true
                                           orderby foundItem.TimesViewed descending
                                           select foundItem;

            if (maxResults > 0)
            {
                return dtoList.Take(maxResults).ToList();
            }

            return dtoList.ToList();
        }

        public IList<BlogPost> GetMostRead(int blogId, int maxResults)
        {
            IQueryable<BlogPost> dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                                           where foundItem.IsPublished == true &&
                                           foundItem.Blog.Id == blogId
                                           orderby foundItem.TimesViewed descending
                                           select foundItem;

            if (maxResults > 0)
            {
                return dtoList.Take(maxResults).ToList();
            }

            return dtoList.ToList();
        }

        public BlogPost GetByTitle(string blogTitle, int blogId)
        {
            return this.GetByProperty("Title", blogTitle, blogId);
        }

        public BlogPost GetByDateAndTitle(string blogTitle, DateTime postDate, int blogId)
        {
            BlogPost retVal = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                               where foundItem.Blog.Id == blogId &&
                               foundItem.IsPublished == true &&
                               foundItem.Title == blogTitle &&
                               foundItem.DatePosted.Year == postDate.Year &&
                               foundItem.DatePosted.Month == postDate.Month &&
                               foundItem.DatePosted.Day == postDate.Day
                               orderby foundItem.DatePosted descending
                               select foundItem).FirstOrDefault();

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
                              join entryTag in ((UnitOfWork)this.UnitOfWork).DataContext.PostTags on foundItem.Id equals entryTag.Post.Id
                              join tagItem in ((UnitOfWork)this.UnitOfWork).DataContext.Tags on entryTag.Tag.Id equals tagItem.Id
                              where tagItem.Blog.Id == blogId.Value &&
                              foundItem.IsPublished == true &&
                              tagItem.Id == tagId
                              orderby foundItem.DatePosted descending
                              select foundItem;
                }
                else
                {
                    dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                              join entryTag in ((UnitOfWork)this.UnitOfWork).DataContext.PostTags on foundItem.Id equals entryTag.Post.Id
                              join tagItem in ((UnitOfWork)this.UnitOfWork).DataContext.Tags on entryTag.Tag.Id equals tagItem.Id
                              where tagItem.Blog.Id == blogId.Value &&
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
                              join entryTag in ((UnitOfWork)this.UnitOfWork).DataContext.PostTags on foundItem.Id equals entryTag.Post.Id
                              join tagItem in ((UnitOfWork)this.UnitOfWork).DataContext.Tags on entryTag.Tag.Id equals tagItem.Id
                              where foundItem.IsPublished == true &&
                              tagItem.Id == tagId
                              orderby foundItem.DatePosted descending
                              select foundItem;
                }
                else
                {
                    dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                              join entryTag in ((UnitOfWork)this.UnitOfWork).DataContext.PostTags on foundItem.Id equals entryTag.Post.Id
                              join tagItem in ((UnitOfWork)this.UnitOfWork).DataContext.Tags on entryTag.Tag.Id equals tagItem.Id
                              where tagItem.Id == tagId
                              orderby foundItem.DatePosted descending
                              select foundItem;
                }
            }

            return dtoList.ToList();
        }

        public IList<BlogPost> GetByMonth(DateTime blogDate, bool publishedOnly)
        {
            return this.GetByMonth(blogDate, null, publishedOnly);
        }

        public IList<BlogPost> GetByMonth(DateTime blogDate, int? blogId, bool publishedOnly)
        {
            IQueryable<BlogPost> dtoList = null;

            if (blogId.HasValue)
            {
                if (publishedOnly == true)
                {
                    dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                              where foundItem.Blog.Id == blogId.Value &&
                              foundItem.IsPublished == true &&
                              foundItem.DatePosted.Month == blogDate.Month &&
                              foundItem.DatePosted.Year == blogDate.Year
                              select foundItem;
                }
                else
                {
                    dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                              where foundItem.Blog.Id == blogId.Value &&
                              foundItem.DatePosted.Month == blogDate.Month &&
                              foundItem.DatePosted.Year == blogDate.Year
                              select foundItem;
                }
            }
            else
            {
                if (publishedOnly == true)
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

            return dtoList.ToList();
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
                              where foundItem.Blog.Id == blogId.Value &&
                              foundItem.IsPublished == true &&
                              foundItem.DatePosted.Date == blogDate.Date
                              select foundItem;
                }
                else
                {
                    dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                              where foundItem.Blog.Id == blogId.Value &&
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
                              where foundItem.DatePosted.Date == blogDate.Date
                              select foundItem;
                }
            }

            return dtoList.ToList();
        }

        public BlogPost GetMostRecent(int blogId, bool published)
        {
            BlogPost retVal = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                               where foundItem.Blog.Id == blogId && foundItem.IsPublished == true
                               orderby foundItem.DatePosted descending
                               select foundItem).FirstOrDefault();

            return retVal;
        }

        public BlogPost GetPreviousEntry(int blogId, int currentPostId)
        {
            BlogPost retVal = null;

            BlogPost currentPost = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                                    where foundItem.Blog.Id == blogId &&
                                    foundItem.Id == currentPostId
                                    select foundItem).FirstOrDefault();

            if (currentPost != null)
            {
                retVal = (from previousItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                          where previousItem.Blog.Id == blogId &&
                          previousItem.IsPublished == true &&
                          previousItem.DatePosted < currentPost.DatePosted
                          orderby previousItem.DatePosted descending
                          select previousItem).FirstOrDefault();
            }

            return retVal;
        }

        public BlogPost GetNextEntry(int blogId, int currentPostId)
        {
            BlogPost retVal = null;

            BlogPost currentPost = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                                    where foundItem.Blog.Id == blogId &&
                                    foundItem.Id == currentPostId
                                    select foundItem).FirstOrDefault();

            if (currentPost != null)
            {
                retVal = (from followingItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                          where followingItem.Blog.Id == blogId &&
                          followingItem.IsPublished == true &&
                          followingItem.DatePosted > currentPost.DatePosted
                          orderby followingItem.DatePosted ascending
                          select followingItem).FirstOrDefault();
            }

            return retVal;
        }

        public IList<DateTime> GetPublishedDatesByMonth(DateTime blogDate)
        {
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
        }

        public IList GetArchiveDates(int? blogId)
        {
            IQueryable<IEnumerable<BlogPost>> foundPosts = null;

            if (blogId.HasValue)
            {
                foundPosts = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                             where foundItem.Blog.Id == blogId.Value
                             group foundItem by new { foundItem.DatePosted.Year, foundItem.DatePosted.Month } into dateGroup
                             let maxDate = dateGroup.Max(x => x.DatePosted)
                             select dateGroup.Where(x => x.DatePosted == maxDate);
            }
            else
            {
                foundPosts = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                             group foundItem by new { foundItem.DatePosted.Year, foundItem.DatePosted.Month } into dateGroup
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
                                     where foundItem.Id == commentId
                                     select foundItem).FirstOrDefault();

            if (targetComment != null)
            {
                retVal = targetComment.Post;
            }

            return retVal;
        }

        public IList<BlogPost> GetByTag(int blogId, string tagName, bool publishedOnly)
        {
            IList<BlogPost> retVal = new List<BlogPost>();

            Tag targetTag = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.Tags
                             where foundItem.Blog.Id == blogId &&
                             foundItem.Name == tagName
                             select foundItem).FirstOrDefault();

            if (targetTag != null)
            {
                if (publishedOnly == true)
                {
                    retVal = targetTag.BlogEntries.Where(post => post.IsPublished == true).ToList();
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
