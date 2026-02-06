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
using NUnit.Framework;
using Moq;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.Services
{
    [TestFixture]
    public class BlogServiceUnitTest
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<IBlogRepository> mockBlogRepository;
        private BlogService service;
        private Blog testBlog;

        [SetUp]
        public void Setup()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockBlogRepository = new Mock<IBlogRepository>();

            service = new BlogService(mockUnitOfWork.Object, mockBlogRepository.Object);

            testBlog = new Blog
            {
                Id = 1,
                Name = "TestBlog",
                SubFolder = "test",
                Description = "Test Description",
                About = "About Test",
                WelcomeMessage = "Welcome",
                Theme = "default"
            };
        }

        [Test]
        public void GetDefaultBlog_ReturnsFirstBlog()
        {
            mockBlogRepository.Setup(x => x.GetById(1)).Returns(testBlog);

            var result = service.GetDefaultBlog();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            mockBlogRepository.Verify(x => x.GetById(1), Times.Once);
        }

        [Test]
        public void GetAll_ReturnsAllBlogs()
        {
            var blogs = new List<Blog> { testBlog, new Blog { Id = 2, Name = "Blog2" } };
            mockBlogRepository.Setup(x => x.GetAll()).Returns(blogs);

            var result = service.GetAll();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetByUserId_ReturnsBlogsForUser()
        {
            var blogs = new List<Blog> { testBlog };
            mockBlogRepository.Setup(x => x.GetByUserId(1)).Returns(blogs);

            var result = service.GetByUserId(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetById_ReturnsBlog()
        {
            mockBlogRepository.Setup(x => x.GetById(1)).Returns(testBlog);

            var result = service.GetById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("TestBlog"));
        }

        [Test]
        public void GetById_WithInvalidId_ReturnsNull()
        {
            mockBlogRepository.Setup(x => x.GetById(999)).Returns((Blog)null);

            var result = service.GetById(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Delete_WithExistingBlog_DeletesBlog()
        {
            mockBlogRepository.Setup(x => x.GetById(1)).Returns(testBlog);
            mockBlogRepository.Setup(x => x.Delete(testBlog)).Returns(true);

            service.Delete(1);

            mockBlogRepository.Verify(x => x.Delete(testBlog), Times.Once);
        }

        [Test]
        public void Delete_WithNonExistingBlog_DoesNotDelete()
        {
            mockBlogRepository.Setup(x => x.GetById(999)).Returns((Blog)null);

            service.Delete(999);

            mockBlogRepository.Verify(x => x.Delete(It.IsAny<Blog>()), Times.Never);
        }

        [Test]
        public void GetByName_ReturnsBlog()
        {
            mockBlogRepository.Setup(x => x.GetByName("TestBlog")).Returns(testBlog);

            var result = service.GetByName("TestBlog");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("TestBlog"));
        }

        [Test]
        public void GetBySubFolder_ReturnsBlog()
        {
            mockBlogRepository.Setup(x => x.GetBySubFolder("test")).Returns(testBlog);

            var result = service.GetBySubFolder("test");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.SubFolder, Is.EqualTo("test"));
        }

        [Test]
        public void Save_WithNewBlog_CreatesBlog()
        {
            mockBlogRepository.Setup(x => x.Save(It.IsAny<Blog>())).Returns((Blog b) => b);

            var result = service.Save(-1, "NewBlog", "newblog", "Description", "About", "Welcome", "default");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("NewBlog"));
            Assert.That(result.SubFolder, Is.EqualTo("newblog"));
            mockBlogRepository.Verify(x => x.Save(It.IsAny<Blog>()), Times.Once);
        }

        [Test]
        public void Save_WithExistingBlog_UpdatesBlog()
        {
            mockBlogRepository.Setup(x => x.GetById(1)).Returns(testBlog);
            mockBlogRepository.Setup(x => x.Save(It.IsAny<Blog>())).Returns((Blog b) => b);

            var result = service.Save(1, "UpdatedBlog", "updated", "NewDesc", "NewAbout", "NewWelcome", "newtheme");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("UpdatedBlog"));
            mockBlogRepository.Verify(x => x.Save(It.IsAny<Blog>()), Times.Once);
        }

        [Test]
        public void Save_ShortOverload_WithExistingBlog_UpdatesBlog()
        {
            mockBlogRepository.Setup(x => x.GetById(1)).Returns(testBlog);
            mockBlogRepository.Setup(x => x.Save(It.IsAny<Blog>())).Returns((Blog b) => b);

            var result = service.Save(1, "NewDescription", "NewAbout", "NewWelcome");

            Assert.That(result, Is.Not.Null);
            mockBlogRepository.Verify(x => x.Save(It.IsAny<Blog>()), Times.Once);
        }

        [Test]
        public void Save_WithNullDescription_SetsEmptyString()
        {
            mockBlogRepository.Setup(x => x.Save(It.IsAny<Blog>())).Returns((Blog b) => b);

            var result = service.Save(-1, "Blog", "blog", null, "About", "Welcome", "default");

            Assert.That(result.Description, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Save_WithNullAbout_SetsEmptyString()
        {
            mockBlogRepository.Setup(x => x.Save(It.IsAny<Blog>())).Returns((Blog b) => b);

            var result = service.Save(-1, "Blog", "blog", "Desc", null, "Welcome", "default");

            Assert.That(result.About, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Save_WithNullWelcome_SetsEmptyString()
        {
            mockBlogRepository.Setup(x => x.Save(It.IsAny<Blog>())).Returns((Blog b) => b);

            var result = service.Save(-1, "Blog", "blog", "Desc", "About", null, "default");

            Assert.That(result.WelcomeMessage, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Save_WithEmptyStrings_SetsEmptyStrings()
        {
            mockBlogRepository.Setup(x => x.Save(It.IsAny<Blog>())).Returns((Blog b) => b);

            var result = service.Save(-1, "Blog", "blog", "", "", "", "default");

            Assert.That(result.Description, Is.EqualTo(string.Empty));
            Assert.That(result.About, Is.EqualTo(string.Empty));
            Assert.That(result.WelcomeMessage, Is.EqualTo(string.Empty));
        }
    }
}
