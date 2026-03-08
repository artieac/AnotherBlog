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
    public class UserServiceUnitTest
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<IUserRepository> mockUserRepository;
        private Mock<IDisposable> mockTransaction;
        private UserService service;
        private AnotherBlogUser testUser;
        private Blog testBlog;

        [SetUp]
        public void Setup()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUserRepository = new Mock<IUserRepository>();
            mockTransaction = new Mock<IDisposable>();

            mockUnitOfWork.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);

            service = new UserService(mockUnitOfWork.Object, mockUserRepository.Object);

            testUser = new AnotherBlogUser
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                IsSiteAdministrator = false,
                ApprovedCommenter = true,
                About = "About me",
                Roles = new Dictionary<long, RoleType.Id>()
            };

            testBlog = new Blog { Id = 1, Name = "TestBlog", SubFolder = "test" };
        }

        [Test]
        public void Save_WithNewUser_CreatesUser()
        {
            mockUserRepository.Setup(x => x.GetById(0)).Returns((AnotherBlogUser)null);
            mockUserRepository.Setup(x => x.Save(It.IsAny<AnotherBlogUser>())).Returns((AnotherBlogUser u) => u);

            var result = service.Save(0, true, false, "About text");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSiteAdministrator, Is.True);
            Assert.That(result.ApprovedCommenter, Is.False);
            Assert.That(result.About, Is.EqualTo("About text"));
        }

        [Test]
        public void Save_WithExistingUser_UpdatesUser()
        {
            mockUserRepository.Setup(x => x.GetById(1)).Returns(testUser);
            mockUserRepository.Setup(x => x.Save(It.IsAny<AnotherBlogUser>())).Returns((AnotherBlogUser u) => u);

            var result = service.Save(1, true, false, "Updated about");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSiteAdministrator, Is.True);
            Assert.That(result.About, Is.EqualTo("Updated about"));
            mockUserRepository.Verify(x => x.Save(It.IsAny<AnotherBlogUser>()), Times.Once);
        }

        [Test]
        public void Save_WithNullAbout_SetsEmptyString()
        {
            mockUserRepository.Setup(x => x.GetById(1)).Returns(testUser);
            mockUserRepository.Setup(x => x.Save(It.IsAny<AnotherBlogUser>())).Returns((AnotherBlogUser u) => u);

            var result = service.Save(1, false, true, null);

            Assert.That(result.About, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Save_WithUserObject_SavesUser()
        {
            mockUserRepository.Setup(x => x.Save(testUser)).Returns(testUser);

            var result = service.Save(testUser);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(testUser.Id));
            mockUserRepository.Verify(x => x.Save(testUser), Times.Once);
        }

        [Test]
        public void Save_WithNullUserObject_ReturnsNull()
        {
            var result = service.Save((AnotherBlogUser)null);

            Assert.That(result, Is.Null);
            mockUserRepository.Verify(x => x.Save(It.IsAny<AnotherBlogUser>()), Times.Never);
        }

        [Test]
        public void Delete_WithExistingUser_DeletesUser()
        {
            mockUserRepository.Setup(x => x.GetById(1)).Returns(testUser);
            mockUserRepository.Setup(x => x.Delete(testUser)).Returns(true);

            service.Delete(1);

            mockUserRepository.Verify(x => x.Delete(testUser), Times.Once);
            mockUnitOfWork.Verify(x => x.EndTransaction(true), Times.Once);
        }

        [Test]
        public void Delete_WithNonExistingUser_DoesNotDelete()
        {
            mockUserRepository.Setup(x => x.GetById(999)).Returns((AnotherBlogUser)null);

            service.Delete(999);

            mockUserRepository.Verify(x => x.Delete(It.IsAny<AnotherBlogUser>()), Times.Never);
        }

        [Test]
        public void GetAll_ReturnsAllUsers()
        {
            var users = new List<AnotherBlogUser> { testUser, new AnotherBlogUser { Id = 2 } };
            mockUserRepository.Setup(x => x.GetAll()).Returns(users);

            var result = service.GetAll();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetById_ReturnsUser()
        {
            mockUserRepository.Setup(x => x.GetById(1)).Returns(testUser);

            var result = service.GetById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public void GetById_WithInvalidId_ReturnsNull()
        {
            mockUserRepository.Setup(x => x.GetById(999)).Returns((AnotherBlogUser)null);

            var result = service.GetById(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetBlogWriters_ReturnsWritersForBlog()
        {
            var writers = new List<AnotherBlogUser> { testUser };
            mockUserRepository.Setup(x => x.GetBlogWriters(testBlog.Id)).Returns(writers);

            var result = service.GetBlogWriters(testBlog);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddBlogRole_WithValidUser_AddsRole()
        {
            testUser.Roles = new Dictionary<long, RoleType.Id>();
            mockUserRepository.Setup(x => x.GetById(1)).Returns(testUser);
            mockUserRepository.Setup(x => x.Save(It.IsAny<AnotherBlogUser>())).Returns((AnotherBlogUser u) => u);

            var result = service.AddBlogRole(1, testBlog.Id, RoleType.Id.Blogger);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Roles.ContainsKey(testBlog.Id), Is.True);
            Assert.That(result.Roles[testBlog.Id], Is.EqualTo(RoleType.Id.Blogger));
        }

        [Test]
        public void AddBlogRole_WithNonExistingUser_ReturnsNull()
        {
            mockUserRepository.Setup(x => x.GetById(999)).Returns((AnotherBlogUser)null);
            mockUserRepository.Setup(x => x.Save(It.IsAny<AnotherBlogUser>())).Returns((AnotherBlogUser)null);

            var result = service.AddBlogRole(999, testBlog.Id, RoleType.Id.Blogger);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void RemoveBlogRole_WithValidUser_RemovesRole()
        {
            testUser.Roles = new Dictionary<long, RoleType.Id> { { testBlog.Id, RoleType.Id.Blogger } };
            mockUserRepository.Setup(x => x.GetById(1)).Returns(testUser);
            mockUserRepository.Setup(x => x.Save(It.IsAny<AnotherBlogUser>())).Returns((AnotherBlogUser u) => u);

            var result = service.RemoveBlogRole(1, testBlog.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Roles.ContainsKey(testBlog.Id), Is.False);
        }

        [Test]
        public void RemoveBlogRole_WithNonExistingUser_ReturnsNull()
        {
            mockUserRepository.Setup(x => x.GetById(999)).Returns((AnotherBlogUser)null);
            mockUserRepository.Setup(x => x.Save(It.IsAny<AnotherBlogUser>())).Returns((AnotherBlogUser)null);

            var result = service.RemoveBlogRole(999, testBlog.Id);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetByEmail_ReturnsUser()
        {
            mockUserRepository.Setup(x => x.GetByEmail("john@example.com")).Returns(testUser);

            var result = service.GetByEmail("john@example.com");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo("john@example.com"));
        }

        [Test]
        public void GetByEmail_WithInvalidEmail_ReturnsNull()
        {
            mockUserRepository.Setup(x => x.GetByEmail("invalid@example.com")).Returns((AnotherBlogUser)null);

            var result = service.GetByEmail("invalid@example.com");

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetByExternalId_ReturnsUser()
        {
            testUser.OAuthServiceUserId = "auth0|123456";
            mockUserRepository.Setup(x => x.GetByExternalId("auth0|123456")).Returns(testUser);

            var result = service.GetByExternalId("auth0|123456");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.OAuthServiceUserId, Is.EqualTo("auth0|123456"));
        }

        [Test]
        public void GetByExternalId_WithInvalidId_ReturnsNull()
        {
            mockUserRepository.Setup(x => x.GetByExternalId("invalid")).Returns((AnotherBlogUser)null);

            var result = service.GetByExternalId("invalid");

            Assert.That(result, Is.Null);
        }

        [Test]
        public void CreateFromAuth0_WithFullName_CreatesUser()
        {
            mockUserRepository.Setup(x => x.Save(It.IsAny<AnotherBlogUser>())).Returns((AnotherBlogUser u) => u);

            var result = service.CreateFromAuth0("test@example.com", "John Doe", "auth0|123");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo("test@example.com"));
            Assert.That(result.FirstName, Is.EqualTo("John"));
            Assert.That(result.LastName, Is.EqualTo("Doe"));
            Assert.That(result.OAuthServiceUserId, Is.EqualTo("auth0|123"));
        }

        [Test]
        public void CreateFromAuth0_WithSingleName_SetsFirstNameOnly()
        {
            mockUserRepository.Setup(x => x.Save(It.IsAny<AnotherBlogUser>())).Returns((AnotherBlogUser u) => u);

            var result = service.CreateFromAuth0("test@example.com", "John", "auth0|123");

            Assert.That(result.FirstName, Is.EqualTo("John"));
            Assert.That(result.LastName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void CreateFromAuth0_WithNullName_SetsEmptyStrings()
        {
            mockUserRepository.Setup(x => x.Save(It.IsAny<AnotherBlogUser>())).Returns((AnotherBlogUser u) => u);

            var result = service.CreateFromAuth0("test@example.com", null, "auth0|123");

            Assert.That(result.FirstName, Is.EqualTo(string.Empty));
            Assert.That(result.LastName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void CreateFromAuth0_WithNullEmail_SetsEmptyString()
        {
            mockUserRepository.Setup(x => x.Save(It.IsAny<AnotherBlogUser>())).Returns((AnotherBlogUser u) => u);

            var result = service.CreateFromAuth0(null, "John Doe", "auth0|123");

            Assert.That(result.Email, Is.EqualTo(string.Empty));
        }

        [Test]
        public void CreateFromAuth0_WithNullAuth0Id_SetsEmptyString()
        {
            mockUserRepository.Setup(x => x.Save(It.IsAny<AnotherBlogUser>())).Returns((AnotherBlogUser u) => u);

            var result = service.CreateFromAuth0("test@example.com", "John Doe", null);

            Assert.That(result.OAuthServiceUserId, Is.EqualTo(string.Empty));
        }

        [Test]
        public void CreateFromAuth0_SetsDefaultValues()
        {
            mockUserRepository.Setup(x => x.Save(It.IsAny<AnotherBlogUser>())).Returns((AnotherBlogUser u) => u);

            var result = service.CreateFromAuth0("test@example.com", "John Doe", "auth0|123");

            Assert.That(result.IsSiteAdministrator, Is.False);
            Assert.That(result.ApprovedCommenter, Is.False);
            Assert.That(result.About, Is.EqualTo(string.Empty));
        }

        [Test]
        public void UpdateExternalId_WithValidUser_UpdatesId()
        {
            mockUserRepository.Setup(x => x.GetById(1)).Returns(testUser);
            mockUserRepository.Setup(x => x.Save(It.IsAny<AnotherBlogUser>())).Returns((AnotherBlogUser u) => u);

            var result = service.UpdateExternalId(1, "new-external-id");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.OAuthServiceUserId, Is.EqualTo("new-external-id"));
        }

        [Test]
        public void UpdateExternalId_WithNonExistingUser_ReturnsNull()
        {
            mockUserRepository.Setup(x => x.GetById(999)).Returns((AnotherBlogUser)null);

            var result = service.UpdateExternalId(999, "new-external-id");

            Assert.That(result, Is.Null);
            mockUserRepository.Verify(x => x.Save(It.IsAny<AnotherBlogUser>()), Times.Never);
        }
    }
}
