using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlwaysMoveForward.AnotherBlog.DataLayer;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.Services
{
    public class ServiceManagerBuilder : AlwaysMoveForward.AnotherBlog.BusinessLayer.Service.ServiceManagerBuilder
    {
        protected override AlwaysMoveForward.Common.DataLayer.IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(true);
        }
    }
}
