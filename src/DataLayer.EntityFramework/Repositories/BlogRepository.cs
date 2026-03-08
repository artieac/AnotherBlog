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
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class BlogRepository : EntityFrameworkRepository<Blog, int>, IBlogRepository
    {
        internal BlogRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public override string IdPropertyName
        {
            get { return "Id"; }
        }

        public Blog GetByName(string name)
        {
            return this.GetByProperty("Name", name);
        }

        public Blog GetBySubFolder(string subFolder)
        {
            Blog retVal = null;

            if (subFolder != null)
            {
                IQueryable<Blog> dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.Blogs
                                           where foundItem.SubFolder == subFolder
                                           select foundItem;

                if (dtoList != null && dtoList.Count() > 0)
                {
                    retVal = dtoList.Single();
                }
            }

            return retVal;
        }

        public IList<Blog> GetByUserId(long userId)
        {
            IQueryable<Blog> retVal = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.Blogs
                                      join userBlogs in ((UnitOfWork)this.UnitOfWork).DataContext.BlogUsers on foundItem.Id equals userBlogs.Blog.Id
                                      where userBlogs.User.Id == userId
                                      select foundItem;
            return retVal.ToList();
        }
    }
}
