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
using AlwaysMoveForward.AnotherBlog.Common;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;

namespace AlwaysMoveForward.AnotherBlog.BusinessLayer.Events.BlogEntry
{
    /// <summary>
    /// Listens for <see cref="EventManager.PostViewedSubscribers"/> and
    /// records the view in the BlogPostViews table for the given year/month.
    ///
    /// Accepts a factory delegate rather than a repository instance so that
    /// each event invocation obtains a fresh, request-scoped repository —
    /// avoiding stale or disposed DbContext references when the listener
    /// is registered once for the lifetime of the application.
    /// </summary>
    public class BlogPostViewedEventListener
    {
        private readonly Func<IBlogPostViewRepository> _repositoryFactory;

        /// <summary>
        /// Constructs the listener with a factory that produces a new
        /// <see cref="IBlogPostViewRepository"/> on each call.
        /// </summary>
        public BlogPostViewedEventListener(Func<IBlogPostViewRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        /// <summary>
        /// Registers this listener with the <see cref="EventManager"/> so it
        /// begins receiving post-viewed events. Call once at application startup.
        /// </summary>
        public void Register()
        {
            EventManager.SubscribeToPostViewed(OnPostViewed);
        }

        /// <summary>
        /// Unregisters this listener. Call during application shutdown to
        /// avoid memory leaks from the static event.
        /// </summary>
        public void Unregister()
        {
            EventManager.UnsubscribeFromPostViewed(OnPostViewed);
        }

        private void OnPostViewed(long blogPostId, int year, int month)
        {
            // Create a fresh repository for this event so we never use a
            // stale or disposed DbContext from a previous HTTP request.
            var repository = _repositoryFactory();
            repository.IncrementView(blogPostId, year, month);
        }
    }
}
