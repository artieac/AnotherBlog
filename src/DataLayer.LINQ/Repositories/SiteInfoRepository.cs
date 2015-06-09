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
using AlwaysMoveForward.Common.DataLayer.Entities;
using AlwaysMoveForward.Common.DataLayer.Map;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer.Entities;
using AlwaysMoveForward.Common.Utilities;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    /// <summary>
    /// This class contains all the code to extract SiteInfo data from the repository using LINQ
    /// The SiteOnfo object is used for web site specific settings rather than blog specific settings.
    /// </summary>
    /// <param name="dataContext"></param>
    public class SiteInfoRepository : LINQRepository<SiteInfo, SiteInfoDTO>, ISiteInfoRepository
    {
        internal SiteInfoRepository(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
            : base(unitOfWork, repositoryManager)
        {

        }

        public override string IdPropertyName
        {
            get { return "SiteId"; }
        }

        protected override SiteInfoDTO GetDTOByDomain(SiteInfo targetItem)
        {
            return this.GetDtoById(targetItem.SiteId);
        }
        /// <summary>
        /// Get stored web site settings.
        /// </summary>
        /// <returns></returns>
        public SiteInfo GetSiteInfo()
        {
            SiteInfoDTO retVal = null;

            try
            {
                retVal = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.SiteInfoDTOs select foundItem).Single();
            }
            catch (Exception e)
            {
                LogManager.GetLogger().Warn(e.Message);
            }

            return this.Map(retVal);
        }
    }
}
