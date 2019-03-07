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
using PucksAndProgramming.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Common.DataLayer.Repositories;
using PucksAndProgramming.AnotherBlog.BusinessLayer.Service;

namespace PucksAndProgramming.AnotherBlog.UnitTest.Services
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
                    this.services = PucksAndProgramming.AnotherBlog.UnitTest.Services.ServiceManagerBuilder.BuildServiceManager();
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

        public AnotherBlogUser TestUser
        {
            get
            {
                AnotherBlogUser retVal = Services.UserService.GetById(1);

                if (retVal == null)
                {
                    using (this.Services.UnitOfWork.BeginTransaction())
                    {
                        retVal = Services.UserService.Save(1, false, true, "");
                        this.Services.UnitOfWork.EndTransaction(true);
                    }
                }

                System.Threading.Thread.CurrentPrincipal = new PucksAndProgramming.AnotherBlog.BusinessLayer.Utilities.SecurityPrincipal(retVal, true);

                return retVal;
            }
        }
    }
}
