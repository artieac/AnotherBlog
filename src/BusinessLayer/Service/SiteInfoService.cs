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
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;

namespace AlwaysMoveForward.AnotherBlog.BusinessLayer.Service
{
    public class SiteInfoService
    {
        public SiteInfoService(IUnitOfWork unitOfWork, ISiteInfoRepository siteInfoRepository)
        {
            this.UnitOfWork = unitOfWork;
            this.SiteInfoRepository = siteInfoRepository;
        }

        private IUnitOfWork UnitOfWork { get; set; }
        protected ISiteInfoRepository SiteInfoRepository { get; private set; }

        public SiteInfo GetSiteInfo()
        {
            return this.SiteInfoRepository.GetSiteInfo();
        }

        public SiteInfo Save(string siteName, string siteAbout, string siteContact, string defaultTheme, string siteAnalyticsId)
        {
            SiteInfo newItem = this.GetSiteInfo();

            if (newItem == null)
            {
                newItem = new SiteInfo();
            }

            newItem.Name = siteName;
            newItem.About = siteAbout;
            newItem.ContactEmail = siteContact;
            newItem.DefaultTheme = defaultTheme;
            newItem.SiteAnalyticsId = siteAnalyticsId;

            return this.SiteInfoRepository.Save(newItem);
        }
    }
}
