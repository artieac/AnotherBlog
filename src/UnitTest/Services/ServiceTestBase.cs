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
using Moq;
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.Services
{
    public class ServiceTestBase
    {
        ServiceManager services;

        public ServiceTestBase()
        {
        }

        public ServiceManager Services
        {
            get
            {
                if (services == null)
                {
                    this.services = AlwaysMoveForward.AnotherBlog.UnitTest.Services.ServiceManagerBuilder.BuildServiceManager();
                }

                return services;
            }
        }

        public Blog TestBlog
        {
            get
            {
                Blog retVal = Services.BlogService.GetBySubFolder("TestBlog");

                if (retVal == null)
                {
                    using(this.Services.UnitOfWork.BeginTransaction())
                    {
                        retVal = Services.BlogService.Save(-1, "TestBlog", "TestBlog", "TestBlog", "", "TestBlog", "");
                        this.Services.UnitOfWork.EndTransaction(true);
                    }
                }

                return retVal;
            }
        }

        public User TestUser
        {
            get
            {
                User retVal = Services.UserService.GetByUserName("TestUser");

                if (retVal == null)
                {
                    using (this.Services.UnitOfWork.BeginTransaction())
                    {
                        retVal = Services.UserService.Save("TestUser", "Password", "testuser@alwaysmoveforward.com", -1, false, false, true, "", "");
                        this.Services.UnitOfWork.EndTransaction(true);
                    }
                }

                System.Threading.Thread.CurrentPrincipal = new AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities.SecurityPrincipal(retVal, true);

                return retVal;
            }
        }
    }
}
