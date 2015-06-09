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
using NHibernate.Transform;
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
    public class DbInfoRepository : NHibernateRepository<DbInfo, DbInfoDTO, int>, IDbInfoRepository
    {
        public DbInfoRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        protected override DbInfoDTO GetDTOById(DbInfo domainInstance)
        {
            return this.GetDTOById(domainInstance.Version);
        }

        protected override DbInfoDTO GetDTOById(int idSource)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<DbInfoDTO>();
            criteria.Add(Expression.Eq("Version", idSource));
            return criteria.UniqueResult<DbInfoDTO>();
        }

        protected override DataMapBase<DbInfo, DbInfoDTO> GetDataMapper()
        {
            return new DbInfoMapper(); 
        }

        public DbInfo GetDbInfo()
        {
            return this.GetDataMapper().Map(this.UnitOfWork.CurrentSession.CreateCriteria<DbInfoDTO>().UniqueResult<DbInfoDTO>());
        }

        public override bool Delete(DbInfo itemToDelete)
        {
            throw new NotImplementedException();
        }
    }
}
