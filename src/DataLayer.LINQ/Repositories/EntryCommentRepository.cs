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
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer.Entities;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class EntryCommentRepository : LINQRepository<Comment, EntryCommentDTO>, IEntryCommentRepository
    {
        internal EntryCommentRepository(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
            : base(unitOfWork, repositoryManager)
        {
        }

        public override string IdPropertyName
        {
            get { return "CommentId"; }
        }

        protected override EntryCommentDTO GetDTOByDomain(Comment targetItem)
        {
            return this.GetDtoById(targetItem.CommentId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="targetStatus"></param>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public IList<Comment> GetByEntry(int blogPostId, int targetStatus, int blogId)
        {
            IQueryable<EntryCommentDTO> dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.EntryCommentDTOs where foundItem.BlogEntryDTO.EntryId == blogPostId && foundItem.Status == targetStatus && foundItem.BlogId == blogId select foundItem;
            return dtoList.Cast<Comment>().ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="targetStatus"></param>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public IList<Comment> GetByEntry(int blogPostId, int blogId)
        {
            return this.GetAllByProperty("EntryId", blogPostId, blogId); 
        }
        /// <summary>
        /// Get all comments for a specific blog that need to be approved by a blogger or administrator
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public IList<Comment> GetAllUnapproved(int blogId)
        {
            return this.GetAllByProperty("Status", Comment.CommentStatus.Unapproved, blogId);
        }
        /// <summary>
        /// Get all approved comments ofr a blog for display with the blog entry.
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public IList<Comment> GetAllApproved(int blogId)
        {
            return this.GetAllByProperty("Status", Comment.CommentStatus.Approved, blogId); 
        }
        /// <summary>
        /// Get all deleted comments (in case it should be undeleted, or for a report on most frequenc abusers)
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public IList<Comment> GetAllDeleted(int blogId)
        {
            return this.GetAllByProperty("Status", Comment.CommentStatus.Deleted, blogId); 
        }
    }
}

