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
using System.Data;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;

namespace AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories
{
    public interface IAnotherBlogRepositoryManager
    {
        IDbInfoRepository DbInfo { get; }
        ISiteInfoRepository SiteInfo { get; }
        IBlogEntryRepository BlogEntries { get; }
        IBlogRepository Blogs { get; }
        ITagRepository Tags { get; }
        IBlogListRepository BlogLists { get; }
        IPollRepository PollRepository { get; }
        IUserRepository UserRepository { get; }
    }
}
