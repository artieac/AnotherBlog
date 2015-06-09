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
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.Common.DataLayer.NHibernate;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer.DTO;
using AlwaysMoveForward.AnotherBlog.DataLayer.Repositories;

namespace AlwaysMoveForward.AnotherBlog.DataLayer
{
    public class RepositoryManager : IAnotherBlogRepositoryManager
    {
        public RepositoryManager(UnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        public UnitOfWork UnitOfWork { get; private set; }

        private IBlogEntryRepository blogEntryRepository;
        public IBlogEntryRepository BlogEntries
        {
            get
            {
                if (this.blogEntryRepository == null)
                {
                    this.blogEntryRepository = new BlogEntryRepository(this.UnitOfWork, this.Tags);
                }

                return this.blogEntryRepository;
            }
        }

        private IBlogRepository blogRepository;
        public IBlogRepository Blogs
        {
            get
            {
                if (this.blogRepository == null)
                {
                    this.blogRepository = new BlogRepository(this.UnitOfWork);
                }

                return this.blogRepository;
            }
        }        

        private IDbInfoRepository databaseInfoRepository;
        public IDbInfoRepository DbInfo
        {
            get
            {
                if (this.databaseInfoRepository == null)
                {
                    this.databaseInfoRepository = new DbInfoRepository(this.UnitOfWork);
                }

                return this.databaseInfoRepository;
            }
        }

        private ISiteInfoRepository siteInfoRepository;
        public ISiteInfoRepository SiteInfo
        {
            get
            {
                if (this.siteInfoRepository == null)
                {
                    this.siteInfoRepository = new SiteInfoRepository(this.UnitOfWork);
                }

                return this.siteInfoRepository;
            }
        }

        private ITagRepository tagRepository;
        public ITagRepository Tags
        {
            get
            {
                if (this.tagRepository == null)
                {
                    this.tagRepository = new TagRepository(this.UnitOfWork);
                }

                return this.tagRepository;
            }
        }

        private IUserRepository userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new UserRepository(this.UnitOfWork);
                }

                return this.userRepository;
            }
        }

        private IBlogListRepository blogListRepository;
        public IBlogListRepository BlogLists
        {
            get
            {
                if (this.blogListRepository == null)
                {
                    this.blogListRepository = new BlogListRepository(this.UnitOfWork);
                }

                return this.blogListRepository;
            }
        }

        private IPollRepository pollRepository;
        public IPollRepository PollRepository
        {
            get
            {
                if (this.pollRepository == null)
                {
                    this.pollRepository = new PollRepository(this.UnitOfWork);
                }

                return this.pollRepository;
            }
        }
    }
}
