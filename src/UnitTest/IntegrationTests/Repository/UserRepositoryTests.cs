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
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.IntegrationTests.Repository
{
    [TestFixture]
    public class UserRepositoryTests : RepositoryTestBase
    {
        [Test]
        public void UserRepository_GetAllTest()
        {
            IList<AnotherBlogUser> foundQuestions = this.RepositoryManager.UserRepository.GetAll();

            Assert.IsNotNull(foundQuestions);
            Assert.IsTrue(foundQuestions.Count > 0);
        }

        [Test]
        public void UserRepository_SaveTest()
        {
            AnotherBlogUser testItem = this.RepositoryManager.UserRepository.GetByUserName("test");

            if (testItem == null)
            {
                testItem = new AnotherBlogUser();
                testItem.FirstName = "test";

                testItem = this.RepositoryManager.UserRepository.Save(testItem);
            }

            Assert.IsNotNull(testItem);
            Assert.IsTrue(testItem.Id > 0);
        }

        [Test]
        public void UserRepository_UpdateTest()
        {
            AnotherBlogUser targetItem = this.RepositoryManager.UserRepository.GetById(1);

            AnotherBlogUser savedItem = this.RepositoryManager.UserRepository.Save(targetItem);

            Assert.IsNotNull(savedItem);
        }

        [Test]
        public void UserRepository_Delete()
        {
            AnotherBlogUser targetItem = this.RepositoryManager.UserRepository.GetById(1);

            AnotherBlogUser savedItem = this.RepositoryManager.UserRepository.Save(targetItem);

            Assert.IsNotNull(savedItem);
        }

        [Test]
        public void UserRepository_GetAllByProperty()
        {
            AnotherBlogUser targetItem = this.RepositoryManager.UserRepository.GetById(1);

            AnotherBlogUser savedItem = this.RepositoryManager.UserRepository.Save(targetItem);

            Assert.IsNotNull(savedItem);
        }

        [Test]
        public void UserRepository_GetBlogWriters()
        {
            AnotherBlogUser targetItem = this.RepositoryManager.UserRepository.GetById(1);

            AnotherBlogUser savedItem = this.RepositoryManager.UserRepository.Save(targetItem);

            Assert.IsNotNull(savedItem);
        }

        [Test]
        public void UserRepository_GetByEmail()
        {
            AnotherBlogUser targetItem = this.RepositoryManager.UserRepository.GetById(1);

            AnotherBlogUser savedItem = this.RepositoryManager.UserRepository.Save(targetItem);

            Assert.IsNotNull(savedItem);
        }

        [Test]
        public void UserRepository_GetByProperty()
        {
            User targetItem = this.RepositoryManager.UserRepository.GetById(1);

            User savedItem = this.RepositoryManager.UserRepository.Save(targetItem);

            Assert.IsNotNull(savedItem);
        }

        [Test]
        public void UserRepository_GetByUserName()
        {
            AnotherBlogUser targetItem = this.RepositoryManager.UserRepository.GetById(1);

            AnotherBlogUser savedItem = this.RepositoryManager.UserRepository.Save(targetItem);

            Assert.IsNotNull(savedItem);
        }

        [Test]
        public void UserRepository_GetByUserNameAndPassword()
        {
            AnotherBlogUser targetItem = this.RepositoryManager.UserRepository.GetById(1);

            AnotherBlogUser savedItem = this.RepositoryManager.UserRepository.Save(targetItem);

            Assert.IsNotNull(savedItem);
        }



    }
}
