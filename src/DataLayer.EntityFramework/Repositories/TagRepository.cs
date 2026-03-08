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
    public class TagRepository : EntityFrameworkRepository<Tag, long>, ITagRepository
    {
        internal TagRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public override string IdPropertyName
        {
            get { return "Id"; }
        }

        public new IList<Tag> GetAll(long blogId)
        {
            return this.GetAllByProperty("BlogId", blogId);
        }

        public IList GetAllWithCount(long? blogId)
        {
            IDictionary<string, object> objectParams = new Dictionary<string, object>();

            string queryString = "SELECT COUNT(bet.Id) AS Count, t.name as TagName";
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

        public Tag GetByName(string name, long blogId)
        {
            return this.GetByProperty("Name", name, blogId);
        }

        public IList<Tag> GetByNames(string[] names, long blogId)
        {
            IQueryable<Tag> retVal = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.Tags
                                     where names.Contains(foundItem.Name) && foundItem.Blog.Id == blogId
                                     select foundItem;
            return retVal.ToList();
        }

        public IList<Tag> GetByBlogEntryId(long entryId)
        {
            IQueryable<Tag> retVal = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.Tags
                                     join blogEntryTag in ((UnitOfWork)this.UnitOfWork).DataContext.PostTags on foundItem.Id equals blogEntryTag.Tag.Id
                                     where blogEntryTag.Post.Id == entryId
                                     select foundItem;
            return retVal.ToList();
        }
    }
}
