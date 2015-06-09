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
using System.Data.Objects;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.DataLayer;
using AlwaysMoveForward.AnotherBlog.DataLayer.Entities;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    /// <summary>
    /// This class contains all the code to extract Tag data from the repository using LINQ
    /// </summary>
    /// <param name="dataContext"></param>
    public class TagRepository : EntityFrameworkRepository<Tag, Tag>, ITagRepository
    {
        internal TagRepository(IUnitOfWork unitOfWork, RepositoryManager repositoryManager)
            : base(unitOfWork, repositoryManager)
        {

        }
        /// <summary>
        /// Get all tags related to a specific blog
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public IList GetAllWithCount(int? blogId)
        {
            IDictionary<String, object> objectParams = new Dictionary<String, object>();

            string queryString = "SELECT  COUNT(bet.BlogEntryTagId) AS Count, t.name as TagName";
            queryString += " FROM Tags t, BlogEntryTags as bet";
            queryString += " WHERE (bet.TagId = t.id)";

            if (blogId.HasValue)
            {
                queryString += " AND (t.BlogId = @blogId)";
                objectParams.Add("blogId", blogId.Value);
            }

            queryString += " GROUP BY t.Name";

            IEnumerable<TagCount> foundTags;

            if (blogId.HasValue)
            {
                foundTags = ((UnitOfWork)this.UnitOfWork).DataContext.ExecuteSQL<TagCount>(queryString, objectParams);
            }
            else
            {
                foundTags = ((UnitOfWork)this.UnitOfWork).DataContext.ExecuteSQL<TagCount>(queryString);
            }

            return foundTags.ToList();
        }
        /// <summary>
        /// Get a specific tag.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public Tag GetByName(string name, int blogId)
        {
            return this.GetByProperty("Name", name, blogId);
        }
        /// <summary>
        /// Get multiple tag records.
        /// </summary>
        /// <param name="names"></param>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public IList<Tag> GetByNames(string[] names, int blogId)
        {
            IQueryable<Tag> retVal = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.Tags
                                         where names.Contains(foundItem.Name) && foundItem.Blog.BlogId == blogId
                                         select foundItem;
            return retVal.ToList();
        }

        public IList<Tag> GetByBlogEntryId(int entryId)
        {
            IQueryable<Tag> retVal = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.Tags
                                         join blogEntryTag in ((UnitOfWork)this.UnitOfWork).DataContext.PostTags on foundItem.Id equals blogEntryTag.Tag.Id
                                         where blogEntryTag.Post.EntryId == entryId
                                         select foundItem;
            return retVal.ToList();
        }
    }
}
