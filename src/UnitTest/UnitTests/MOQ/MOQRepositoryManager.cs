using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.UnitTest.MOQ.Repositories;

namespace AlwaysMoveForward.AnotherBlog.UnitTest
{
    public class MOQRepositoryManager : IAnotherBlogRepositoryManager
    {
        Mock<IBlogEntryRepository> blogEntryRepository;
        Mock<IBlogRepository> blogRepository;
        Mock<IDbInfoRepository> dbInfoRepository;
        Mock<ISiteInfoRepository> siteInfoRepository;
        Mock<IUserRepository> userRepository;
        Mock<IBlogListRepository> blogLists;
        Mock<IPollRepository> pollRepository;
        Mock<IEntryCommentRepository> entryCommentRepository;
        Mock<IBlogPostViewRepository> blogPostViewRepository;
        
        public MOQRepositoryManager()
        {
            
        }


        public IUnitOfWork UnitOfWork { get; set; }

        public IBlogEntryRepository BlogEntries
        {
            get
            {
                if (this.blogEntryRepository == null)
                {
                    this.blogEntryRepository = new Mock<IBlogEntryRepository>();
                }

                return this.blogEntryRepository.Object;
            }
        }

        public IBlogRepository Blogs
        {
            get
            {
                if (this.blogRepository == null)
                {
                    this.blogRepository = new Mock<IBlogRepository>();
                    MOQBlogRepositoryHelper.ConfigureGetBySubFolder(this.blogRepository);
                }

                return this.blogRepository.Object;
            }
        }

        public IDbInfoRepository DbInfo
        {
            get
            {
                if (this.dbInfoRepository == null)
                {
                    this.dbInfoRepository = new Mock<IDbInfoRepository>();
                }

                return this.dbInfoRepository.Object;
            }
        }

        public ISiteInfoRepository SiteInfo
        {
            get
            {
                if (this.siteInfoRepository == null)
                {
                    this.siteInfoRepository = new Mock<ISiteInfoRepository>();
                }

                return this.siteInfoRepository.Object;
            }
        }

        public IUserRepository Users
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new Mock<IUserRepository>();
                }

                return this.userRepository.Object;
            }
        }

        public IBlogListRepository BlogLists
        {
            get
            {
                if (this.blogLists == null)
                {
                    this.blogLists = new Mock<IBlogListRepository>();
                }

                return this.blogLists.Object;
            }
        }

        public IPollRepository PollRepository
        {
            get
            {
                if (this.pollRepository == null)
                {
                    this.pollRepository = new Mock<IPollRepository>();
                }

                return this.pollRepository.Object;
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new Mock<IUserRepository>();
                }

                return this.userRepository.Object;
            }
        }

        public IEntryCommentRepository EntryComments
        {
            get
            {
                if (this.entryCommentRepository == null)
                {
                    this.entryCommentRepository = new Mock<IEntryCommentRepository>();
                }

                return this.entryCommentRepository.Object;
            }
        }

        public IBlogPostViewRepository BlogPostViews
        {
            get
            {
                if (this.blogPostViewRepository == null)
                {
                    this.blogPostViewRepository = new Mock<IBlogPostViewRepository>();
                }

                return this.blogPostViewRepository.Object;
            }
        }
    }
}
