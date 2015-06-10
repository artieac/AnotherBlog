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
using System.Web;

using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Web.Models.BlogModels
{
    public class RSSModel
    {
        public string Scheme { get; set; }

        public string Authority { get; set; }

        public CommonBlogModel BlogCommon { get; set; }

        public Dictionary<Blog, IList<BlogPost>> BlogEntries { get; set; }
        
        public Dictionary<Blog, DateTime> MostRecentPosts { get; set; }
        
        public Dictionary<Blog, IList<Comment>> Comments { get; set; }
    }
}
