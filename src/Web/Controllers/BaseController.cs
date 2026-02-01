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
using Microsoft.AspNetCore.Mvc.Filters;
using X.PagedList;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.Factories;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;
using AlwaysMoveForward.AnotherBlog.Web.Models;
using AlwaysMoveForward.AnotherBlog.Web.Models.BlogModels;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers;

public abstract class BaseController : Controller
{
    private readonly ServiceManagerBuilder _serviceManagerBuilder;
    private ServiceManager _serviceManager;

    protected BaseController(ServiceManagerBuilder serviceManagerBuilder)
    {
        _serviceManagerBuilder = serviceManagerBuilder;
    }

    public ServiceManager Services
    {
        get
        {
            if (_serviceManager == null)
            {
                _serviceManager = _serviceManagerBuilder.CreateServiceManager();
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
                    retVal = new SecurityPrincipal(Services, UserFactory.CreateGuestUser());
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
            HttpContext.User = value;
        }
    }

    public X.PagedList.IPagedList<BlogPostModel> PopulateBlogPostInfo(IList<BlogPost> blogPosts, int currentPageIndex)
    {
        // X.PagedList uses 1-based page numbers, so add 1 to the 0-based index
        return X.PagedList.Extensions.PagedListExtensions.ToPagedList(this.PopulateBlogPostInfo(blogPosts), currentPageIndex + 1, Constants.PageSize);
    }

    public IList<BlogPostModel> PopulateBlogPostInfo(IList<BlogPost> blogPosts)
    {
        IList<BlogPostModel> retVal = new List<BlogPostModel>();

        if (blogPosts != null)
        {
            for (int i = 0; i < blogPosts.Count; i++)
            {
                BlogPostModel blogPost = new BlogPostModel();
                blogPost.Post = blogPosts[i];
                blogPost.Author = blogPosts[i].Author;
                blogPost.Tags = blogPosts[i].Tags;
                retVal.Add(blogPost);
            }
        }

        return retVal;
    }
}
