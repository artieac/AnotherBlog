﻿/**
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

namespace AlwaysMoveForward.AnotherBlog.Common.DomainModel
{
    public class TagCount
    {
        public TagCount()
        {

        }

        public TagCount(string tagName, int tagCount)
        {
            this.TagName = tagName;
            this.Count = tagCount;
        }

        public string TagName { get; set; }
        public int Count { get; set; }
    }
}
