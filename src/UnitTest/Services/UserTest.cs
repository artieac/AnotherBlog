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
    public class UserTest : ServiceTestBase
    {
        AnotherBlogUser testUser;

        public UserTest()
            : base()
        {

        }

        [SetUp]
        public void SetUp()
        {
            testUser = this.Services.UserService.GetById(1);
        }

        [Test]
        public void UserService_CreateTestUser()
        {
            AnotherBlogUser newUser = this.Services.UserService.GetById(this.testUser.Id);

            if(newUser == null)
            {
                using (this.Services.UnitOfWork.BeginTransaction())
                {
                    newUser = Services.UserService.Save(1, false, true, "");
                    this.Services.UnitOfWork.EndTransaction(true);
                }
            }

            Assert.IsNotNull(newUser);
            Assert.AreEqual(newUser.Id, this.testUser.Id);

            using (this.Services.UnitOfWork.BeginTransaction())
            {
                Services.UserService.Delete(newUser.Id);
                this.Services.UnitOfWork.EndTransaction(true);
            }
        }

        [Test]
        public void UserService_GetAll()
        {
            IList<AnotherBlogUser> allUsers = Services.UserService.GetAll();

            Assert.IsNotNull(allUsers);
            Assert.Greater(allUsers.Count, 0);
        }

        [Test]
        public void UserService_GetById()
        {
            long testId = 0;

            testId = this.TestUser.Id;

            AnotherBlogUser testIdUser = Services.UserService.GetById(testId);

            Assert.IsNotNull(testIdUser);
            Assert.AreEqual(testId, testIdUser.Id);
        }
    }
}
