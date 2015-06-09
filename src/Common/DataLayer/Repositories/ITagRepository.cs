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
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories
{
    public interface ITagRepository : IRepository<Tag, int>
    {
        IList<Tag> GetAll(int blogId);
        IList GetAllWithCount(int? blogId);
        Tag GetByName(string name, int blogId);
        IList<Tag> GetByNames(string[] names, int blogId);
        IList<Tag> GetByBlogEntryId(int blogEntryId);
    }
}
