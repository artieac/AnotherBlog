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
using System.Data;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer.Entities;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class ExtensionConfigurationRepository : LINQRepository<ExtensionConfiguration, ExtensionConfigurationDTO>, IExtensionConfigurationRepository
    {
        public ExtensionConfigurationRepository(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
            : base(unitOfWork, repositoryManager)
        {
        }

        public ExtensionConfiguration GetByConfigurationId(int configurationId)
        {
            return this.GetByProperty("ConfigurationId", configurationId);
        }

        public ExtensionConfiguration GetByExtensionId(int extensionId)
        {
            return this.GetByProperty("ExtensionId", extensionId);
        }

        public ExtensionConfiguration GetByExtensionIdAndBlog(int extensionId, int blogId)
        {
            return this.GetByProperty("ExtensionId", extensionId, blogId);
        }

        protected override ExtensionConfigurationDTO GetDTOByDomain(ExtensionConfiguration targetItem)
        {
            return this.GetDtoById(targetItem.ExtensionId);
        }
    }
}
