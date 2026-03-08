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
    public interface IEntryCommentRepository : IRepository<Comment, long>
    {
        IList<Comment> GetByEntry(long blogPostId, int targetStatus, long blogId);
        IList<Comment> GetByEntry(long blogPostId, long blogId);
        IList<Comment> GetAllUnapproved(long blogId);
        IList<Comment> GetAllApproved(long blogId);
        IList<Comment> GetAllDeleted(long blogId);
    }
}
