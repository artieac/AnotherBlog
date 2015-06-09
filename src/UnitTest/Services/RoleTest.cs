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
    public class RoleTest : ServiceTestBase
    {
        public RoleTest()
            : base()
        {

        }

        [SetUp]
        public void SetUp()
        {

        }

        [TearDown]
        public void Teardown()
        {

        }

        [Test]
        public void RoleService_GetDefaultRole()
        {
            Role testRole = Services.RoleService.GetDefaultRole();
            Assert.IsNotNull(testRole);
        }

        [Test]
        public void RoleService_GetAll()
        {
            IList<Role> testRoles = Services.RoleService.GetAll();

            Assert.IsNotNull(testRoles);
            Assert.Greater(testRoles.Count, 0);
        }

        [Test]
        public void RoleService_GetById()
        {
            Role testRole = Services.RoleService.GetById(3);
            Assert.IsNotNull(testRole);
        }
    }
}
