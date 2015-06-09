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
using System.Reflection;
using System.Configuration;
using System.Data;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer.Repositories;

namespace AlwaysMoveForward.AnotherBlog.DataLayer
{
    public class RepositoryManager : IRepositoryManager
    {
        IBlogEntryRepository blogEntryRepository;
        IBlogEntryTagRepository blogEntryTagRepository;
        IBlogExtensionRepository blogExtensionRepository;
        IBlogRepository blogRepository;
        IBlogUserRepository blogUserRepository;
        IDbInfoRepository dbInfoRepository;
        IEntryCommentRepository entryCommentRepository;
        IExtensionConfigurationRepository extensionConfigurationRepository;
        IRoleRepository roleRepository;
        ISiteInfoRepository siteInfoRepository;
        ITagRepository tagRepository;
        IUserRepository userRepository;
        IBlogListRepository blogLists;
        IBlogListItemRepository blogListItems;

        public RepositoryManager()
        {
        }

        public IUnitOfWork UnitOfWork { get; set; }

        public IRepository<TargetType> GetRepository<TargetType>() where TargetType : class
        {
            IRepository<TargetType> retVal = null;

            if(typeof(TargetType) == typeof(BlogEntryRepository))
            {
                retVal = (IRepository<TargetType>)this.BlogEntries;
            }

            return retVal;
        }

        public IBlogEntryRepository BlogEntries
        {
            get
            {
                if (this.blogEntryRepository == null)
                {
                    this.blogEntryRepository = new BlogEntryRepository(this.UnitOfWork, this);
                }

                return this.blogEntryRepository;
            }
        }

        public IBlogEntryTagRepository BlogEntryTags
        {
            get
            {
                if (this.blogEntryTagRepository == null)
                {
                    this.blogEntryTagRepository = new BlogEntryTagRepository(this.UnitOfWork, this);
                }

                return this.blogEntryTagRepository;
            }
        }

        public IBlogExtensionRepository BlogExtensions
        {
            get
            {
                if (this.blogExtensionRepository == null)
                {
                    this.blogExtensionRepository = new BlogExtensionRepository(this.UnitOfWork, this);
                }

                return this.blogExtensionRepository;
            }
        }

        public IBlogRepository Blogs
        {
            get
            {
                if (this.blogRepository == null)
                {
                    this.blogRepository = new BlogRepository(this.UnitOfWork, this);
                }

                return this.blogRepository;
            }
        }

        public IBlogUserRepository BlogUsers
        {
            get
            {
                if (this.blogUserRepository == null)
                {
                    this.blogUserRepository = new BlogUserRepository(this.UnitOfWork, this);
                }

                return this.blogUserRepository;
            }
        }

        public IDbInfoRepository DbInfo
        {
            get
            {
                if (this.dbInfoRepository == null)
                {
                    this.dbInfoRepository = new DbInfoRepository(this.UnitOfWork, this);
                }

                return this.dbInfoRepository;
            }
        }

        public IEntryCommentRepository EntryComments
        {
            get
            {
                if (this.entryCommentRepository == null)
                {
                    this.entryCommentRepository = new EntryCommentRepository(this.UnitOfWork, this);
                }

                return this.entryCommentRepository;
            }
        }

        public IExtensionConfigurationRepository ExtensionConfiguration
        {
            get
            {
                if (this.extensionConfigurationRepository == null)
                {
                    this.extensionConfigurationRepository = new ExtensionConfigurationRepository(this.UnitOfWork, this);
                }

                return this.extensionConfigurationRepository;
            }
        }

        public IRoleRepository Roles
        {
            get
            {
                if (this.roleRepository == null)
                {
                    this.roleRepository = new RoleRepository(this.UnitOfWork, this);
                }

                return this.roleRepository;
            }
        }

        public ISiteInfoRepository SiteInfo
        {
            get
            {
                if (this.siteInfoRepository == null)
                {
                    this.siteInfoRepository = new SiteInfoRepository(this.UnitOfWork, this);
                }

                return this.siteInfoRepository;
            }
        }

        public ITagRepository Tags
        {
            get
            {
                if (this.tagRepository == null)
                {
                    this.tagRepository = new TagRepository(this.UnitOfWork, this);
                }

                return this.tagRepository;
            }
        }

        public IUserRepository Users
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new UserRepository(this.UnitOfWork, this);
                }

                return this.userRepository;
            }
        }

        public IBlogListRepository BlogLists
        {
            get
            {
                if (this.blogLists == null)
                {
                    this.blogLists = new BlogListRepository(this.UnitOfWork, this);
                }

                return this.blogLists;
            }
        }

        public IBlogListItemRepository BlogListItems
        {
            get
            {
                if (this.blogListItems == null)
                {
                    this.blogListItems = new BlogListItemRepository(this.UnitOfWork, this);
                }

                return this.blogListItems;
            }
        }

    }
}

