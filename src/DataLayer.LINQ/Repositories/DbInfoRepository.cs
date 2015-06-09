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
    public class DbInfoRepository : LINQRepository<DbInfo, DbInfoDTO>, IDbInfoRepository
    {
        internal DbInfoRepository(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
            : base(unitOfWork, repositoryManager)
        {

        }

        protected override DbInfoDTO GetDTOByDomain(DbInfo targetItem)
        {
            return this.GetDtoById(targetItem.Version);
        }

        public DbInfo GetDbInfo()
        {
            DbInfoDTO retVal = null;

            try
            {
                retVal = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.DbInfoDTOs select foundItem).Single();
            }
            catch (Exception e)
            {
                LogManager.GetLogger().Warn(e.Message);
            }

            return this.Map(retVal);

        }
    }
}
