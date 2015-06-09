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
    public interface IBlogEntryRepository : IRepository<BlogPost, int>
    {
        IList<BlogPost> GetAll(bool publishedOnly, int maxResults);
        IList<BlogPost> GetAllByBlog(int blogId, bool publishedOnly, int maxResults, string sortColumn, bool sortAscending);
        IList<BlogPost> GetMostRead(int maxResults);
        IList<BlogPost> GetMostRead(int blogId, int maxResults);
        BlogPost GetByTitle(string blogTitle, int blogId);
        BlogPost GetByDateAndTitle(string blogTitle, DateTime postDate, int blogId);
        IList<BlogPost> GetByTag(int blogId, string tagText, bool publishedOnly);
        IList<BlogPost> GetByTag(int tagId, bool publishedOnly);
        IList<BlogPost> GetByTag(int? blogId, int tagId, bool publishedOnly);
        IList<BlogPost> GetByMonth(DateTime blogDate, bool publishedOnly);
        IList<BlogPost> GetByMonth(DateTime blogDate, int? blogId, bool publishedOnly);
        IList<BlogPost> GetByDate(DateTime blogDate, bool publishedOnly);
        IList<BlogPost> GetByDate(DateTime blogDate, int? blogId, bool publishedOnly);
        BlogPost GetMostRecent(int blogId, bool published);
        BlogPost GetPreviousEntry(int blogId, int currentPostId);
        BlogPost GetNextEntry(int blogId, int currentPostId);
        IList<DateTime> GetPublishedDatesByMonth(DateTime blogDate);
        IList GetArchiveDates(int? blogId);
        BlogPost GetByCommentId(int commentId);
    }
}
