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

namespace AlwaysMoveForward.AnotherBlog.Common.DomainModel
{
    public class SiteInfo 
    {
        public int SiteId { get; set; }

        public string About { get; set; }
 
        public string Name { get; set; }
        
        public string ContactEmail { get; set; }
        
        public string DefaultTheme { get; set; }
        
        public string SiteAnalyticsId { get; set; }
    }
}
