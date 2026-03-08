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

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.DataLayer.MappingDomainObjects;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class BlogEntryTagRepository : EntityFrameworkRepository<PostTag, long>
    {
        internal BlogEntryTagRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public override string IdPropertyName
        {
            get { return "Id"; }
        }

        public override string TableName
        {
            get { return "BlogEntryTags"; }
        }

        public IList<PostTag> GetByBlogEntry(int blogPostId)
        {
            IQueryable<PostTag> retVal = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.PostTags
                                         where foundItem.Post.Id == blogPostId
                                         select foundItem;
            return retVal.ToList();
        }

        public bool DeleteByBlogEntry(int blogPostId)
        {
            bool retVal = true;

            IList<PostTag> postTags = this.GetByBlogEntry(blogPostId);

            for (int i = 0; i < postTags.Count; i++)
            {
                this.Delete(postTags[i]);
            }

            return retVal;
        }
    }
}
