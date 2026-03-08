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
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.Services
{
    [TestFixture]
    public class BlogEntryServiceTest
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<IBlogEntryRepository> mockBlogEntryRepository;
        private Mock<ITagRepository> mockTagRepository;
        private BlogEntryService service;
        private Blog testBlog;
        private BlogPost testPost;

        [SetUp]
        public void Setup()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockBlogEntryRepository = new Mock<IBlogEntryRepository>();
            mockTagRepository = new Mock<ITagRepository>();

            service = new BlogEntryService(mockUnitOfWork.Object, mockBlogEntryRepository.Object, mockTagRepository.Object);

            testBlog = new Blog { Id = 1, Name = "TestBlog", SubFolder = "test" };
            testPost = new BlogPost
            {
                Id = 1,
                Title = "Test Post",
                EntryText = "Test Content",
                Blog = testBlog,
                DatePosted = DateTime.Now,
                TimesViewed = 5
            };
        }

        [Test]
        public void GetAll_ReturnsAllBlogPosts()
        {
            var posts = new List<BlogPost> { testPost };
            mockBlogEntryRepository.Setup(x => x.GetAll()).Returns(posts);

            var result = service.GetAll();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            mockBlogEntryRepository.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public void GetAllByBlog_WithValidBlog_ReturnsPosts()
        {
            var posts = new List<BlogPost> { testPost };
            mockBlogEntryRepository.Setup(x => x.GetAllByBlog(testBlog.Id, true, -1, BlogEntryService.DefaultPostSort, false))
                .Returns(posts);

            var result = service.GetAllByBlog(testBlog, true);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GetAllByBlog_WithNullBlog_ReturnsEmptyList()
        {
            var result = service.GetAllByBlog(null, true);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetAllByBlog_WithMaxResults_ReturnsPosts()
        {
            var posts = new List<BlogPost> { testPost };
            mockBlogEntryRepository.Setup(x => x.GetAllByBlog(testBlog.Id, true, 10, BlogEntryService.DefaultPostSort, false))
                .Returns(posts);

            var result = service.GetAllByBlog(testBlog, true, 10);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GetAllByBlog_WithSortParameters_ReturnsPosts()
        {
            var posts = new List<BlogPost> { testPost };
            mockBlogEntryRepository.Setup(x => x.GetAllByBlog(testBlog.Id, false, 5, "Title", true))
                .Returns(posts);

            var result = service.GetAllByBlog(testBlog, false, 5, "Title", true);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetAllByBlogId_WithValidBlog_ReturnsPosts()
        {
            var result = service.GetAllByBlogId(testBlog, 0);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GetAllByBlogId_WithNullBlog_ReturnsEmptyList()
        {
            var result = service.GetAllByBlogId(null, 0);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetById_WithValidBlog_ReturnsPost()
        {
            mockBlogEntryRepository.Setup(x => x.GetById(1)).Returns(testPost);

            var result = service.GetById(testBlog, 1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public void GetById_WithNullBlog_ReturnsNull()
        {
            var result = service.GetById(null, 1);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetByTitle_WithValidBlog_ReturnsPost()
        {
            mockBlogEntryRepository.Setup(x => x.GetByTitle("Test Post", testBlog.Id)).Returns(testPost);

            var result = service.GetByTitle(testBlog, "Test Post");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Test Post"));
        }

        [Test]
        public void GetByTitle_WithNullBlog_ReturnsNull()
        {
            var result = service.GetByTitle(null, "Test Post");

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetByDateAndTitle_WithValidBlog_ReturnsPost()
        {
            var postDate = DateTime.Now;
            mockBlogEntryRepository.Setup(x => x.GetByDateAndTitle("Test Post", postDate, testBlog.Id)).Returns(testPost);

            var result = service.GetByDateAndTitle(testBlog, postDate, "Test Post");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GetByDateAndTitle_WithNullBlog_ReturnsNull()
        {
            var result = service.GetByDateAndTitle(null, DateTime.Now, "Test Post");

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetByTag_WithValidBlog_ReturnsPosts()
        {
            var posts = new List<BlogPost> { testPost };
            mockBlogEntryRepository.Setup(x => x.GetByTag(testBlog.Id, "testtag", true)).Returns(posts);

            var result = service.GetByTag(testBlog, "testtag", true);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetByTag_WithNullBlog_ReturnsEmptyList()
        {
            var result = service.GetByTag(null, "testtag", true);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetByMonth_WithDate_ReturnsPosts()
        {
            var blogDate = new DateTime(2024, 1, 1);
            var posts = new List<BlogPost> { testPost };
            mockBlogEntryRepository.Setup(x => x.GetByMonth(blogDate, true)).Returns(posts);

            var result = service.GetByMonth(blogDate, true);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetByMonth_WithBlogAndDate_ReturnsPosts()
        {
            var blogDate = new DateTime(2024, 1, 1);
            var posts = new List<BlogPost> { testPost };
            mockBlogEntryRepository.Setup(x => x.GetByMonth(blogDate, testBlog.Id, true)).Returns(posts);

            var result = service.GetByMonth(testBlog, blogDate, true);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetByMonth_WithNullBlog_ReturnsEmptyList()
        {
            var result = service.GetByMonth(null, DateTime.Now, true);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetByDate_WithDate_ReturnsPosts()
        {
            var blogDate = new DateTime(2024, 1, 15);
            var posts = new List<BlogPost> { testPost };
            mockBlogEntryRepository.Setup(x => x.GetByDate(blogDate, true)).Returns(posts);

            var result = service.GetByDate(blogDate, true);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetByDate_WithBlogAndDate_ReturnsPosts()
        {
            var blogDate = new DateTime(2024, 1, 15);
            var posts = new List<BlogPost> { testPost };
            mockBlogEntryRepository.Setup(x => x.GetByDate(blogDate, testBlog.Id, true)).Returns(posts);

            var result = service.GetByDate(testBlog, blogDate, true);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetByDate_WithNullBlog_ReturnsEmptyList()
        {
            var result = service.GetByDate(null, DateTime.Now, true);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetMostRecent_WithValidBlog_ReturnsPost()
        {
            mockBlogEntryRepository.Setup(x => x.GetMostRecent(testBlog.Id, true)).Returns(testPost);

            var result = service.GetMostRecent(testBlog);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GetMostRecent_WithNullBlog_ReturnsNull()
        {
            var result = service.GetMostRecent(null);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetMostRecent_WithMaxResults_ReturnsPosts()
        {
            var posts = new List<BlogPost> { testPost };
            mockBlogEntryRepository.Setup(x => x.GetAll(true, 5)).Returns(posts);

            var result = service.GetMostRecent(5);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetMostRead_ReturnsPosts()
        {
            var posts = new List<BlogPost> { testPost };
            mockBlogEntryRepository.Setup(x => x.GetMostRead(10)).Returns(posts);

            var result = service.GetMostRead(10);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetMostRead_WithBlogId_ReturnsPosts()
        {
            var posts = new List<BlogPost> { testPost };
            mockBlogEntryRepository.Setup(x => x.GetMostRead(testBlog.Id, 10)).Returns(posts);

            var result = service.GetMostRead(testBlog.Id, 10);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetPreviousEntry_WithValidBlog_ReturnsPost()
        {
            var previousPost = new BlogPost { Id = 0, Title = "Previous" };
            mockBlogEntryRepository.Setup(x => x.GetPreviousEntry(testBlog.Id, testPost.Id)).Returns(previousPost);

            var result = service.GetPreviousEntry(testBlog, testPost);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Previous"));
        }

        [Test]
        public void GetPreviousEntry_WithNullBlog_ReturnsNull()
        {
            var result = service.GetPreviousEntry(null, testPost);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetNextEntry_WithValidBlog_ReturnsPost()
        {
            var nextPost = new BlogPost { Id = 2, Title = "Next" };
            mockBlogEntryRepository.Setup(x => x.GetNextEntry(testBlog.Id, testPost.Id)).Returns(nextPost);

            var result = service.GetNextEntry(testBlog, testPost);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Next"));
        }

        [Test]
        public void GetNextEntry_WithNullBlog_ReturnsNull()
        {
            var result = service.GetNextEntry(null, testPost);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetPublishedDatesByMonth_ReturnsDates()
        {
            var blogDate = new DateTime(2024, 1, 1);
            var dates = new List<DateTime> { new DateTime(2024, 1, 5), new DateTime(2024, 1, 10) };
            mockBlogEntryRepository.Setup(x => x.GetPublishedDatesByMonth(blogDate)).Returns(dates);

            var result = service.GetPublishedDatesByMonth(blogDate);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetArchiveDates_WithValidBlog_ReturnsDates()
        {
            var dates = new ArrayList { new DateTime(2024, 1, 1), new DateTime(2024, 2, 1) };
            mockBlogEntryRepository.Setup(x => x.GetArchiveDates(testBlog.Id)).Returns(dates);

            var result = service.GetArchiveDates(testBlog);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetArchiveDates_WithNullBlog_ReturnsDates()
        {
            var dates = new ArrayList { new DateTime(2024, 1, 1) };
            mockBlogEntryRepository.Setup(x => x.GetArchiveDates(null)).Returns(dates);

            var result = service.GetArchiveDates(null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Delete_CallsRepositoryDelete()
        {
            mockBlogEntryRepository.Setup(x => x.Delete(testPost)).Returns(true);

            var result = service.Delete(testPost);

            Assert.That(result, Is.True);
            mockBlogEntryRepository.Verify(x => x.Delete(testPost), Times.Once);
        }

        [Test]
        public void Save_WithBlogPost_SavesPost()
        {
            mockBlogEntryRepository.Setup(x => x.Save(testPost)).Returns(testPost);

            var result = service.Save(testPost);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(testPost.Id));
            mockBlogEntryRepository.Verify(x => x.Save(testPost), Times.Once);
        }

        [Test]
        public void Save_WithNullBlogPost_ReturnsNull()
        {
            var result = service.Save((BlogPost)null);

            Assert.That(result, Is.Null);
            mockBlogEntryRepository.Verify(x => x.Save(It.IsAny<BlogPost>()), Times.Never);
        }

        [Test]
        public void UpdateTimesViewed_WithValidPost_IncrementsViewCount()
        {
            testPost.TimesViewed = 5;
            mockBlogEntryRepository.Setup(x => x.Save(testPost)).Returns(testPost);

            var result = service.UpdateTimesViewed(testPost);

            Assert.That(result, Is.EqualTo(6));
            Assert.That(testPost.TimesViewed, Is.EqualTo(6));
            mockBlogEntryRepository.Verify(x => x.Save(testPost), Times.Once);
        }

        [Test]
        public void UpdateTimesViewed_WithNullPost_ReturnsZero()
        {
            var result = service.UpdateTimesViewed(null);

            Assert.That(result, Is.EqualTo(0));
            mockBlogEntryRepository.Verify(x => x.Save(It.IsAny<BlogPost>()), Times.Never);
        }
    }
}
