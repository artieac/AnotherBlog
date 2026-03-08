using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlwaysMoveForward.AnotherBlog.DataLayer;
using AlwaysMoveForward.Common.Configuration;
using Microsoft.Extensions.Options;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.Services
{
    public class ServiceManagerBuilder : AlwaysMoveForward.AnotherBlog.BusinessLayer.Service.ServiceManagerBuilder
    {
        private readonly DatabaseConfiguration _databaseConfiguration;

        public ServiceManagerBuilder(IOptions<DatabaseConfiguration> databaseConfiguration)
            : base(databaseConfiguration)
        {
            _databaseConfiguration = databaseConfiguration.Value;
        }

        protected override UnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(_databaseConfiguration);
        }
    }
}
