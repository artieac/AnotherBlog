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

namespace AlwaysMoveForward.AnotherBlog.BusinessLayer.Events.BlogEntry
{
    public class BlogEntryEvents
    {
    }

    /// <summary>
    /// Raised when a blog post is viewed. Carries the post ID and the
    /// calendar year/month in which the view occurred so listeners can
    /// bucket the count into BlogPostViews.
    /// </summary>
    public class BlogPostViewedEvent
    {
        public BlogPostViewedEvent(long blogPostId, int year, int month)
        {
            BlogPostId = blogPostId;
            Year = year;
            Month = month;
        }

        public long BlogPostId { get; }
        public int Year { get; }
        public int Month { get; }
    }
}
