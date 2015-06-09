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
using System.Security.Principal;
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.DTO
{
    [NHibernate.Mapping.Attributes.Class(Table = "Users")]
    public class UserDTO 
    {
        public UserDTO() : base()
        {
            this.Id = -1;
        }

        [NHibernate.Mapping.Attributes.Id(0, Name = "Id", Column = "Id", UnsavedValue = "-1")]
        [NHibernate.Mapping.Attributes.Generator(1, Class = "native")]
        public virtual long Id { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual bool ApprovedCommenter { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual bool IsSiteAdministrator { get; set; }

        [NHibernate.Mapping.Attributes.Property(Type="StringClob")]
        public virtual string About { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual long OAuthServiceUserId { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual string FirstName { get; set; }
        
        [NHibernate.Mapping.Attributes.Property]
        public virtual string LastName { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual string AccessToken { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual string AccessTokenSecret { get; set; }

        [NHibernate.Mapping.Attributes.Bag(0, Table = "BlogUsers", Cascade = "All-Delete-Orphan", Inverse = true)]
        [NHibernate.Mapping.Attributes.Key(1, Column = "UserId")]
        [NHibernate.Mapping.Attributes.OneToMany(2, ClassType = typeof(BlogUserDTO))]
        public virtual IList<BlogUserDTO> Roles { get; set; }
    }
}
