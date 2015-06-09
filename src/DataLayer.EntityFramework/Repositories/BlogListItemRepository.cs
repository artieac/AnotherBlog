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
    public class BlogListItemRepository : EntityFrameworkRepository<BlogListItem, BlogListItem>, IBlogListItemRepository
    {
        internal BlogListItemRepository(IUnitOfWork unitOfWork, RepositoryManager repositoryManager)
            : base(unitOfWork, repositoryManager)
        {

        }

        public override string IdPropertyName
        {
            get { return "Id"; }
        }

        public IList<BlogListItem> GetByBlogList(int blogListId)
        {
            IQueryable<BlogListItem> retVal = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogListItems
                                      where foundItem.BlogList.Id == blogListId
                                      select foundItem;
            return retVal.ToList<BlogListItem>();
        }
    }
}
