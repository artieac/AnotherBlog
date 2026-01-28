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
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer.Repositories;

namespace AlwaysMoveForward.AnotherBlog.DataLayer
{
    public class RepositoryManager : IAnotherBlogRepositoryManager
    {
        // Interface-defined repositories
        private IBlogEntryRepository blogEntryRepository;
        private IBlogRepository blogRepository;
        private IBlogListRepository blogListRepository;
        private IDbInfoRepository dbInfoRepository;
        private ISiteInfoRepository siteInfoRepository;
        private ITagRepository tagRepository;
        private IUserRepository userRepository;

        // Additional repositories (not in interface)
        private BlogEntryTagRepository blogEntryTagRepository;
        private BlogExtensionRepository blogExtensionRepository;
        private BlogListItemRepository blogListItemRepository;
        private BlogUserRepository blogUserRepository;
        private EntryCommentRepository entryCommentRepository;
        private ExtensionConfigurationRepository extensionConfigurationRepository;
        private RoleRepository roleRepository;

        public IUnitOfWork UnitOfWork { get; set; }

        public RepositoryManager()
        {
        }

        public RepositoryManager(UnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        public IRepository<TargetType, int> GetRepository<TargetType>() where TargetType : class
        {
            IRepository<TargetType, int> retVal = null;

            if (typeof(TargetType) == typeof(BlogEntryRepository))
            {
                retVal = (IRepository<TargetType, int>)this.BlogEntries;
            }

            return retVal;
        }

        // IAnotherBlogRepositoryManager interface members
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

        public IBlogListRepository BlogLists
        {
            get
            {
                if (this.blogListRepository == null)
                {
                    this.blogListRepository = new BlogListRepository(this.UnitOfWork, this);
                }
                return this.blogListRepository;
            }
        }

        public IPollRepository PollRepository
        {
            get
            {
                throw new NotImplementedException("PollRepository is not implemented in this data layer.");
            }
        }

        public IUserRepository UserRepository
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

        // Additional repository accessors (not in interface)
        public BlogEntryTagRepository BlogEntryTags
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

        public BlogExtensionRepository BlogExtensions
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

        public BlogListItemRepository BlogListItems
        {
            get
            {
                if (this.blogListItemRepository == null)
                {
                    this.blogListItemRepository = new BlogListItemRepository(this.UnitOfWork, this);
                }
                return this.blogListItemRepository;
            }
        }

        public BlogUserRepository BlogUsers
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

        public EntryCommentRepository EntryComments
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

        public ExtensionConfigurationRepository ExtensionConfiguration
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

        public RoleRepository Roles
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
    }
}
