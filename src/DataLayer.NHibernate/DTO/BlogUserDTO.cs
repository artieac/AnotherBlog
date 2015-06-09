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
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.DTO
{
    [NHibernate.Mapping.Attributes.Class(Table = "BlogUsers")]
    public class BlogUserDTO 
    {
        public BlogUserDTO()
        {
            this.Id = -1;
        }

        [NHibernate.Mapping.Attributes.Id(0, Name = "Id", Column = "Id", UnsavedValue = "-1")]
        [NHibernate.Mapping.Attributes.Generator(1, Class = "native")]
        public virtual int Id { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual int BlogId { get; set; }

        [NHibernate.Mapping.Attributes.ManyToOne(Name = "User", Class = "UserDTO", ClassType = typeof(UserDTO), Column = "UserId")]
        public virtual UserDTO User { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual int RoleId { get; set; }
    }
}
