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
using PucksAndProgramming.Common.DomainModel.DataMap;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;

namespace PucksAndProgramming.AnotherBlog.DataLayer.DTO
{
    [NHibernate.Mapping.Attributes.Class(Table = "DbInfo")]
    public class DbInfoDTO
    {
        public DbInfoDTO() : base()
        {

        }

        [NHibernate.Mapping.Attributes.Id(Type = "Int32", Column = "Version", UnsavedValue = "-1")]
        public virtual int Version { get; set; }
    }
}
