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
using System.Configuration;

namespace AlwaysMoveForward.AnotherBlog.Web.Code.Utilities
{
    public class WebSiteConfiguration : ConfigurationSection
    {
        public const string UpdateDatabaseSetting = "UpdateDb";
        public const string EnableSSLSetting = "EnableSSL";
        public const string DefaultSiteNameSetting = "DefaultSiteName";

        public WebSiteConfiguration() { }
        public WebSiteConfiguration(bool updateDb)
        {
            this.UpdateDb = updateDb;
        }

        public override bool IsReadOnly()
        {
            return false;
        }

        [ConfigurationProperty(WebSiteConfiguration.UpdateDatabaseSetting, IsRequired = true)]
        public bool UpdateDb
        {
            get { return (bool)this[WebSiteConfiguration.UpdateDatabaseSetting]; }
            set { this[WebSiteConfiguration.UpdateDatabaseSetting] = value; }
        }

        [ConfigurationProperty(WebSiteConfiguration.EnableSSLSetting, IsRequired = true)]
        public bool EnableSSL
        {
            get { return (bool)this[WebSiteConfiguration.EnableSSLSetting]; }
            set { this[WebSiteConfiguration.EnableSSLSetting] = value; }
        }

        [ConfigurationProperty(WebSiteConfiguration.DefaultSiteNameSetting, IsRequired = true)]
        public string DefaultSiteName
        {
            get { return (string)this[WebSiteConfiguration.DefaultSiteNameSetting]; }
            set { this[WebSiteConfiguration.DefaultSiteNameSetting] = value; }
        }
    }
}
