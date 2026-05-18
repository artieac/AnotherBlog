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
using Microsoft.EntityFrameworkCore;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class BlogEntryRepository : EntityFrameworkRepository<BlogPost, long>, IBlogEntryRepository
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

        public override BlogPost GetById(long id)
        {
            return this.GetDbSet()
                .FirstOrDefault(p => p.Id == id);
        }

        public override BlogPost Save(BlogPost itemToSave)
        {
            if (itemToSave != null)
            {
                var dataContext = ((UnitOfWork)this.UnitOfWork).DataContext;

                if (itemToSave.Blog != null && itemToSave.Blog.Id > 0)
                {
                    var trackedBlog = dataContext.Blogs.Find(itemToSave.Blog.Id);
                    if (trackedBlog != null)
                    {
                        itemToSave.Blog = trackedBlog;
                    }
                }

                if (itemToSave.Author != null && itemToSave.Author.Id > 0)
                {
                    var trackedUser = dataContext.Users.Find(itemToSave.Author.Id);
                    if (trackedUser != null)
                    {
                        itemToSave.Author = trackedUser;
                    }
                }
            }

            return base.Save(itemToSave);
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

        public IList<BlogPost> GetAllByBlog(long blogId, bool publishedOnly, int maxResults, string sortColumn, bool sortAscending)
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

        public IList<BlogPost> GetMostRead(long blogId, int maxResults)
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

        public BlogPost GetByTitle(string blogTitle, long blogId)
        {
            return this.GetByProperty("Title", blogTitle, blogId);
        }

        public BlogPost GetByDateAndTitle(string blogTitle, DateTime postDate, long blogId)
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

        public IList<BlogPost> GetByMonth(DateTime blogDate, bool publishedOnly)
        {
            return this.GetByMonth(blogDate, null, publishedOnly);
        }

        public IList<BlogPost> GetByMonth(DateTime blogDate, long? blogId, bool publishedOnly)
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

        public IList<BlogPost> GetByDate(DateTime blogDate, long? blogId, bool publishedOnly)
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

        public BlogPost GetMostRecent(long blogId, bool published)
        {
            BlogPost retVal = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts
                               where foundItem.Blog.Id == blogId && foundItem.IsPublished == true
                               orderby foundItem.DatePosted descending
                               select foundItem).FirstOrDefault();

            return retVal;
        }

        public BlogPost GetPreviousEntry(long blogId, long currentPostId)
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

        public BlogPost GetNextEntry(long blogId, long currentPostId)
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

        public IList GetArchiveDates(long? blogId)
        {
            IQueryable<BlogPost> query = ((UnitOfWork)this.UnitOfWork).DataContext.BlogPosts;

            if (blogId.HasValue)
            {
                query = query.Where(foundItem => foundItem.Blog.Id == blogId.Value);
            }

            var groupedPosts = query
                .GroupBy(foundItem => new { foundItem.DatePosted.Year, foundItem.DatePosted.Month })
                .Select(dateGroup => new
                {
                    Year = dateGroup.Key.Year,
                    Month = dateGroup.Key.Month,
                    PostCount = dateGroup.Count(),
                    MaxDate = dateGroup.Max(x => x.DatePosted)
                })
                .ToList();

            IList retVal = new ArrayList();

            foreach (var groupItem in groupedPosts)
            {
                BlogPostCount newItem = new BlogPostCount();
                newItem.PostCount = groupItem.PostCount;
                newItem.MaxDate = groupItem.MaxDate;
                retVal.Add(newItem);
            }

            return retVal;
        }

        public BlogPost GetByCommentId(long commentId)
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
    }
}
