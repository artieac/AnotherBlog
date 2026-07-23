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

namespace AlwaysMoveForward.AnotherBlog.Common
{
    public class EventManager
    {
        // -------------------------------------------------------
        // Existing: Blog entry published event
        // -------------------------------------------------------
        public delegate void PublishBlogEntry(int blotEntryId);
        public static event PublishBlogEntry EntryPublishedSubscribers;

        public static void FirePublishBlogEntryEvent(int blogId)
        {
            if (EntryPublishedSubscribers != null)
            {
                EntryPublishedSubscribers(blogId);
            }
        }

        public static void SubscribeToEntryPublish(PublishBlogEntry eventHandler)
        {
            EntryPublishedSubscribers += eventHandler;
        }

        public static void UnsubscribeFromEntryPublish(PublishBlogEntry eventHandler)
        {
            EntryPublishedSubscribers -= eventHandler;
        }

        // -------------------------------------------------------
        // Blog post viewed event
        // Fired each time a visitor views a post. Carries the post
        // ID and the year/month so listeners can update BlogPostViews.
        // -------------------------------------------------------
        public delegate void BlogPostViewed(long blogPostId, int year, int month);
        public static event BlogPostViewed PostViewedSubscribers;

        public static void FireBlogPostViewedEvent(long blogPostId, int year, int month)
        {
            if (PostViewedSubscribers != null)
            {
                PostViewedSubscribers(blogPostId, year, month);
            }
        }

        public static void SubscribeToPostViewed(BlogPostViewed eventHandler)
        {
            PostViewedSubscribers += eventHandler;
        }

        public static void UnsubscribeFromPostViewed(BlogPostViewed eventHandler)
        {
            PostViewedSubscribers -= eventHandler;
        }
    }
}
