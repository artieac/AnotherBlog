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
using AlwaysMoveForward.Common.Configuration;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.Common.Business;
using AlwaysMoveForward.OAuth.Client;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;
using AlwaysMoveForward.AnotherBlog.DataLayer;

namespace AlwaysMoveForward.AnotherBlog.BusinessLayer.Service
{
    public class ServiceManager
    {
        public ServiceManager(
            UnitOfWork unitOfWork, 
            IAnotherBlogRepositoryManager repositoryManager, 
            OAuthClientBase oauthClient)
        {
            this.UnitOfWork = unitOfWork;
            this.RepositoryManager = repositoryManager;
            this.OAuthClient = oauthClient;
        }

        public UnitOfWork UnitOfWork { get; set; }

        public IAnotherBlogRepositoryManager RepositoryManager { get; set; }

        public OAuthClientBase OAuthClient { get; private set; }

        private SiteInfoService siteInfo;
        public SiteInfoService SiteInfoService
        {
            get
            {
                if (this.siteInfo == null)
                {
                    this.siteInfo = new SiteInfoService(this.UnitOfWork, this.RepositoryManager.SiteInfo);
                }

                return this.siteInfo;
            }
        }

        private BlogEntryService blogEntryService;
        public BlogEntryService BlogEntryService
        {
            get
            {
                if (this.blogEntryService == null)
                {
                    this.blogEntryService = new BlogEntryService(this.UnitOfWork, this.RepositoryManager.BlogEntries, this.RepositoryManager.Tags);
                }

                return this.blogEntryService;
            }
        }

        private BlogService blogService;
        public BlogService BlogService
        {
            get
            {
                if (this.blogService == null)
                {
                    this.blogService = new BlogService(this.UnitOfWork, this.RepositoryManager.Blogs);
                }

                return this.blogService;
            }
        }

        private TagService tagService;
        public TagService TagService
        {
            get
            {
                if (this.tagService == null)
                {
                    this.tagService = new TagService(this.UnitOfWork, this.RepositoryManager.Tags);
                }

                return this.tagService;
            }
        }

        private UploadedFileManager uploadedFileManager;
        public UploadedFileManager UploadedFiles
        {
            get
            {
                if (this.uploadedFileManager == null)
                {
                    this.uploadedFileManager = new UploadedFileManager(this.UnitOfWork);
                }

                return this.uploadedFileManager;
            }
        }

        private BlogListService blogListService;
        public BlogListService BlogListService
        {
            get
            {
                if (this.blogListService == null)
                {
                    this.blogListService = new BlogListService(this.UnitOfWork, this.RepositoryManager.BlogLists);
                }

                return this.blogListService;
            }
        }

        private UserService userService;
        public UserService UserService
        {
            get
            {
                if (this.userService == null)
                {
                    this.userService = new UserService(this.UnitOfWork, this.RepositoryManager.UserRepository,  new OAuthRepository(this.OAuthClient));
                }

                return this.userService;
            }
        }

        private PollService pollService;
        public PollService PollService
        {
            get
            {
                if (this.pollService == null)
                {
                    this.pollService = new PollService(this.UnitOfWork, this.RepositoryManager.PollRepository);
                }

                return this.pollService;
            }
        }
    }
}
