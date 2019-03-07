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
using System.Collections.Generic;
using System.Linq;
using PucksAndProgramming.Common.DataLayer;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Common.Factories;
using PucksAndProgramming.Common.DataLayer.Repositories;
using PucksAndProgramming.AnotherBlog.Common.DataLayer.Repositories;

namespace PucksAndProgramming.AnotherBlog.BusinessLayer.Service
{
    /// <summary>
    /// A managing class for a blog objects business rules.
    /// </summary>
    public class BlogService : AnotherBlogService
    {
        public BlogService(IUnitOfWork unitOfWork, IBlogRepository blogRepository) : base(unitOfWork) 
        {
            this.BlogRepository = blogRepository;
        }

        protected IBlogRepository BlogRepository { get; private set; }

        /// <summary>
        /// Get the default blog for the site (the first one created)
        /// </summary>
        /// <returns></returns>
        public Blog GetDefaultBlog()
        {
            return this.BlogRepository.GetById(1);
        }
        /// <summary>
        /// Get all blogs configured in the system.
        /// </summary>
        /// <returns></returns>
        public IList<Blog> GetAll()
        {
            return this.BlogRepository.GetAll();
        }
        /// <summary>
        /// Get all blogs that a user is associated with (via different security roles)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<Blog> GetByUserId(long userId)
        {
            return this.BlogRepository.GetByUserId(userId);
        }
        /// <summary>
        /// Get a particular blog by an id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Blog GetById(int id)
        {
            return this.BlogRepository.GetById(id);
        }
        /// <summary>
        /// Delete a blog entry.
        /// </summary>
        /// <param name="blogId"></param>
        public void Delete(int blogId)
        {
            Blog targetBlog = this.BlogRepository.GetById(blogId);

            if (targetBlog != null)
            {
                this.BlogRepository.Delete(targetBlog);
            }
        }
        /// <summary>
        /// Get a particular blog by its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Blog GetByName(string name)
        {
            return this.BlogRepository.GetByName(name);
        }
        /// <summary>
        /// Get a particular blog by its site subfolder
        /// </summary>
        /// <param name="subFolder"></param>
        /// <returns></returns>
        public Blog GetBySubFolder(string subFolder)
        {
            return this.BlogRepository.GetBySubFolder(subFolder);
        }

        /// <summary>
        /// Save a blog instance and its configuration settings.
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="name"></param>
        /// <param name="subFolder"></param>
        /// <param name="description"></param>
        /// <param name="about"></param>
        /// <param name="blogWelcome"></param>
        /// <returns></returns>
        public Blog Save(int blogId, string description, string about, string blogWelcome)
        {
            Blog itemToSave = null;

            if (blogId <= 0)
            {
                itemToSave = BlogFactory.CreateBlog();
            }
            else
            {
                itemToSave = this.BlogRepository.GetById(blogId);
            }

            return this.Save(itemToSave.Id, itemToSave.Name, itemToSave.SubFolder, description, about, blogWelcome, itemToSave.Theme);
        }

        /// <summary>
        /// Save a blog instance and its configuration settings.
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="name"></param>
        /// <param name="subFolder"></param>
        /// <param name="description"></param>
        /// <param name="about"></param>
        /// <param name="blogWelcome"></param>
        /// <returns></returns>
        public Blog Save(int blogId, string name, string subFolder, string description, string about, string blogWelcome, string blogTheme)
        {
            Blog itemToSave = null;

            if (blogId <= 0)
            {
                itemToSave = BlogFactory.CreateBlog();
            }
            else
            {
                itemToSave = this.BlogRepository.GetById(blogId);
            }

            itemToSave.Name = name;
            itemToSave.SubFolder = subFolder;

            if(string.IsNullOrEmpty(description))
            {
                description = string.Empty;
            }

            itemToSave.Description = description;

            if(string.IsNullOrEmpty(about))
            {
                about = string.Empty;
            }

            itemToSave.About = about;

            if(string.IsNullOrEmpty(blogWelcome))
            {
                blogWelcome = string.Empty;
            }

            itemToSave.WelcomeMessage = blogWelcome;
            itemToSave.Theme = blogTheme;

            return this.BlogRepository.Save(itemToSave);
        }
    }
}
