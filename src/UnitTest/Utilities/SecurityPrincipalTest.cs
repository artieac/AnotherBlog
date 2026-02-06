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
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.Utilities
{
    [TestFixture]
    public class SecurityPrincipalTest
    {
        private AnotherBlogUser testUser;
        private Blog testBlog;

        [SetUp]
        public void Setup()
        {
            testUser = new AnotherBlogUser
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                IsSiteAdministrator = false,
                ApprovedCommenter = true,
                Roles = new Dictionary<long, RoleType.Id>()
            };

            testBlog = new Blog { Id = 1, Name = "TestBlog", SubFolder = "test" };
        }

        [Test]
        public void Constructor_WithUser_SetsCurrentUser()
        {
            var principal = new SecurityPrincipal(null, testUser);

            Assert.That(principal.CurrentUser, Is.EqualTo(testUser));
        }

        [Test]
        public void Constructor_WithIsAuthenticated_SetsAuthenticatedState()
        {
            var principal = new SecurityPrincipal(null, testUser, true);

            Assert.That(principal.IsAuthenticated, Is.True);
        }

        [Test]
        public void Constructor_WithoutIsAuthenticated_DefaultsToNotAuthenticated()
        {
            var principal = new SecurityPrincipal(null, testUser);

            Assert.That(principal.IsAuthenticated, Is.False);
        }

        [Test]
        public void Name_ReturnsUserDisplayName()
        {
            var principal = new SecurityPrincipal(null, testUser, true);

            Assert.That(principal.Name, Is.Not.Null);
        }

        [Test]
        public void IsInRole_WithSiteAdmin_ReturnsTrueForSiteAdmin()
        {
            testUser.IsSiteAdministrator = true;
            var principal = new SecurityPrincipal(null, testUser);

            var result = principal.IsInRole(RoleType.Names.SiteAdministrator);

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsInRole_WithoutSiteAdmin_ReturnsFalseForSiteAdmin()
        {
            testUser.IsSiteAdministrator = false;
            var principal = new SecurityPrincipal(null, testUser);

            var result = principal.IsInRole(RoleType.Names.SiteAdministrator);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsInRole_WithBloggerRole_ReturnsTrueForBlogger()
        {
            testUser.Roles.Add(testBlog.Id, RoleType.Id.Blogger);
            var principal = new SecurityPrincipal(null, testUser);

            var result = principal.IsInRole(RoleType.Id.Blogger.ToString());

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsInRole_WithoutBloggerRole_ReturnsFalseForBlogger()
        {
            var principal = new SecurityPrincipal(null, testUser);

            var result = principal.IsInRole(RoleType.Id.Blogger.ToString());

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsInRole_WithNullUser_ReturnsFalse()
        {
            var principal = new SecurityPrincipal(null, null);

            var result = principal.IsInRole(RoleType.Names.SiteAdministrator);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsInRole_WithMultipleRoles_FindsCorrectRole()
        {
            testUser.Roles.Add(1, RoleType.Id.Blogger);
            testUser.Roles.Add(2, RoleType.Id.Administrator);
            testUser.Roles.Add(3, RoleType.Id.Reader);
            var principal = new SecurityPrincipal(null, testUser);

            Assert.That(principal.IsInRole(RoleType.Id.Blogger.ToString()), Is.True);
            Assert.That(principal.IsInRole(RoleType.Id.Administrator.ToString()), Is.True);
            Assert.That(principal.IsInRole(RoleType.Id.Reader.ToString()), Is.True);
        }

        [Test]
        public void IsInRole_ArrayOverload_WithSiteAdminRole_ReturnsTrue()
        {
            testUser.IsSiteAdministrator = true;
            var principal = new SecurityPrincipal(null, testUser);
            var roles = new[] { RoleType.Names.SiteAdministrator, RoleType.Names.Blogger };

            var result = principal.IsInRole(roles, testBlog);

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsInRole_ArrayOverload_WithMatchingBlogRole_ReturnsTrue()
        {
            testUser.Roles.Add(testBlog.Id, RoleType.Id.Blogger);
            var principal = new SecurityPrincipal(null, testUser);
            var roles = new[] { RoleType.Id.Blogger.ToString() };

            var result = principal.IsInRole(roles, testBlog);

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsInRole_ArrayOverload_WithNoMatchingRole_ReturnsFalse()
        {
            var principal = new SecurityPrincipal(null, testUser);
            var roles = new[] { RoleType.Id.Administrator.ToString() };

            var result = principal.IsInRole(roles, testBlog);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsInRole_ArrayOverload_WithNullBlog_ReturnsFalse()
        {
            testUser.Roles.Add(testBlog.Id, RoleType.Id.Blogger);
            var principal = new SecurityPrincipal(null, testUser);
            var roles = new[] { RoleType.Id.Blogger.ToString() };

            var result = principal.IsInRole(roles, null);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsInRole_ArrayOverload_WithNullUser_ReturnsFalse()
        {
            var principal = new SecurityPrincipal(null, null);
            var roles = new[] { RoleType.Names.SiteAdministrator };

            var result = principal.IsInRole(roles, testBlog);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsInRole_ArrayOverload_WithNullRoles_ReturnsFalse()
        {
            var principal = new SecurityPrincipal(null, testUser);

            var result = principal.IsInRole((string[])null, testBlog);

            Assert.That(result, Is.False);
        }
    }
}
