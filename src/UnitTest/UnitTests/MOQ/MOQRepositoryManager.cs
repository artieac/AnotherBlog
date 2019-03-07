using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using PucksAndProgramming.Common.DataLayer;
using PucksAndProgramming.Common.DataLayer.Repositories;
using PucksAndProgramming.AnotherBlog.Common.DataLayer.Repositories;
using PucksAndProgramming.AnotherBlog.UnitTest.MOQ.Repositories;

namespace PucksAndProgramming.AnotherBlog.UnitTest
{
    public class MOQRepositoryManager : IAnotherBlogRepositoryManager
    {
        Mock<IBlogEntryRepository> blogEntryRepository;
        Mock<IBlogRepository> blogRepository;
        Mock<IDbInfoRepository> dbInfoRepository;
        Mock<ISiteInfoRepository> siteInfoRepository;
        Mock<ITagRepository> tagRepository;
        Mock<IUserRepository> userRepository;
        Mock<IBlogListRepository> blogLists;
        Mock<IPollRepository> pollRepository;
        
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

        public ITagRepository Tags
        {
            get
            {
                if (this.tagRepository == null)
                {
                    this.tagRepository = new Mock<ITagRepository>();
                    MOQTagRepositoryHelper.ConfigureGetAll(this.tagRepository);
                    MOQTagRepositoryHelper.ConfigureGetByName(this.tagRepository);
                    MOQTagRepositoryHelper.ConfigureSave(this.tagRepository);
                
                }

                return this.tagRepository.Object;
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
    }
}
