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
    public class CommentServiceTest
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<IEntryCommentRepository> mockCommentRepository;
        private CommentService service;
        private Blog testBlog;

        [SetUp]
        public void Setup()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockCommentRepository = new Mock<IEntryCommentRepository>();

            service = new CommentService(mockUnitOfWork.Object, mockCommentRepository.Object);

            testBlog = new Blog { Id = 1, Name = "TestBlog", SubFolder = "test" };
        }

        [Test]
        public void GetByBlogAndPostId_WithValidBlog_ReturnsComments()
        {
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Text = "Comment 1" },
                new Comment { Id = 2, Text = "Comment 2" }
            };
            mockCommentRepository.Setup(x => x.GetByEntry(1, testBlog.Id)).Returns(comments);

            var result = service.GetByBlogAndPostId(testBlog, 1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            mockCommentRepository.Verify(x => x.GetByEntry(1, testBlog.Id), Times.Once);
        }

        [Test]
        public void GetByBlogAndPostId_WithNullBlog_ReturnsEmptyList()
        {
            var result = service.GetByBlogAndPostId(null, 1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
            mockCommentRepository.Verify(x => x.GetByEntry(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void GetByBlogAndPostId_WithNoComments_ReturnsEmptyList()
        {
            mockCommentRepository.Setup(x => x.GetByEntry(1, testBlog.Id)).Returns(new List<Comment>());

            var result = service.GetByBlogAndPostId(testBlog, 1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}
