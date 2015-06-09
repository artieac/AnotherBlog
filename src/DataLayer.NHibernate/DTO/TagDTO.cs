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
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.DTO
{
    [NHibernate.Mapping.Attributes.Class(Table = "Tags")]
    public class TagDTO
    {
        public TagDTO() : base()
        {
            this.Id = -1;
        }

        [NHibernate.Mapping.Attributes.Id(0, Name="Id", Column = "Id", UnsavedValue = "-1")]
        [NHibernate.Mapping.Attributes.Generator(1, Class = "native")]
        public virtual int Id { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual string Name { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual int BlogId { get; set; }

        [NHibernate.Mapping.Attributes.Bag(0, Table = "BlogEntryTags")]
        [NHibernate.Mapping.Attributes.Key(1, Column = "TagID")]
        [NHibernate.Mapping.Attributes.ManyToMany(2, Column = "BlogEntryId", ClassType = typeof(BlogPostDTO))]
        public virtual IList<BlogPostDTO> BlogEntries { get; set; }
    }
}
