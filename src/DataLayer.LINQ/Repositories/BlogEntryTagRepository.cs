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

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer.Entities;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class BlogEntryTagRepository : LINQRepository<PostTag, BlogEntryTagDTO>, IBlogEntryTagRepository
    {
        /// <summary>
        /// Contains all of data access code for working with BlogEntryTags (a table that associates tags to blog entries)
        /// </summary>
        /// <param name="dataContext"></param>
        internal BlogEntryTagRepository(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
            : base(unitOfWork, repositoryManager)
        {

        }

        public override string IdPropertyName
        {
            get { return "BlogEntryTagId"; }
        }

        protected override BlogEntryTagDTO GetDTOByDomain(PostTag targetItem)
        {
            return this.GetDtoById(targetItem.PostTagId);
        }

        /// <summary>
        /// Get all comments for a specific blog entry.
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public IList<PostTag> GetByBlogEntry(int blogPostId)
        {
            return this.GetAllByProperty("PostId", blogPostId);
        }

        public Boolean DeleteByBlogEntry(int blogPostId)
        {
            Boolean retVal = false;

            try
            {
                IQueryable<BlogEntryTagDTO> postTags = from postTag in ((UnitOfWork)this.UnitOfWork).DataContext.BlogEntryTagDTOs
                                                       where postTag.PostId == blogPostId
                                                       select postTag;

                if(postTags!=null)
                {
                    ((UnitOfWork)this.UnitOfWork).DataContext.BlogEntryTagDTOs.DeleteAllOnSubmit(postTags);
                }
                retVal = true;
            }
            catch (Exception e)
            {

            }

            return retVal;
        }

        public override PostTag Save(PostTag itemToSave)
        {
            BlogEntryTagDTO targetItem = this.GetDTOByDomain(itemToSave);

            if (targetItem == null)
            {
                targetItem = new BlogEntryTagDTO();
                targetItem.PostId = itemToSave.Post.EntryId;
                targetItem.TagId = itemToSave.Tag.Id;
                ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<BlogEntryTagDTO>().InsertOnSubmit(targetItem);
            }
            else
            {
                targetItem.PostId = itemToSave.Post.EntryId;
                targetItem.TagId = itemToSave.Tag.Id;
            }

            this.UnitOfWork.Flush();

            return this.Map(targetItem);
        }


    }
}
