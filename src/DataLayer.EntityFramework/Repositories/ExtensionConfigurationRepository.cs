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

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.DataLayer.MappingDomainObjects;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class ExtensionConfigurationRepository : EntityFrameworkRepository<ExtensionConfiguration, int>
    {
        public ExtensionConfigurationRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public override string IdPropertyName
        {
            get { return "Id"; }
        }

        public ExtensionConfiguration GetByConfigurationId(int configurationId)
        {
            return this.GetById(configurationId);
        }

        public ExtensionConfiguration GetByExtensionId(int extensionId)
        {
            return this.GetByProperty("ExtensionId", extensionId);
        }

        public ExtensionConfiguration GetByExtensionIdAndBlog(int extensionId, int blogId)
        {
            return this.GetByProperty("ExtensionId", extensionId, blogId);
        }
    }
}
