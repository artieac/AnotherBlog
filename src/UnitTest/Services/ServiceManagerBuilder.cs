using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlwaysMoveForward.AnotherBlog.DataLayer;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.Services
{
    public class ServiceManagerBuilder : AlwaysMoveForward.AnotherBlog.BusinessLayer.Service.ServiceManagerBuilder
    {
        public ServiceManagerBuilder(string connectionString)
            : base(connectionString)
        {
        }

        protected override UnitOfWork CreateUnitOfWork(string connectionString)
        {
            return new UnitOfWork(connectionString);
        }
    }
}
