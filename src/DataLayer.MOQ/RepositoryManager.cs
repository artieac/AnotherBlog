using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Moq;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;

namespace AlwaysMoveForward.AnotherBlog.DataLayer
{
    public class RepositoryManager : IAnotherBlogRepositoryManager
    {
        Mock<IBlogEntryRepository> blogEntryRepository;
        Mock<IBlogRepository> blogRepository;
        Mock<IBlogUserRepository> blogUserRepository;
        Mock<IDbInfoRepository> dbInfoRepository;
        Mock<ICommentRepository> entryCommentRepository;
        Mock<IRoleRepository> roleRepository;
        Mock<ISiteInfoRepository> siteInfoRepository;
        Mock<IUserRepository> userRepository;
        Mock<IBlogListRepository> blogLists;
        Mock<IBlogListItemRepository> blogListItems;
        Mock<ICommentRepository> commentRepository;

        public RepositoryManager()
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
                    AlwaysMoveForward.AnotherBlog.DataLayer.Repositories.MOQBlogRepositoryHelper.ConfigureGetBySubFolder(this.blogRepository);
                }

                return this.blogRepository.Object;
            }
        }

        public IBlogUserRepository BlogUsers
        {
            get
            {
                if (this.blogUserRepository == null)
                {
                    this.blogUserRepository = new Mock<IBlogUserRepository>();
                }

                return this.blogUserRepository.Object;
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

        public ICommentRepository EntryComments
        {
            get
            {
                if (this.entryCommentRepository == null)
                {
                    this.entryCommentRepository = new Mock<ICommentRepository>();
                }

                return this.entryCommentRepository.Object;
            }
        }

        public IRoleRepository Roles
        {
            get
            {
                if (this.roleRepository == null)
                {
                    this.roleRepository = new Mock<IRoleRepository>();
                }

                return this.roleRepository.Object;
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

        public IBlogListItemRepository BlogListItems
        {
            get
            {
                if (this.blogListItems == null)
                {
                    this.blogListItems = new Mock<IBlogListItemRepository>();
                }

                return this.blogListItems.Object;
            }
        }

        public ICommentRepository CommentRepository
        {
            get
            {
                if (this.commentRepository == null)
                {
                    this.commentRepository = new Mock<ICommentRepository>();
                }

                return this.commentRepository.Object;
            }
        }
    }
}
