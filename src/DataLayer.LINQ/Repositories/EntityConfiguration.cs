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

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    /// <summary>
    /// This class contains the connection strings neccessary for any database access.  This way it doesn't
    /// have to call out to upper layers (which it shouldn't have access to)
    /// </summary>
    public class EntityConfiguration
    {
        public static string connectionString = "";
        public static string adminConnectionString = "";
    }
}
