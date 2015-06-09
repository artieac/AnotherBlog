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
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;

namespace AlwaysMoveForward.AnotherBlog.Common.DomainModel
{
    public class BlogPost
    {
        public const int MaxShortEntryLength = 1000;
        public static readonly DateTime StartDate = new DateTime(2009, 1, 1);

        public BlogPost()
        {
            this.Id = -1;
            this.DatePosted = BlogPost.StartDate;
            this.Tags = new List<Tag>();
        }

        public int Id { get; set; }
        public bool IsPublished { get; set; }
        public Blog Blog { get; set; }
        public AnotherBlogUser Author { get; set; }
        public string EntryText { get; set; }
        public string Title { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime DateCreated { get; set; }
        public int CommentCount { get; set; }
        public int TimesViewed { get; set; }
        public IList<Comment> Comments { get; set; }
        public IList<Tag> Tags { get; set; }
        
        public IList<Comment> FilteredComments(Comment.CommentStatus targetStatus)
        {
            return this.Comments.Where(comment => comment.Status == targetStatus).ToList();
        }

        public Comment AddComment(string authorName, string authorEmail, string commentText, string commentLink, AnotherBlogUser currentUser)
        {
            Comment retVal = new Comment();
            retVal.AuthorName = authorName;
            retVal.AuthorEmail = authorEmail;
            retVal.DatePosted = DateTime.Now;
            retVal.Link = commentLink;
            retVal.Status = Comment.CommentStatus.Unapproved;
            retVal.Text = commentText;

            if(currentUser != null && currentUser.ApprovedCommenter == true)
            {
                retVal.Status = Comment.CommentStatus.Approved;
            }

            this.Comments.Add(retVal);
            return retVal;
        }

        public Comment UpdateCommentStatus(int commentId, Comment.CommentStatus commentStatus)
        {
            Comment targetComment = this.Comments.Where(comment => comment.Id == commentId).First();

            if(targetComment != null)
            {
                if(commentStatus == Comment.CommentStatus.Deleted && targetComment.Status == Comment.CommentStatus.Deleted)
                {
                    this.Comments.Remove(targetComment);
                }
                else
                {
                    targetComment.Status = commentStatus;
                }
            }

            return targetComment;
        }

        public string ShortEntryText
        {
            get
            {
                string retVal = Utils.StripHtml(this.EntryText);

                if (retVal.Length > BlogPost.MaxShortEntryLength)
                {
                    retVal = retVal.Substring(0, BlogPost.MaxShortEntryLength);
                }

                return retVal;
            }
        }

        public void SetPublishState(bool newState)
        {
            if (this.IsPublished != newState)
            {
                // the published state has changed
                if (newState == true)
                {
                    if (this.DatePosted.Date == BlogPost.StartDate)
                    {
                        this.DatePosted = DateTime.Now;
                    }
                }
            }
            else
            {
                if (newState == false)
                {
                    this.DatePosted = BlogPost.StartDate;
                }
            }

            this.IsPublished = newState;
        }
    }
}
