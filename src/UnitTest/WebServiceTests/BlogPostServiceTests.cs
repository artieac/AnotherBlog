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
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

using AlwaysMoveForward.AnotherBlog.UnitTest.BlogPostReference;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.WebServiceTests
{
    [TestFixture]
    public class BlogPostServiceTests
    {
        BlogPostServiceClient testClient;

        [SetUp]
        public void Setup()
        {
            testClient = new BlogPostServiceClient();      
        }

        [TearDown]
        public void TearDown()
        {
            testClient = null;
        }

        [Test]
        public void GetBlogsTest()
        {
            GetBlogsRequest request = new GetBlogsRequest();

            IList<AlwaysMoveForward.AnotherBlog.UnitTest.BlogPostReference.BlogElement> foundBlogs = testClient.GetBlogs(request).Blogs;

            Assert.IsNotNull(foundBlogs);
        }

        [Test]
        public void GetBlogEntriesTest()
        {
            GetBlogsRequest blogRequest = new GetBlogsRequest();
            IList<AlwaysMoveForward.AnotherBlog.UnitTest.BlogPostReference.BlogElement> foundBlogs = testClient.GetBlogs(blogRequest).Blogs;

            bool foundEntries = false;

            if (foundBlogs != null)
            {
                for (int i = 0; i < foundBlogs.Count; i++)
                {
                    GetAllBlogEntriesByBlogRequest entryRequest = new GetAllBlogEntriesByBlogRequest();
                    entryRequest.BlogId = foundBlogs[i].BlogId;

                    IList<AlwaysMoveForward.AnotherBlog.UnitTest.BlogPostReference.BlogPostElement> entryList = testClient.GetAllBlogEntriesByBlog(entryRequest).BlogEntries;

                    if (entryList.Count > 0)
                    {
                        foundEntries = true;
                        break;
                    }
                }
            }

            Assert.IsTrue(foundEntries == true);
        }

    }
}
