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
    }
}
