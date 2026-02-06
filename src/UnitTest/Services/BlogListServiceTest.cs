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
    public class BlogListServiceTest
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<IBlogListRepository> mockBlogListRepository;
        private BlogListService service;
        private Blog testBlog;
        private BlogList testBlogList;

        [SetUp]
        public void Setup()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockBlogListRepository = new Mock<IBlogListRepository>();

            service = new BlogListService(mockUnitOfWork.Object, mockBlogListRepository.Object);

            testBlog = new Blog { Id = 1, Name = "TestBlog", SubFolder = "test" };
            testBlogList = new BlogList
            {
                Id = 1,
                Name = "Test List",
                BlogId = testBlog.Id,
                ShowOrdered = true
            };
        }

        [Test]
        public void GetById_ReturnsListById()
        {
            mockBlogListRepository.Setup(x => x.GetById(1)).Returns(testBlogList);

            var result = service.GetById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            mockBlogListRepository.Verify(x => x.GetById(1), Times.Once);
        }

        [Test]
        public void GetById_WithInvalidId_ReturnsNull()
        {
            mockBlogListRepository.Setup(x => x.GetById(999)).Returns((BlogList)null);

            var result = service.GetById(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetByBlog_ReturnsListsForBlog()
        {
            var lists = new List<BlogList> { testBlogList };
            mockBlogListRepository.Setup(x => x.GetByBlog(testBlog.Id)).Returns(lists);

            var result = service.GetByBlog(testBlog);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            mockBlogListRepository.Verify(x => x.GetByBlog(testBlog.Id), Times.Once);
        }

        [Test]
        public void GetByName_WithValidBlog_ReturnsList()
        {
            mockBlogListRepository.Setup(x => x.GetByNameAndBlogId("Test List", testBlog.Id)).Returns(testBlogList);

            var result = service.GetByName(testBlog, "Test List");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Test List"));
        }

        [Test]
        public void GetByName_WithNullBlog_ReturnsNull()
        {
            var result = service.GetByName(null, "Test List");

            Assert.That(result, Is.Null);
            mockBlogListRepository.Verify(x => x.GetByNameAndBlogId(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void GetListNames_WithValidBlog_ReturnsNames()
        {
            var lists = new List<BlogList>
            {
                new BlogList { Id = 1, Name = "List1", BlogId = testBlog.Id },
                new BlogList { Id = 2, Name = "List2", BlogId = testBlog.Id }
            };
            mockBlogListRepository.Setup(x => x.GetByBlog(testBlog.Id)).Returns(lists);

            var result = service.GetListNames(testBlog);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result, Does.Contain("List1"));
            Assert.That(result, Does.Contain("List2"));
        }

        [Test]
        public void GetListNames_WithNullBlog_ReturnsEmptyList()
        {
            var result = service.GetListNames(null);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetListNames_WithNoLists_ReturnsEmptyList()
        {
            mockBlogListRepository.Setup(x => x.GetByBlog(testBlog.Id)).Returns((IList<BlogList>)null);

            var result = service.GetListNames(testBlog);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void Save_WithExistingList_UpdatesList()
        {
            mockBlogListRepository.Setup(x => x.GetByIdAndBlogId(1, testBlog.Id)).Returns(testBlogList);
            mockBlogListRepository.Setup(x => x.Save(It.IsAny<BlogList>())).Returns(testBlogList);

            var result = service.Save(testBlog, 1, "Updated Name", false);

            Assert.That(result, Is.Not.Null);
            mockBlogListRepository.Verify(x => x.Save(It.IsAny<BlogList>()), Times.Once);
        }

        [Test]
        public void Save_WithNewList_CreatesList()
        {
            mockBlogListRepository.Setup(x => x.Save(It.IsAny<BlogList>())).Returns((BlogList l) => l);

            var result = service.Save(testBlog, -1, "New List", true);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("New List"));
            Assert.That(result.ShowOrdered, Is.True);
        }

        [Test]
        public void Save_WithBlogListObject_SavesList()
        {
            mockBlogListRepository.Setup(x => x.Save(testBlogList)).Returns(testBlogList);

            var result = service.Save(testBlogList);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(testBlogList.Id));
            mockBlogListRepository.Verify(x => x.Save(testBlogList), Times.Once);
        }

        [Test]
        public void Save_WithNullBlogList_ReturnsNull()
        {
            var result = service.Save((BlogList)null);

            Assert.That(result, Is.Null);
            mockBlogListRepository.Verify(x => x.Save(It.IsAny<BlogList>()), Times.Never);
        }

        [Test]
        public void UpdateItem_WithValidList_UpdatesItem()
        {
            testBlogList.Items = new List<BlogListItem>();
            mockBlogListRepository.Setup(x => x.Save(testBlogList)).Returns(testBlogList);

            var result = service.UpdateItem(testBlogList, 0, "Item Name", "http://link.com", 1);

            Assert.That(result, Is.Not.Null);
            mockBlogListRepository.Verify(x => x.Save(testBlogList), Times.Once);
        }

        [Test]
        public void UpdateItem_WithNullList_ReturnsNull()
        {
            var result = service.UpdateItem(null, 0, "Item Name", "http://link.com", 1);

            Assert.That(result, Is.Null);
            mockBlogListRepository.Verify(x => x.Save(It.IsAny<BlogList>()), Times.Never);
        }

        [Test]
        public void Delete_WithValidList_DeletesList()
        {
            testBlogList.Items = new List<BlogListItem>();
            mockBlogListRepository.Setup(x => x.Delete(testBlogList)).Returns(true);

            var result = service.Delete(testBlogList);

            Assert.That(result, Is.True);
            mockBlogListRepository.Verify(x => x.Delete(testBlogList), Times.Once);
        }

        [Test]
        public void Delete_WithListContainingItems_ClearsItemsAndDeletes()
        {
            testBlogList.Items = new List<BlogListItem>
            {
                new BlogListItem { Id = 1, Name = "Item1" },
                new BlogListItem { Id = 2, Name = "Item2" }
            };
            mockBlogListRepository.Setup(x => x.Delete(testBlogList)).Returns(true);

            var result = service.Delete(testBlogList);

            Assert.That(result, Is.True);
            Assert.That(testBlogList.Items.Count, Is.EqualTo(0));
        }
    }
}
