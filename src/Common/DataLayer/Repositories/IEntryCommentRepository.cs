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

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories
{
    public interface IEntryCommentRepository : IRepository<Comment, int>
    {
        IList<Comment> GetByEntry(int blogPostId, int targetStatus, int blogId);
        IList<Comment> GetByEntry(int blogPostId, int blogId);
        IList<Comment> GetAllUnapproved(int blogId);
        IList<Comment> GetAllApproved(int blogId);
        IList<Comment> GetAllDeleted(int blogId);
    }
}
