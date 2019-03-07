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
using System.Linq;
using System.Text;
using NUnit.Framework;
using PucksAndProgramming.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.BusinessLayer.Service;

namespace PucksAndProgramming.AnotherBlog.UnitTest.Services
{
    [TestFixture]
    public class BlogTest : ServiceTestBase
    {
        public BlogTest()
            : base()
        {
        }

        [SetUp]
        public void Setup()
        {
            using(this.Services.UnitOfWork.BeginTransaction())
            {
                Services.UserService.AddBlogRole(this.TestUser.Id, this.TestBlog.Id, RoleType.Id.Blogger);
                this.Services.UnitOfWork.EndTransaction(true);
            }
        }

        [Test]
        public void BlogService_Create()
        {
            Blog test = new Blog();
            Services.BlogService.Save(-1, "Test", "Test", "", "", "", "default");
            Assert.IsNotNull(test);
        }

        [Test]
        public void BlogService_GetAll()
        {
            IList<Blog> test = Services.BlogService.GetAll();
            Assert.IsNotNull(test);
            Assert.Greater(test.Count, 0);
        }

        [Test]
        public void BlogService_GetByUserId()
        {
            Assert.IsNotNull(this.TestUser);

            IList<Blog> test = Services.BlogService.GetByUserId(this.TestUser.Id);
            Assert.IsNotNull(test);
        }

        [Test]
        public void BlogService_GetById()
        {
            Assert.IsNotNull(this.TestBlog);

            Blog test = Services.BlogService.GetById(this.TestBlog.Id);
            Assert.IsNotNull(test);
            Assert.AreEqual(test.Id, this.TestBlog.Id);
        }

        [Test]
        public void BlogService_GetByName()
        {
            Assert.IsNotNull(this.TestBlog);

            Blog test = Services.BlogService.GetByName(this.TestBlog.Name);
            Assert.IsNotNull(test);
            Assert.AreEqual(test.Name, this.TestBlog.Name);
        }

        [Test]
        public void BlogService_GetBySubFolder()
        {
            Assert.IsNotNull(this.TestBlog);

            Blog test = Services.BlogService.GetBySubFolder(this.TestBlog.SubFolder);
            Assert.IsNotNull(test);
            Assert.AreEqual(test.Name, this.TestBlog.SubFolder);
        }        
    }
}
