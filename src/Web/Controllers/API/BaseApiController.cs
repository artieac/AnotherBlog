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
using Microsoft.AspNetCore.Mvc;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.Factories;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers.API;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    private ServiceManager _serviceManager;

    public ServiceManager Services
    {
        get
        {
            try
            {
                LogManager.GetLogger().Info("Creating Service Manager for BaseAPIController");
                _serviceManager = ServiceManagerBuilder.BuildServiceManager();
                LogManager.GetLogger().Info("Creating Service Manager Complete for BaseAPIController");
            }
            catch (Exception e)
            {
                LogManager.GetLogger().Error(e);
            }

            return _serviceManager;
        }
    }

    public SecurityPrincipal CurrentPrincipal
    {
        get
        {
            SecurityPrincipal retVal = HttpContext.Items["CurrentPrincipal"] as SecurityPrincipal;

            if (retVal == null)
            {
                try
                {
                    retVal = new SecurityPrincipal(UserFactory.CreateGuestUser());
                    HttpContext.Items["CurrentPrincipal"] = retVal;
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
            HttpContext.Items["CurrentPrincipal"] = value;
        }
    }
}
