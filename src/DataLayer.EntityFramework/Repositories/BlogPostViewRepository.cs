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
using System.Linq;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class BlogPostViewRepository : IBlogPostViewRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        internal BlogPostViewRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private Entities.AnotherBlogDataContextCF DataContext =>
            ((UnitOfWork)_unitOfWork).DataContext;

        /// <inheritdoc/>
        public void IncrementView(long blogPostId, int year, int month)
        {
            var existing = DataContext.BlogPostViews
                .FirstOrDefault(v => v.BlogPostId == blogPostId &&
                                     v.Year == year &&
                                     v.Month == month);

            if (existing == null)
            {
                DataContext.BlogPostViews.Add(new BlogPostView
                {
                    BlogPostId = blogPostId,
                    Year = year,
                    Month = month,
                    TimesViewed = 1
                });
            }
            else
            {
                existing.TimesViewed++;
            }

            DataContext.SaveChanges();

            // Remove any records for this post older than 13 months.
            // Cutoff: the first day of the month that is 12 months before now,
            // which gives a rolling 13-month window (current month + 12 prior).
            var cutoff = new DateTime(year, month, 1).AddMonths(-12);
            int cutoffYear = cutoff.Year;
            int cutoffMonth = cutoff.Month;

            var staleViews = DataContext.BlogPostViews
                .Where(v => v.BlogPostId == blogPostId &&
                            (v.Year < cutoffYear ||
                             (v.Year == cutoffYear && v.Month < cutoffMonth)))
                .ToList();

            if (staleViews.Count > 0)
            {
                DataContext.BlogPostViews.RemoveRange(staleViews);
                DataContext.SaveChanges();
            }
        }

        /// <inheritdoc/>
        public long GetTotalViews(long blogPostId)
        {
            return DataContext.BlogPostViews
                .Where(v => v.BlogPostId == blogPostId)
                .Sum(v => (long?)v.TimesViewed) ?? 0L;
        }
    }
}
