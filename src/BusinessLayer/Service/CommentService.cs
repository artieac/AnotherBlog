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
using AlwaysMoveForward.Common.Business;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer.Repositories;

namespace AlwaysMoveForward.AnotherBlog.BusinessLayer.Service
{
    public class CommentService : AnotherBlogService
    {
        public CommentService(IUnitOfWork unitOfWork, IEntryCommentRepository commentRepository) : base(unitOfWork) 
        {
            this.EntryCommentRepository = commentRepository;
        }

        protected IEntryCommentRepository EntryCommentRepository { get; private set; }

        public IList<Comment> GetByBlogAndPostId(Blog targetBlog, int postId)
        {
            IList<Comment> retVal = new List<Comment>();

            if (targetBlog != null)
            {
                return this.EntryCommentRepository.GetByEntry(postId, targetBlog.Id);
            }

            return retVal;
        }
    }
}
