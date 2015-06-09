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
    [NHibernate.Mapping.Attributes.Class(Table = "BlogEntries")]
    public class BlogPostDTO 
    {
        public BlogPostDTO()
        {
            this.Id = -1;
        }

        [NHibernate.Mapping.Attributes.Id(0, Name="Id", Column = "Id", UnsavedValue = "-1")]
        [NHibernate.Mapping.Attributes.Generator(1, Class = "native")]
        public virtual int Id { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual bool IsPublished { get; set; }

        [NHibernate.Mapping.Attributes.ManyToOne(Name = "Blog", Class = "BlogDTO", ClassType = typeof(BlogDTO), Column = "BlogId")]
        public virtual BlogDTO Blog { get; set; }

        [NHibernate.Mapping.Attributes.ManyToOne(Name = "Author", Class = "UserDTO", ClassType = typeof(UserDTO), Column = "UserId")]
        public virtual UserDTO Author { get; set; }

        [NHibernate.Mapping.Attributes.Property(Type="StringClob")]
        public virtual string EntryText { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual string Title { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual DateTime DatePosted { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual DateTime DateCreated { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual int TimesViewed { get; set; }

        [NHibernate.Mapping.Attributes.Bag(0, Table = "BlogEntryTags", Cascade = "Save-Update")]
        [NHibernate.Mapping.Attributes.Key(1, Column = "BlogEntryId")]
        [NHibernate.Mapping.Attributes.ManyToMany(2, Column = "TagId", ClassType = typeof(TagDTO))]
        public virtual IList<TagDTO> Tags { get; set; }

        [NHibernate.Mapping.Attributes.Bag(0, Table = "EntryComments", Cascade = "All-Delete-Orphan", Inverse = true)]
        [NHibernate.Mapping.Attributes.Key(1, Column = "Id")]
        [NHibernate.Mapping.Attributes.OneToMany(2, ClassType = typeof(EntryCommentsDTO))]
        public virtual IList<EntryCommentsDTO> Comments { get; set; }
    }
}
