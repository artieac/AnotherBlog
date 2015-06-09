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
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer.Entities;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    /// <summary>
    /// This class contains all the code to extract Tag data from the repository using LINQ
    /// </summary>
    /// <param name="dataContext"></param>
    public class TagRepository : LINQRepository<Tag, TagDTO>, ITagRepository
    {
        internal TagRepository(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
            : base(unitOfWork, repositoryManager)
        {

        }

        public Tag Create()
        {
            return new Tag();
        }

        protected override TagDTO GetDTOByDomain(Tag targetItem)
        {
            return this.GetDtoById(targetItem.Id);
        }
        /// <summary>
        /// Get all tags related to a specific blog
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public IList GetAllWithCount(int? blogId)
        {
            string queryString = "SELECT  COUNT(bet.BlogEntryTagId) AS Count, t.name as TagName";
            queryString += " FROM Tags t, BlogEntryTags as bet";
            queryString += " WHERE (bet.TagId = t.id)";

            if (blogId.HasValue)
            {
                queryString += " AND (t.BlogId = {0})";
            }

            queryString += " GROUP BY t.Name";

            IEnumerable<TagCount> foundTags;

            if (blogId.HasValue)
            {
                foundTags = ((UnitOfWork)this.UnitOfWork).DataContext.ExecuteQuery<TagCount>(queryString, blogId.Value);
            }
            else
            {
                foundTags = ((UnitOfWork)this.UnitOfWork).DataContext.ExecuteQuery<TagCount>(queryString, blogId.Value);
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
            IQueryable<TagDTO> dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.TagDTOs
                                     where names.Contains(foundItem.Name) && foundItem.BlogId == blogId
                                     select foundItem;
            return this.Map(dtoList.ToList<TagDTO>());
        }

        public IList<Tag> GetByBlogEntryId(int entryId)
        {
            IQueryable<TagDTO> dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.TagDTOs
                                         join blogEntryTag in ((UnitOfWork)this.UnitOfWork).DataContext.BlogEntryTagDTOs on foundItem.Id equals blogEntryTag.TagId
                                         where blogEntryTag.PostId == entryId
                                         select foundItem;
            return this.Map(dtoList.ToList<TagDTO>());
        }

        public override Tag Save(Tag itemToSave)
        {
            TagDTO targetItem = this.GetDTOByDomain(itemToSave);

            if (targetItem == null)
            {
                targetItem = new TagDTO();
                targetItem.Name = itemToSave.Name;
                targetItem.BlogId = itemToSave.Blog.BlogId;
                ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<TagDTO>().InsertOnSubmit(targetItem);
            }
            else
            {
                targetItem = this.Map(itemToSave);
                targetItem.BlogId = itemToSave.Blog.BlogId;
            }

            this.UnitOfWork.Flush();

            return this.Map(targetItem);
        }
    }
}
