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

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class EntryCommentRepository : EntityFrameworkRepository<Comment, int>
    {
        internal EntryCommentRepository(IUnitOfWork unitOfWork, RepositoryManager repositoryManager)
            : base(unitOfWork, repositoryManager)
        {
        }

        public override string IdPropertyName
        {
            get { return "Id"; }
        }

        public IList<Comment> GetByEntry(int blogPostId, int targetStatus, int blogId)
        {
            IEnumerable<Comment> retVal = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.Comments
                                          where foundItem.Post.Id == blogPostId &&
                                          (int)foundItem.Status == targetStatus &&
                                          foundItem.BlogId == blogId
                                          select foundItem;
            return retVal.ToList();
        }

        public IList<Comment> GetByEntry(int blogPostId, int blogId)
        {
            IEnumerable<Comment> retVal = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.Comments
                                          where foundItem.Post.Id == blogPostId &&
                                          foundItem.BlogId == blogId
                                          select foundItem;
            return retVal.ToList();
        }

        public IList<Comment> GetAllUnapproved(int blogId)
        {
            return this.GetAllByProperty("Status", (int)CommentStatus.Unapproved, blogId);
        }

        public IList<Comment> GetAllApproved(int blogId)
        {
            return this.GetAllByProperty("Status", (int)CommentStatus.Approved, blogId);
        }

        public IList<Comment> GetAllDeleted(int blogId)
        {
            return this.GetAllByProperty("Status", (int)CommentStatus.Deleted, blogId);
        }
    }
}
