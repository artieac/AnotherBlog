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
using NHibernate;
using NHibernate.Criterion;
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.NHibernate;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer.DTO;
using AlwaysMoveForward.AnotherBlog.DataLayer.DataMapper;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    /// <summary>
    /// This class contains all the code to extract SiteInfo data from the repository using LINQ
    /// The SiteOnfo object is used for web site specific settings rather than blog specific settings.
    /// </summary>
    /// <param name="dataContext"></param>
    public class SiteInfoRepository : NHibernateRepository<SiteInfo, SiteInfoDTO, int>, ISiteInfoRepository
    {
        public SiteInfoRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        protected override SiteInfoDTO GetDTOById(SiteInfo domainInstance)
        {
            return this.GetDTOById(domainInstance.SiteId);
        }

        protected override SiteInfoDTO GetDTOById(int idSource)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<SiteInfoDTO>();
            criteria.Add(Expression.Eq("SiteId", idSource));

            return criteria.UniqueResult<SiteInfoDTO>();
        }

        protected override DataMapBase<SiteInfo, SiteInfoDTO> GetDataMapper()
        {
            return new SiteInfoDataMap(); 
        }

        /// <summary>
        /// Get stored web site settings.
        /// </summary>
        /// <returns></returns>
        public SiteInfo GetSiteInfo()
        {
            return this.GetDataMapper().Map(this.UnitOfWork.CurrentSession.CreateCriteria<SiteInfoDTO>().UniqueResult<SiteInfoDTO>());
        }

        public override bool Delete(SiteInfo itemToDelete)
        {
            throw new NotImplementedException();
        }
    }
}
