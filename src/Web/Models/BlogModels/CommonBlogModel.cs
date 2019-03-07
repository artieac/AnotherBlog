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
using System.Collections;

using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Web.Models;

namespace PucksAndProgramming.AnotherBlog.Web.Models.BlogModels
{
    public class CommonBlogModel
    {
        public Blog TargetBlog { get; set; }
        public CommonModel Common { get; set; }
    }
}