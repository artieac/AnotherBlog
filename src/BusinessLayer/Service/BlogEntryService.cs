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
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.Business;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;

namespace AlwaysMoveForward.AnotherBlog.BusinessLayer.Service
{
    /// <summary>
    /// This class controlls the business rules realted to a blog entry, and controls the data access to the 
    /// repository classes to help support those roles.
    /// </summary>
    public class BlogEntryService : AnotherBlogService
    {
        public const string DefaultPostSort = "DatePosted";

        public BlogEntryService(IUnitOfWork unitOfWork, IBlogEntryRepository blogEntryRepository, ITagRepository tagRepository) : base(unitOfWork) 
        {
            this.BlogEntryRepository = blogEntryRepository;
            this.TagRepository = tagRepository;
        }

        protected IBlogEntryRepository BlogEntryRepository { get; private set; }

        protected ITagRepository TagRepository { get; private set; }

        /// <summary>
        /// Instantiate and initialize a BlogEntry instance.
        /// </summary>
        /// <param name="targetBlog"></param>
        /// <returns></returns>
        public BlogPost Create(Blog targetBlog)
        {
            BlogPost retVal = new BlogPost();
            retVal.DateCreated = DateTime.Now;
            retVal.Author = ((SecurityPrincipal)System.Threading.Thread.CurrentPrincipal).CurrentUser;
            retVal.Blog = targetBlog;
            
            return retVal;
        }

        /// <summary>
        /// Save a blog entry to the database.
        /// </summary>
        /// <param name="targetBlog"></param>
        /// <param name="title"></param>
        /// <param name="entryText"></param>
        /// <param name="entryId"></param>
        /// <param name="isPublished"></param>
        /// <param name="_submitChanges"></param>
        /// <returns></returns>
        public BlogPost Save(Blog targetBlog, string title, string entryText, int entryId, bool isPublished, string[] tagNames)
        {
            BlogPost itemToSave = null;
            DateTime startDate = new DateTime(2009, 1, 1);

            if (entryId <= 0)
            {
                itemToSave = this.Create(targetBlog);
                itemToSave.SetPublishState(isPublished);
            }
            else
            {
                itemToSave = this.BlogEntryRepository.GetById(entryId);
            }

            itemToSave.Title = title;
            itemToSave.EntryText = entryText;
            itemToSave.Blog = targetBlog;
            itemToSave.Author = ((SecurityPrincipal)System.Threading.Thread.CurrentPrincipal).CurrentUser;
            itemToSave.SetPublishState(isPublished);
            itemToSave = this.AddTags(itemToSave, tagNames);

            return this.Save(itemToSave);
        }

        /// <summary>
        /// Save a blog entry to the database.
        /// </summary>
        /// <param name="targetBlog"></param>
        /// <param name="title"></param>
        /// <param name="entryText"></param>
        /// <param name="entryId"></param>
        /// <param name="isPublished"></param>
        /// <param name="_submitChanges"></param>
        /// <returns></returns>
        public BlogPost Save(BlogPost blogPost)
        {
            if(blogPost != null)
            {
                blogPost = this.BlogEntryRepository.Save(blogPost);
            }

            return blogPost;
        }

        private BlogPost AddTags(BlogPost targetPost, string[] names)
        {
            if (targetPost.Tags == null)
            {
                targetPost.Tags = new List<Tag>();
            }
            else
            {
                targetPost.Tags.Clear();
            }

            for (int i = 0; i < names.Length; i++)
            {
                string trimmedName = names[i].Trim();

                if (trimmedName != string.Empty)
                {
                    Tag currentTag = this.TagRepository.GetByName(trimmedName, targetPost.Blog.Id);

                    if (currentTag == null)
                    {
                        currentTag = new Tag();
                        currentTag.Name = trimmedName;
                        currentTag.BlogId = targetPost.Blog.Id;
                    }

                    targetPost.Tags.Add(currentTag);
                }
            }

            return targetPost;
        }
        
        /// <summary>
        /// Get all blog entries in the system.
        /// </summary>
        /// <returns></returns>
        public IList<BlogPost> GetAll()
        {
            return this.BlogEntryRepository.GetAll();
        }
        /// <summary>
        /// Get all blog entries related to a single blog
        /// </summary>
        /// <param name="targetBlog"></param>
        /// <returns></returns>
        public IList<BlogPost> GetAllByBlogId(Blog targetBlog, int currentPageIndex)
		{
            IList<BlogPost> retVal = new List<BlogPost>();

            if (targetBlog != null)
            {
                this.BlogEntryRepository.GetAllByBlog(targetBlog.Id, false, -1, DefaultPostSort, true);
            }

            return retVal;
        }
        /// <summary>
        /// Another vrsion of get all by blog id that just takes the blog id itself rather than a blog object.
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public IList<BlogPost> GetAllByBlog(Blog targetBlog, bool publishedOnly)
        {
            return this.GetAllByBlog(targetBlog, publishedOnly, -1);
        }

        public IList<BlogPost> GetAllByBlog(Blog targetBlog, bool publishedOnly, int maxResults)
        {
            return this.GetAllByBlog(targetBlog, publishedOnly, maxResults, DefaultPostSort, false);
        }

        public IList<BlogPost> GetAllByBlog(Blog targetBlog, bool publishedOnly, int maxResults, string sortColumn, bool sortAscending)
        {
            IList<BlogPost> retVal = new List<BlogPost>();

            if (targetBlog != null)
            {
                retVal = this.BlogEntryRepository.GetAllByBlog(targetBlog.Id, publishedOnly, maxResults, sortColumn, sortAscending);
            }

            return retVal;
        }
        /// <summary>
        /// Get a particular blog entry in a particular blog
        /// </summary>
        /// <param name="targetBlog"></param>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public BlogPost GetById(Blog targetBlog, int entryId)
        {
            BlogPost retVal = null;

            if (targetBlog != null)
            {
                retVal = this.BlogEntryRepository.GetById(entryId);
            }

            return retVal;
        }
        /// <summary>
        /// Get a particular blog entry in a particular blog
        /// </summary>
        /// <param name="targetBlog"></param>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public BlogPost GetByTitle(Blog targetBlog, string blogTitle)
        {
            BlogPost retVal = null;

            if (targetBlog != null)
            {
                retVal = this.BlogEntryRepository.GetByTitle(blogTitle, targetBlog.Id);
            }

            return retVal;
        }
        /// <summary>
        /// Get a particular blog entry in a particular blog
        /// </summary>
        /// <param name="targetBlog"></param>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public BlogPost GetByDateAndTitle(Blog targetBlog, DateTime postDate, string blogTitle)
        {
            BlogPost retVal = null;

            if (targetBlog != null)
            {
                retVal = this.BlogEntryRepository.GetByDateAndTitle(blogTitle, postDate, targetBlog.Id);
            }

            return retVal;
        }

        /// <summary>
        /// Get all blog entries that have a particular tag associated with them whether they are published or not.
        /// </summary>
        /// <param name="targetBlog"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public IList<BlogPost> GetByTag(Blog targetBlog, string tag, bool publishedOnly)
        {
            IList<BlogPost> retVal = new List<BlogPost>();

            if (targetBlog != null)
            {
                retVal = this.BlogEntryRepository.GetByTag(targetBlog.Id, tag, publishedOnly);
            }
            return retVal;
        }

        public IList<BlogPost> GetByMonth(DateTime blogDate, bool publishedOnly)
        {
            return this.BlogEntryRepository.GetByMonth(blogDate, publishedOnly);
        }

        public IList<BlogPost> GetByMonth(Blog targetBlog, DateTime blogDate, bool publishedOnly)
        {
            IList<BlogPost> retVal = new List<BlogPost>();

            if (targetBlog != null)
            {
                retVal = this.BlogEntryRepository.GetByMonth(blogDate, targetBlog.Id, publishedOnly);
            }
            return retVal;
        }

        public IList<BlogPost> GetByDate(DateTime blogDate, bool publishedOnly)
        {
            return this.BlogEntryRepository.GetByDate(blogDate, publishedOnly);
        }

        public IList<BlogPost> GetByDate(Blog targetBlog, DateTime blogDate, bool publishedOnly)
        {
            IList<BlogPost> retVal = new PagedList<BlogPost>();

            if (targetBlog != null)
            {
                retVal = this.BlogEntryRepository.GetByDate(blogDate, targetBlog.Id, publishedOnly);
            }
            return retVal;
        }

        public BlogPost GetMostRecent(Blog targetBlog)
        {
            BlogPost retVal = null;

            if (targetBlog != null)
            {
                retVal = this.BlogEntryRepository.GetMostRecent(targetBlog.Id, true);
            }

            return retVal;
        }

        public IList<BlogPost> GetMostRecent(int maxResults)
        {
            return this.BlogEntryRepository.GetAll(true, maxResults);
        }

        public IList<BlogPost> GetMostRead(int maxResults)
        {
            return this.BlogEntryRepository.GetMostRead(maxResults);
        }

        public IList<BlogPost> GetMostRead(int blogId, int maxResults)
        {
            return this.BlogEntryRepository.GetMostRead(blogId, maxResults);
        }

        public BlogPost GetPreviousEntry(Blog targetBlog, BlogPost currentEntry)
        {
            BlogPost retVal = null;

            if (targetBlog != null)
            {
                retVal = this.BlogEntryRepository.GetPreviousEntry(targetBlog.Id, currentEntry.Id);
            }

            return retVal;
        }

        public BlogPost GetNextEntry(Blog targetBlog, BlogPost currentEntry)
        {
            BlogPost retVal = null;

            if (targetBlog != null)
            {
                retVal = this.BlogEntryRepository.GetNextEntry(targetBlog.Id, currentEntry.Id);
            }

            return retVal;
        }

        public IList<DateTime> GetPublishedDatesByMonth(DateTime blogDate)
        {
            return this.BlogEntryRepository.GetPublishedDatesByMonth(blogDate);
        }

        public IList GetArchiveDates(Blog targetBlog)
        {
            if (targetBlog != null)
            {
                return this.BlogEntryRepository.GetArchiveDates(targetBlog.Id);
            }
            else
            {
                return this.BlogEntryRepository.GetArchiveDates(null);
            }
        }

        public bool Delete(BlogPost targetEntry)
        {
            return this.BlogEntryRepository.Delete(targetEntry);
        }

        public int UpdateTimesViewed(BlogPost targetPost)
        {
            int retVal = 0;
            
            if (targetPost != null)
            {
                targetPost.TimesViewed++;
                this.BlogEntryRepository.Save(targetPost);
                retVal = targetPost.TimesViewed;
            }

            return retVal;
        }
    }
}
