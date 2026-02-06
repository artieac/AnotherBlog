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
using NUnit.Framework;
using Moq;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.DataLayer;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.Services
{
    [TestFixture]
    public class ServiceManagerTest
    {
        private Mock<UnitOfWork> mockUnitOfWork;
        private Mock<IAnotherBlogRepositoryManager> mockRepositoryManager;
        private Mock<IBlogRepository> mockBlogRepository;
        private Mock<IBlogEntryRepository> mockBlogEntryRepository;
        private Mock<ITagRepository> mockTagRepository;
        private Mock<ISiteInfoRepository> mockSiteInfoRepository;
        private Mock<IUserRepository> mockUserRepository;
        private Mock<IBlogListRepository> mockBlogListRepository;
        private Mock<IEntryCommentRepository> mockCommentRepository;

        [SetUp]
        public void Setup()
        {
            mockUnitOfWork = new Mock<UnitOfWork>(MockBehavior.Loose, null);
            mockRepositoryManager = new Mock<IAnotherBlogRepositoryManager>();
            mockBlogRepository = new Mock<IBlogRepository>();
            mockBlogEntryRepository = new Mock<IBlogEntryRepository>();
            mockTagRepository = new Mock<ITagRepository>();
            mockSiteInfoRepository = new Mock<ISiteInfoRepository>();
            mockUserRepository = new Mock<IUserRepository>();
            mockBlogListRepository = new Mock<IBlogListRepository>();
            mockCommentRepository = new Mock<IEntryCommentRepository>();

            mockRepositoryManager.Setup(x => x.Blogs).Returns(mockBlogRepository.Object);
            mockRepositoryManager.Setup(x => x.BlogEntries).Returns(mockBlogEntryRepository.Object);
            mockRepositoryManager.Setup(x => x.Tags).Returns(mockTagRepository.Object);
            mockRepositoryManager.Setup(x => x.SiteInfo).Returns(mockSiteInfoRepository.Object);
            mockRepositoryManager.Setup(x => x.UserRepository).Returns(mockUserRepository.Object);
            mockRepositoryManager.Setup(x => x.BlogLists).Returns(mockBlogListRepository.Object);
            mockRepositoryManager.Setup(x => x.EntryComments).Returns(mockCommentRepository.Object);
        }

        [Test]
        public void Constructor_SetsUnitOfWork()
        {
            var manager = new ServiceManager(mockUnitOfWork.Object, mockRepositoryManager.Object);

            Assert.That(manager.UnitOfWork, Is.EqualTo(mockUnitOfWork.Object));
        }

        [Test]
        public void Constructor_SetsRepositoryManager()
        {
            var manager = new ServiceManager(mockUnitOfWork.Object, mockRepositoryManager.Object);

            Assert.That(manager.RepositoryManager, Is.EqualTo(mockRepositoryManager.Object));
        }

        [Test]
        public void BlogService_ReturnsLazyLoadedInstance()
        {
            var manager = new ServiceManager(mockUnitOfWork.Object, mockRepositoryManager.Object);

            var service = manager.BlogService;

            Assert.That(service, Is.Not.Null);
            Assert.That(service, Is.TypeOf<BlogService>());
        }

        [Test]
        public void BlogService_ReturnsSameInstanceOnMultipleCalls()
        {
            var manager = new ServiceManager(mockUnitOfWork.Object, mockRepositoryManager.Object);

            var service1 = manager.BlogService;
            var service2 = manager.BlogService;

            Assert.That(service1, Is.SameAs(service2));
        }

        [Test]
        public void BlogEntryService_ReturnsLazyLoadedInstance()
        {
            var manager = new ServiceManager(mockUnitOfWork.Object, mockRepositoryManager.Object);

            var service = manager.BlogEntryService;

            Assert.That(service, Is.Not.Null);
            Assert.That(service, Is.TypeOf<BlogEntryService>());
        }

        [Test]
        public void BlogEntryService_ReturnsSameInstanceOnMultipleCalls()
        {
            var manager = new ServiceManager(mockUnitOfWork.Object, mockRepositoryManager.Object);

            var service1 = manager.BlogEntryService;
            var service2 = manager.BlogEntryService;

            Assert.That(service1, Is.SameAs(service2));
        }

        [Test]
        public void TagService_ReturnsLazyLoadedInstance()
        {
            var manager = new ServiceManager(mockUnitOfWork.Object, mockRepositoryManager.Object);

            var service = manager.TagService;

            Assert.That(service, Is.Not.Null);
            Assert.That(service, Is.TypeOf<TagService>());
        }

        [Test]
        public void TagService_ReturnsSameInstanceOnMultipleCalls()
        {
            var manager = new ServiceManager(mockUnitOfWork.Object, mockRepositoryManager.Object);

            var service1 = manager.TagService;
            var service2 = manager.TagService;

            Assert.That(service1, Is.SameAs(service2));
        }

        [Test]
        public void SiteInfoService_ReturnsLazyLoadedInstance()
        {
            var manager = new ServiceManager(mockUnitOfWork.Object, mockRepositoryManager.Object);

            var service = manager.SiteInfoService;

            Assert.That(service, Is.Not.Null);
            Assert.That(service, Is.TypeOf<SiteInfoService>());
        }

        [Test]
        public void SiteInfoService_ReturnsSameInstanceOnMultipleCalls()
        {
            var manager = new ServiceManager(mockUnitOfWork.Object, mockRepositoryManager.Object);

            var service1 = manager.SiteInfoService;
            var service2 = manager.SiteInfoService;

            Assert.That(service1, Is.SameAs(service2));
        }

        [Test]
        public void UserService_ReturnsLazyLoadedInstance()
        {
            var manager = new ServiceManager(mockUnitOfWork.Object, mockRepositoryManager.Object);

            var service = manager.UserService;

            Assert.That(service, Is.Not.Null);
            Assert.That(service, Is.TypeOf<UserService>());
        }

        [Test]
        public void UserService_ReturnsSameInstanceOnMultipleCalls()
        {
            var manager = new ServiceManager(mockUnitOfWork.Object, mockRepositoryManager.Object);

            var service1 = manager.UserService;
            var service2 = manager.UserService;

            Assert.That(service1, Is.SameAs(service2));
        }

        [Test]
        public void BlogListService_ReturnsLazyLoadedInstance()
        {
            var manager = new ServiceManager(mockUnitOfWork.Object, mockRepositoryManager.Object);

            var service = manager.BlogListService;

            Assert.That(service, Is.Not.Null);
            Assert.That(service, Is.TypeOf<BlogListService>());
        }

        [Test]
        public void BlogListService_ReturnsSameInstanceOnMultipleCalls()
        {
            var manager = new ServiceManager(mockUnitOfWork.Object, mockRepositoryManager.Object);

            var service1 = manager.BlogListService;
            var service2 = manager.BlogListService;

            Assert.That(service1, Is.SameAs(service2));
        }

        [Test]
        public void CommentService_ReturnsLazyLoadedInstance()
        {
            var manager = new ServiceManager(mockUnitOfWork.Object, mockRepositoryManager.Object);

            var service = manager.CommentService;

            Assert.That(service, Is.Not.Null);
            Assert.That(service, Is.TypeOf<CommentService>());
        }

        [Test]
        public void CommentService_ReturnsSameInstanceOnMultipleCalls()
        {
            var manager = new ServiceManager(mockUnitOfWork.Object, mockRepositoryManager.Object);

            var service1 = manager.CommentService;
            var service2 = manager.CommentService;

            Assert.That(service1, Is.SameAs(service2));
        }

        [Test]
        public void UploadedFiles_ReturnsLazyLoadedInstance()
        {
            var manager = new ServiceManager(mockUnitOfWork.Object, mockRepositoryManager.Object);

            var service = manager.UploadedFiles;

            Assert.That(service, Is.Not.Null);
            Assert.That(service, Is.TypeOf<UploadedFileManager>());
        }

        [Test]
        public void UploadedFiles_ReturnsSameInstanceOnMultipleCalls()
        {
            var manager = new ServiceManager(mockUnitOfWork.Object, mockRepositoryManager.Object);

            var service1 = manager.UploadedFiles;
            var service2 = manager.UploadedFiles;

            Assert.That(service1, Is.SameAs(service2));
        }
    }
}
