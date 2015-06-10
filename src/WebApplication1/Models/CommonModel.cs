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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.Web.Models
{
    public class CommonModel
    {
        public CommonModel()
        {
            this.TargetMonth = DateTime.Now;
        }

        public string ContentTitle { get; set; }

        public DateTime TargetMonth { get; set; }
        
        public CalendarModel Calendar { get; set; }
    }
}