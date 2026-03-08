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
        RoleType.Id testRole;

        public BlogUserTest()
            : base()
        {

        }

        [SetUp]
        public void Setup()
        {
            testRole = RoleType.Id.Administrator;
        }

        bool HasRole(AnotherBlogUser testUser, RoleType.Id roleType)
        {
            bool retVal = false;

            if (testUser != null)
            {
                if(testUser.Roles!=null)
                {
                    for(int i = 0; i < testUser.Roles.Count; i++)
                    {
                        if(testUser.Roles[i]==roleType)
                        {
                            retVal = true;
                            break;
                        }
                    }
                }
            }

            return retVal;
        }

        [Test]
        public void BlogUserService_Save()
        {
            Assert.That(this.TestBlog, Is.Not.Null);
            Assert.That(this.TestUser, Is.Not.Null);

            AnotherBlogUser testUser = Services.UserService.AddBlogRole(this.TestUser.Id, this.TestBlog.Id, this.testRole);

            Assert.That(this.HasRole(testUser, testRole), Is.True);

            Services.UserService.RemoveBlogRole(this.TestUser.Id, this.TestBlog.Id);
        }

        [Test]
        public void BlogUserService_GetUserBlog()
        {
            Assert.That(this.TestBlog, Is.Not.Null);
            Assert.That(this.TestUser, Is.Not.Null);

            AnotherBlogUser testUser = this.Services.UserService.AddBlogRole(this.TestUser.Id, this.TestBlog.Id, testRole);

            Assert.That(this.HasRole(this.TestUser, testRole), Is.True);

            Services.UserService.RemoveBlogRole(this.TestUser.Id, this.TestBlog.Id);
        }

        [Test]
        public void BlogUserService_DeleteUserBlog()
        {
            Assert.That(this.TestBlog, Is.Not.Null);
            Assert.That(this.TestUser, Is.Not.Null);

            AnotherBlogUser testUser = this.Services.UserService.AddBlogRole(this.TestUser.Id, this.TestBlog.Id, testRole);

            Assert.That(this.HasRole(this.TestUser, testRole), Is.True);

            Services.UserService.RemoveBlogRole(this.TestUser.Id, this.TestBlog.Id);

            Assert.That(this.HasRole(this.TestUser, testRole), Is.False);
        }
    }
}
