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
    public class BlogRepository : LINQRepository<Blog, BlogDTO>, IBlogRepository
    {
        /// <summary>
        /// This class contains all the code to extract data from the repository using LINQ
        /// </summary>
        /// <param name="dataContext"></param>
        internal BlogRepository(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
            : base(unitOfWork, repositoryManager)
        {
        }

        public override string IdPropertyName
        {
            get { return "BlogId"; }
        }

        protected override BlogDTO GetDTOByDomain(Blog targetItem)
        {
            return this.GetDtoById(targetItem.BlogId);
        }
        /// <summary>
        /// Get a blog as specified by the name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Blog GetByName(string name)
        {
            return this.GetByProperty("Name", name); 
        }
        /// <summary>
        /// Get a blog specified by the site subfolder that contains it.
        /// </summary>
        /// <param name="subFolder"></param>
        /// <returns></returns>
        public Blog GetBySubFolder(string subFolder)
        {
            return this.GetByProperty("SubFolder", subFolder); 
        }
        /// <summary>
        /// Get all blogs that a user is associated with (i.e. ones that the user has security access specifations for it)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<Blog> GetByUserId(int userId)
        {
            IQueryable<BlogDTO> dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogDTOs
                                      join userBlogs in ((UnitOfWork)this.UnitOfWork).DataContext.BlogUserDTOs on foundItem.BlogId equals userBlogs.BlogDTO.BlogId
                                      where userBlogs.UserDTO.UserId == userId
                                      select foundItem;
            return this.Map(dtoList.ToList());
        }

        public override Blog Save(Blog itemToSave)
        {
            BlogDTO targetItem = this.GetDTOByDomain(itemToSave);

            if (targetItem == null)
            {
                targetItem = this.Map(itemToSave);
                ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<BlogDTO>().InsertOnSubmit(targetItem);
            }
            else
            {
                targetItem.About = itemToSave.About;
                targetItem.ContactEmail = itemToSave.ContactEmail;
                targetItem.Description = itemToSave.Description;
                targetItem.Name = itemToSave.Name;
                targetItem.SubFolder = itemToSave.SubFolder;
                targetItem.Theme = itemToSave.Theme;
                targetItem.WelcomeMessage = itemToSave.WelcomeMessage;
            }

            this.UnitOfWork.Flush();

            return this.Map(targetItem);
        }
    }
}
