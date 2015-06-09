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
using System.Data.Objects;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.Entities;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer;
using AlwaysMoveForward.AnotherBlog.DataLayer.Entities;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    /// <summary>
    /// This class contains all the code to extract SiteInfo data from the repository using LINQ
    /// The SiteOnfo object is used for web site specific settings rather than blog specific settings.
    /// </summary>
    /// <param name="dataContext"></param>
    public class SiteInfoRepository : EntityFrameworkRepository<SiteInfo, SiteInfo>, ISiteInfoRepository
    {
        internal SiteInfoRepository(IUnitOfWork unitOfWork, RepositoryManager repositoryManager)
            : base(unitOfWork, repositoryManager)
        {

        }

        public override string IdPropertyName
        {
            get { return "SiteId"; }
        }

        public override string TableName
        {
            get { return "SiteInfo";}
        }
        /// <summary>
        /// Get stored web site settings.
        /// </summary>
        /// <returns></returns>
        public SiteInfo GetSiteInfo()
        {
            SiteInfo retVal = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.SiteInfos select foundItem).Single();
            return retVal;
        }
    }
}
