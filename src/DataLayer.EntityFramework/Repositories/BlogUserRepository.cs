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
    public class BlogUserRepository : EntityFrameworkRepository<BlogUser, long>
    {
        internal BlogUserRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public override string IdPropertyName
        {
            get { return "Id"; }
        }

        public IList<BlogUser> GetUserBlogs(int userId)
        {
            IQueryable<BlogUser> foundItems = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogUsers
                                              where foundItem.User.Id == userId
                                              select foundItem;

            return foundItems.ToList();
        }

        public BlogUser GetUserBlog(int userId, int blogId)
        {
            BlogUser retVal = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogUsers
                               where foundItem.User.Id == userId &&
                               foundItem.Blog.Id == blogId
                               select foundItem).SingleOrDefault();
            return retVal;
        }

        private BlogUser GetUserBlogDTO(int userId, int blogId)
        {
            BlogUser retVal = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogUsers
                               where foundItem.User.Id == userId &&
                               foundItem.Blog.Id == blogId
                               select foundItem).SingleOrDefault();
            return retVal;
        }

        public bool DeleteUserBlog(int userId, int blogId)
        {
            bool retVal = false;

            BlogUser targetUserBlog = this.GetUserBlogDTO(userId, blogId);

            if (targetUserBlog != null)
            {
                this.Delete(targetUserBlog);
                retVal = true;
            }

            return retVal;
        }
    }
}
