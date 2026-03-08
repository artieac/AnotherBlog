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

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class BlogListItemRepository : EntityFrameworkRepository<BlogListItem, long>
    {
        internal BlogListItemRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
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
            return retVal.ToList();
        }
    }
}
