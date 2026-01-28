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
using X.PagedList;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Web.Models
{
    public class UserModel : ModelBase
    {
        public UserModel()
            : base()
        {

        }

        public Blog TargetBlog { get; set; }
        public AnotherBlogUser CurrentUser { get; set; }
        public IPagedList<AnotherBlogUser> UserList { get; set; }
        public IList<RoleType> RoleList { get; set; }
    }
}