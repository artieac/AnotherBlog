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
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using PucksAndProgramming.Common.DomainModel;
using PucksAndProgramming.Common.DataLayer;
using PucksAndProgramming.Common.DataLayer.NHibernate;
using PucksAndProgramming.Common.DataLayer.Repositories;
using PucksAndProgramming.AnotherBlog.Common.DataLayer;
using PucksAndProgramming.AnotherBlog.Common.DataLayer.Map;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Common.DataLayer.Repositories;
using PucksAndProgramming.AnotherBlog.DataLayer.DTO;
using PucksAndProgramming.AnotherBlog.DataLayer.DataMapper;

namespace PucksAndProgramming.AnotherBlog.DataLayer.Repositories
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
