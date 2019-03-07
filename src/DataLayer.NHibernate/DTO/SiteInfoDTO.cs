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
using PucksAndProgramming.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Common.DataLayer.Map;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;

namespace PucksAndProgramming.AnotherBlog.DataLayer.DTO
{
    [NHibernate.Mapping.Attributes.Class(Table = "SiteInfo")]
    public class SiteInfoDTO 
    {
        public SiteInfoDTO() : base()
        {
            this.SiteId = -1;
        }

        [NHibernate.Mapping.Attributes.Id(0, Name="SiteId", Type = "Int32", Column = "SiteId", UnsavedValue = "-1")]
        [NHibernate.Mapping.Attributes.Generator(1, Class = "native")]
        public virtual int SiteId { get; set; }

        [NHibernate.Mapping.Attributes.Property(Type="StringClob")]
        public virtual string About { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual string Name { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual string ContactEmail { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual string DefaultTheme { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual string SiteAnalyticsId { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual string DefaultAuthor { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual string DefaultKeywords { get; set; }
    }
}
