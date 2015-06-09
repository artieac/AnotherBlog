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
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.Services
{
    [TestFixture]
    public class BlogUserTest : ServiceTestBase
    {
        Role testRole;

        public BlogUserTest()
            : base()
        {

        }

        [SetUp]
        public void Setup()
        {
            testRole = Services.RoleService.GetById(1);
        }

        [Test]
        public void BlogUserService_Create()
        {
            BlogUser test = Services.BlogUserService.Create();
            Assert.IsNotNull(test);
        }

        [Test]
        public void BlogUserService_Save()
        {
            Assert.IsNotNull(this.TestBlog);
            Assert.IsNotNull(this.TestUser);
            Assert.IsNotNull(this.testRole);

            BlogUser test = Services.BlogUserService.Save(this.TestUser.UserId, this.TestBlog.BlogId, testRole.RoleId);

            Assert.IsNotNull(test);

            Services.BlogUserService.DeleteUserBlog(this.TestBlog.BlogId, this.TestUser.UserId);
        }

        [Test]
        public void BlogUserService_GetUserBlog()
        {
            Assert.IsNotNull(this.TestBlog);
            Assert.IsNotNull(this.TestUser);
            Assert.IsNotNull(this.testRole);

            BlogUser test = Services.BlogUserService.Save(this.TestUser.UserId, this.TestBlog.BlogId, testRole.RoleId);
            test = Services.BlogUserService.GetUserBlog(this.TestUser.UserId, this.TestBlog.BlogId);

            if(test==null)
            {
                test = Services.BlogUserService.Save(this.TestUser.UserId, this.TestBlog.BlogId, testRole.RoleId);
                test = Services.BlogUserService.GetUserBlog(this.TestUser.UserId, this.TestBlog.BlogId);
            }

            Assert.IsNotNull(test);

            Services.BlogUserService.DeleteUserBlog(this.TestBlog.BlogId, this.TestUser.UserId);
        }

        [Test]
        public void BlogUserService_GetUserBlogs()
        {
            Assert.IsNotNull(this.TestUser);

            BlogUser test = Services.BlogUserService.Save(this.TestUser.UserId, this.TestBlog.BlogId, testRole.RoleId);

            IList<BlogUser> testList = Services.BlogUserService.GetUserBlogs(this.TestUser.UserId);
            Assert.IsNotNull(testList);

            Services.BlogUserService.DeleteUserBlog(test);
        }

        [Test]
        public void BlogUserService_DeleteUserBlog()
        {
            Assert.IsNotNull(this.TestBlog);
            Assert.IsNotNull(this.TestUser);

            BlogUser test = Services.BlogUserService.GetUserBlog(this.TestUser.UserId, this.TestBlog.BlogId);

            if (test == null)
            {
                test = Services.BlogUserService.Save(this.TestUser.UserId, this.TestBlog.BlogId, testRole.RoleId);
            }

            Assert.IsNotNull(test);

            Services.BlogUserService.DeleteUserBlog(this.TestBlog.BlogId, this.TestUser.UserId);

            test = Services.BlogUserService.GetUserBlog(this.TestUser.UserId, this.TestBlog.BlogId);

            Assert.IsNull(test);
        }
    }
}
