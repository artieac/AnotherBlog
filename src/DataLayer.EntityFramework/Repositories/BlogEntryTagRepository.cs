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
using System.Data.Objects;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.DataLayer;
using AlwaysMoveForward.AnotherBlog.DataLayer.Entities;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class BlogEntryTagRepository : EntityFrameworkRepository<PostTag, PostTag>, IBlogEntryTagRepository
    {
        /// <summary>
        /// Contains all of data access code for working with BlogEntryTags (a table that associates tags to blog entries)
        /// </summary>
        /// <param name="dataContext"></param>
        internal BlogEntryTagRepository(IUnitOfWork unitOfWork, RepositoryManager repositoryManager)
            : base(unitOfWork, repositoryManager)
        {

        }

        public override string IdPropertyName
        {
            get { return "BlogEntryTagId"; }
        }

        public override string TableName
        {
            get{ return "BlogEntryTags";}
        }

        public override PostTag GetDTOByDomain(PostTag domainEntity)
        {
            PostTag retVal = null;

            IEnumerable<PostTag> dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.PostTags
                                             where foundItem.PostTagId == domainEntity.PostTagId
                                             select foundItem;

            if (dtoList != null)
            {
                retVal = dtoList.FirstOrDefault();
            }

            return retVal;
        }

        public override IList<PostTag> GetAllByProperty(string propertyName, object idValue)
        {
            String sql = "SELECT BlogEntryTagId as PostTagId, TagId, BlogEntryId FROM " + this.TableName;
            sql += " WHERE " + propertyName + " =@propertyValue";

            IDictionary<String, object> queryParams = new Dictionary<String, object>();
            queryParams.Add("propertyValue", idValue);

            IEnumerable<PostTag> dtoList = ((UnitOfWork)this.UnitOfWork).DataContext.ExecuteSQL<PostTag>(sql, queryParams);
            return dtoList.ToList<PostTag>();
        }

        /// <summary>
        /// Get all comments for a specific blog entry.
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public IList<PostTag> GetByBlogEntry(int blogPostId)
        {
            return this.GetAllByProperty("BlogEntryId", blogPostId);
        }

        public Boolean DeleteByBlogEntry(int blogPostId)
        {
            Boolean retVal = true;

            IList<PostTag> postTags = this.GetByBlogEntry(blogPostId);

            for (int i = 0; i < postTags.Count; i++)
            {
                this.Delete(postTags[i]);
            }

            return retVal;
        }

        /// <summary>
        /// Remove the blog entry
        /// </summary>
        /// <param name="saveItem"></param>
        //public override bool Delete(PostTag itemToDelete)
        //{
        //    bool retVal = false;

        //    if (itemToDelete != null)
        //    {
        //        ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<PostTag>().Remove(itemToDelete);
        //        ((UnitOfWork)this.UnitOfWork).DataContext.SaveChanges();
        //        retVal = true;
        //    }

        //    return retVal;
        //}

    }
}
