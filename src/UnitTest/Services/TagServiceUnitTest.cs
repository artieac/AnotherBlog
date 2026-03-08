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
    public class TagServiceUnitTest
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<ITagRepository> mockTagRepository;
        private TagService service;
        private Blog testBlog;

        [SetUp]
        public void Setup()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockTagRepository = new Mock<ITagRepository>();

            service = new TagService(mockUnitOfWork.Object, mockTagRepository.Object);

            testBlog = new Blog { Id = 1, Name = "TestBlog", SubFolder = "test" };
        }

        [Test]
        public void GetAll_ReturnsAllTagsForBlog()
        {
            var tags = new List<Tag>
            {
                new Tag { Id = 1, Name = "Tag1", BlogId = testBlog.Id },
                new Tag { Id = 2, Name = "Tag2", BlogId = testBlog.Id }
            };
            mockTagRepository.Setup(x => x.GetAll(testBlog.Id)).Returns(tags);

            var result = service.GetAll(testBlog);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            mockTagRepository.Verify(x => x.GetAll(testBlog.Id), Times.Once);
        }

        [Test]
        public void GetAll_WithNoTags_ReturnsEmptyList()
        {
            mockTagRepository.Setup(x => x.GetAll(testBlog.Id)).Returns(new List<Tag>());

            var result = service.GetAll(testBlog);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetAllWithCount_ReturnsTagsWithCounts()
        {
            var tagsWithCounts = new ArrayList
            {
                new { Name = "Tag1", Count = 5 },
                new { Name = "Tag2", Count = 3 }
            };
            mockTagRepository.Setup(x => x.GetAllWithCount(testBlog.Id)).Returns(tagsWithCounts);

            var result = service.GetAllWithCount(testBlog);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            mockTagRepository.Verify(x => x.GetAllWithCount(testBlog.Id), Times.Once);
        }

        [Test]
        public void GetAllWithCount_WithNoTags_ReturnsEmptyList()
        {
            mockTagRepository.Setup(x => x.GetAllWithCount(testBlog.Id)).Returns(new ArrayList());

            var result = service.GetAllWithCount(testBlog);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}
