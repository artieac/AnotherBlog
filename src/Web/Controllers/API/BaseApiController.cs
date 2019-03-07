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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Security.Permissions;
using PucksAndProgramming.Common.Utilities;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Common.Factories;
using PucksAndProgramming.AnotherBlog.BusinessLayer.Service;
using PucksAndProgramming.AnotherBlog.BusinessLayer.Utilities;
using PucksAndProgramming.AnotherBlog.Web.Models;
using PucksAndProgramming.AnotherBlog.Web.Models.BlogModels;

namespace PucksAndProgramming.AnotherBlog.Web.Controllers.API
{
    public abstract class BaseApiController : ApiController
    {
        private ServiceManager serviceManager;

        public ServiceManager Services
        {
            get
            {
                try
                {
                    LogManager.GetLogger().Info("Creating Service Manager for BaseAPIController");
                    this.serviceManager = ServiceManagerBuilder.BuildServiceManager();
                    LogManager.GetLogger().Info("Creating Service Manager Complete for BaseAPIController");
                }
                catch (Exception e)
                {
                    LogManager.GetLogger().Error(e);
                }

                return this.serviceManager;
            }
        }

        public SecurityPrincipal CurrentPrincipal
        {
            get 
            {
                SecurityPrincipal retVal = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;

                if (retVal == null)
                {
                    try
                    {
                        retVal = new SecurityPrincipal(UserFactory.CreateGuestUser());
                        System.Threading.Thread.CurrentPrincipal = retVal;
                    }
                    catch (Exception e)
                    {
                        LogManager.GetLogger().Error(e);
                    }
                }

                return retVal;
            }
            set
            {
                System.Threading.Thread.CurrentPrincipal = value;
            }
        }
    }
}