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
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.Services
{
    [TestFixture]
    public class UploadedFileManagerTest
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private UploadedFileManager manager;
        private Blog testBlog;

        [SetUp]
        public void Setup()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            manager = new UploadedFileManager(mockUnitOfWork.Object);

            testBlog = new Blog { Id = 1, Name = "TestBlog", SubFolder = "test" };
        }

        [Test]
        public void UploadedFileRoot_ReturnsCorrectPath()
        {
            var result = manager.UploadedFileRoot(testBlog);

            Assert.That(result, Is.EqualTo("Content/UploadedFiles/test"));
        }

        [Test]
        public void UploadedFileRoot_WithDifferentSubFolder_ReturnsCorrectPath()
        {
            testBlog.SubFolder = "myblog";

            var result = manager.UploadedFileRoot(testBlog);

            Assert.That(result, Is.EqualTo("Content/UploadedFiles/myblog"));
        }

        [Test]
        public void GeneratePath_ContainsBaseDirectory()
        {
            var result = manager.GeneratePath(testBlog);

            Assert.That(result, Does.Contain(AppDomain.CurrentDomain.BaseDirectory));
        }

        [Test]
        public void GeneratePath_ContainsUploadedFileRoot()
        {
            var result = manager.GeneratePath(testBlog);

            Assert.That(result, Does.Contain("Content/UploadedFiles/test"));
        }

        [Test]
        public void GeneratePath_ContainsYearAndMonth()
        {
            var now = DateTime.Now;

            var result = manager.GeneratePath(testBlog);

            Assert.That(result, Does.Contain($"/{now.Year}/{now.Month}"));
        }

        [Test]
        public void GetRecentUploadedFiles_LocalPaths_ReturnsListWhenDirectoryDoesNotExist()
        {
            // When directories don't exist, should return empty list
            var result = manager.GetRecentUploadedFiles_LocalPaths(testBlog);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<System.Collections.Generic.List<string>>());
        }

        [Test]
        public void GetRecentUploadedFiles_RelativePaths_ReturnsListWhenDirectoryDoesNotExist()
        {
            // When directories don't exist, should return empty list
            var result = manager.GetRecentUploadedFiles_RelativePaths(testBlog);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<System.Collections.Generic.List<string>>());
        }
    }
}
