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
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.Common.DataLayer;

namespace PucksAndProgramming.AnotherBlog.Common.DataLayer.Repositories
{
    public interface IUserRepository : IRepository<AnotherBlogUser, long>
    {
        IList<AnotherBlogUser> GetBlogWriters(int blogId);

        AnotherBlogUser GetByOAuthServiceUserId(string userId);

        AnotherBlogUser GetByEmail(string email);
    }
}
