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
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories
{
    /// <summary>
    /// Repository for the BlogPostViews table which tracks per-month
    /// view counts for each blog post.
    /// </summary>
    public interface IBlogPostViewRepository
    {
        /// <summary>
        /// Records a view for the given post in the current year/month.
        /// Creates a new row with TimesViewed=1 if one does not exist,
        /// otherwise increments the existing row by 1.
        /// </summary>
        void IncrementView(long blogPostId, int year, int month);

        /// <summary>
        /// Returns the sum of TimesViewed across all months for the given post.
        /// </summary>
        long GetTotalViews(long blogPostId);
    }
}
