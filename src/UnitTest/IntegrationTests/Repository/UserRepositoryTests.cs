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
using AlwaysMoveForward.AnotherBlog.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.IntegrationTests.Repository
{
    [TestFixture]
    public class UserRepositoryTests : RepositoryTestBase
    {
        [Test]
        public void UserRepository_GetAllTest()
        {
            IList<User> foundQuestions = this.RepositoryManager.Users.GetAll();

            Assert.IsNotNull(foundQuestions);
            Assert.IsTrue(foundQuestions.Count > 0);
        }

        [Test]
        public void UserRepository_SaveTest()
        {
            User testItem = this.RepositoryManager.Users.GetByUserName("test");

            if (testItem == null)
            {
                testItem = new User();
                testItem.UserName = "test";
                testItem.Password = "test";
                testItem.Email = "test";

                testItem = this.RepositoryManager.Users.Save(testItem);
            }

            Assert.IsNotNull(testItem);
            Assert.IsTrue(testItem.UserId > 0);
        }

        [Test]
        public void UserRepository_UpdateTest()
        {
            User targetItem = this.RepositoryManager.Users.GetById(1);

            User savedItem = this.RepositoryManager.Users.Save(targetItem);

            Assert.IsNotNull(savedItem);
        }

        [Test]
        public void UserRepository_Delete()
        {
            User targetItem = this.RepositoryManager.Users.GetById(1);

            User savedItem = this.RepositoryManager.Users.Save(targetItem);

            Assert.IsNotNull(savedItem);
        }

        [Test]
        public void UserRepository_GetAllByProperty()
        {
            User targetItem = this.RepositoryManager.Users.GetById(1);

            User savedItem = this.RepositoryManager.Users.Save(targetItem);

            Assert.IsNotNull(savedItem);
        }

        [Test]
        public void UserRepository_GetBlogWriters()
        {
            User targetItem = this.RepositoryManager.Users.GetById(1);

            User savedItem = this.RepositoryManager.Users.Save(targetItem);

            Assert.IsNotNull(savedItem);
        }

        [Test]
        public void UserRepository_GetByEmail()
        {
            User targetItem = this.RepositoryManager.Users.GetById(1);

            User savedItem = this.RepositoryManager.Users.Save(targetItem);

            Assert.IsNotNull(savedItem);
        }

        [Test]
        public void UserRepository_GetByProperty()
        {
            User targetItem = this.RepositoryManager.Users.GetById(1);

            User savedItem = this.RepositoryManager.Users.Save(targetItem);

            Assert.IsNotNull(savedItem);
        }

        [Test]
        public void UserRepository_GetByUserName()
        {
            User targetItem = this.RepositoryManager.Users.GetById(1);

            User savedItem = this.RepositoryManager.Users.Save(targetItem);

            Assert.IsNotNull(savedItem);
        }

        [Test]
        public void UserRepository_GetByUserNameAndPassword()
        {
            User targetItem = this.RepositoryManager.Users.GetById(1);

            User savedItem = this.RepositoryManager.Users.Save(targetItem);

            Assert.IsNotNull(savedItem);
        }



    }
}
