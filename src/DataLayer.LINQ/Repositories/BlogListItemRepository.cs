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
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer.Entities;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class BlogListItemRepository : LINQRepository<BlogListItem, BlogListItemDTO>, IBlogListItemRepository
    {
        internal BlogListItemRepository(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
            : base(unitOfWork, repositoryManager)
        {

        }

        public override string IdPropertyName
        {
            get { return "Id"; }
        }

        protected override BlogListItemDTO GetDTOByDomain(BlogListItem targetItem)
        {
            return this.GetDtoById(targetItem.Id);
        }

        public IList<BlogListItem> GetByBlogList(int blogListId)
        {
            IQueryable<BlogListItemDTO> dtoList = null;

            dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogListItemDTOs
                      where foundItem.BlogListId == blogListId
                      select foundItem;

            return this.Map(dtoList.ToList<BlogListItemDTO>());
        }

        public override BlogListItem Save(BlogListItem itemToSave)
        {
            BlogListItemDTO targetItem = this.GetDTOByDomain(itemToSave);

            if (targetItem == null)
            {
                targetItem = this.Map(itemToSave);
                ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<BlogListItemDTO>().InsertOnSubmit(targetItem);
            }
            else
            {
                targetItem.DisplayOrder = itemToSave.DisplayOrder;
                targetItem.Name = itemToSave.Name;
                targetItem.RelatedLink = itemToSave.RelatedLink;
            }

            this.UnitOfWork.Flush();

            return this.Map(targetItem);
        }
    }
}
