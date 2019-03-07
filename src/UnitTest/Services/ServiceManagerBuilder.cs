using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PucksAndProgramming.AnotherBlog.DataLayer;

namespace PucksAndProgramming.AnotherBlog.UnitTest.Services
{
    public class ServiceManagerBuilder : PucksAndProgramming.AnotherBlog.BusinessLayer.Service.ServiceManagerBuilder
    {
        protected override UnitOfWork CreateUnitOfWork(string connectionString)
        {
            return new UnitOfWork(connectionString);
        }
    }
}
