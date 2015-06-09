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
        User testUser;

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
        public void UserTest_GetDefaultUser()
        {
            User defaultUser = Services.UserService.GetDefaultUser();
            Assert.AreEqual(string.Compare(defaultUser.UserName, "Guest", true), 0);
        }

        [Test]
        public void UserService_Create()
        {
            User newUser = Services.UserService.Create();
            Assert.NotNull(newUser);
            Assert.IsTrue(newUser.IsActive);
        }

        [Test]
        public void UserService_IsValidEmail()
        {
            Assert.IsTrue(Services.UserService.IsValidEmail("acorrea@alwaysmoveforward.com"));
            Assert.IsFalse(Services.UserService.IsValidEmail("acorrea.alwaysmoveforward.com"));
        }

        [Test]
        public void UserService_Login()
        {
            User loginUser = Services.UserService.Login("acorrea", "AMFCa1tl1n3");

            Assert.NotNull(loginUser);
            Assert.AreEqual(testUser.UserId, loginUser.UserId);
        }

        [Test]
        public void UserService_CreateTestUser()
        {
            User newUser = this.Services.UserService.GetByUserName("TestUser2");

            if(newUser == null)
            {
                using (this.Services.UnitOfWork.BeginTransaction())
                {
                    newUser = Services.UserService.Save("TestUser2", "TestPassword2", "test2@test.com", -1, false, false, true, "", "Test2");
                    this.Services.UnitOfWork.EndTransaction(true);
                }
            }

            Assert.IsNotNull(newUser);
            Assert.AreEqual(newUser.UserName, "TestUser2");

            using (this.Services.UnitOfWork.BeginTransaction())
            {
                Services.UserService.Delete(newUser.UserId);
                this.Services.UnitOfWork.EndTransaction(true);
            }
        }

        [Test]
        public void UserService_GetAll()
        {
            IList<User> allUsers = Services.UserService.GetAll();

            Assert.IsNotNull(allUsers);
            Assert.Greater(allUsers.Count, 0);
        }

        [Test]
        public void UserService_GetByUserName()
        {
            User targetUser = Services.UserService.GetByUserName(testUser.UserName);

            Assert.NotNull(targetUser);
            Assert.AreEqual(targetUser.UserName, testUser.UserName);
        }

        [Test]
        public void UserService_GetById()
        {
            int testId = 0;

            testId = testUser.UserId;

            User testIdUser = Services.UserService.GetById(testId);

            Assert.IsNotNull(testIdUser);
            Assert.AreEqual(testId, testIdUser.UserId);
        }

        [Test]
        public void UserService_GetByEmail()
        {
            string testEmail = testUser.Email;

            User testEmailUser = Services.UserService.GetByEmail(testEmail);

            Assert.IsNotNull(testEmailUser);
            Assert.AreEqual(testEmailUser.UserId, testUser.UserId);
        }
    }
}
