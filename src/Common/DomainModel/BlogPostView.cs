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
namespace AlwaysMoveForward.AnotherBlog.Common.DomainModel
{
    /// <summary>
    /// Tracks the number of times a blog post has been viewed,
    /// grouped by calendar year and month.
    /// </summary>
    public class BlogPostView
    {
        public long BlogPostId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int TimesViewed { get; set; }
    }
}
