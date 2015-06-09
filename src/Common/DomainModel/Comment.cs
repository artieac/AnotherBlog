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

using AlwaysMoveForward.AnotherBlog.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;

namespace AlwaysMoveForward.AnotherBlog.Common.DomainModel
{
    public class Comment 
    {
        /// <summary>
        /// What are the allowed comment statuses?  
        /// </summary>
        public enum CommentStatus
        {
            Unapproved = 0,
            Approved = 1,
            Deleted = 2,
            None = 99
        }

        public Comment()
        {
            this.Id = -1;
        }

        public int Id { get; set; }
        public CommentStatus Status { get; set; }
        public string Link { get; set; }
        public string AuthorEmail { get; set; }
        public string Text { get; set; }
        public string AuthorName { get; set; }
        public DateTime DatePosted { get; set; }
        
        public void CleanCommentText()
        {
            this.Text = Utils.StripJavascript(this.Text);
        }

        public string DefaultDateStringFormat
        {
            get { return this.DatePosted.ToShortDateString(); }
        }

        public string StatusText
        {
            get { return this.Status.ToString(); }
        }
    }
}
