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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories
{
    public interface IBlogEntryRepository : IRepository<BlogPost, long>
    {
        IList<BlogPost> GetAll(bool publishedOnly, int maxResults);
        IList<BlogPost> GetAllByBlog(long blogId, bool publishedOnly, int maxResults, string sortColumn, bool sortAscending);
        IList<BlogPost> GetMostRead(int maxResults);
        IList<BlogPost> GetMostRead(long blogId, int maxResults);
        BlogPost GetByTitle(string blogTitle, long blogId);
        BlogPost GetByDateAndTitle(string blogTitle, DateTime postDate, long blogId);
        IList<BlogPost> GetByTag(long blogId, string tagText, bool publishedOnly);
        IList<BlogPost> GetByTag(long tagId, bool publishedOnly);
        IList<BlogPost> GetByTag(long? blogId, long tagId, bool publishedOnly);
        IList<BlogPost> GetByMonth(DateTime blogDate, bool publishedOnly);
        IList<BlogPost> GetByMonth(DateTime blogDate, long? blogId, bool publishedOnly);
        IList<BlogPost> GetByDate(DateTime blogDate, bool publishedOnly);
        IList<BlogPost> GetByDate(DateTime blogDate, long? blogId, bool publishedOnly);
        BlogPost GetMostRecent(long blogId, bool published);
        BlogPost GetPreviousEntry(long blogId, long currentPostId);
        BlogPost GetNextEntry(long blogId, long currentPostId);
        IList<DateTime> GetPublishedDatesByMonth(DateTime blogDate);
        IList GetArchiveDates(long? blogId);
        BlogPost GetByCommentId(long commentId);
    }
}
