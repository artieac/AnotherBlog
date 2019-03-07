using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PucksAndProgramming.Common.DataLayer;
using PucksAndProgramming.Common.Business;
using PucksAndProgramming.AnotherBlog.Common.DataLayer.Repositories;
using PucksAndProgramming.Common.DataLayer.Repositories;

namespace PucksAndProgramming.AnotherBlog.BusinessLayer.Service
{
    public class AnotherBlogService
    {
        public AnotherBlogService(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork { get; set; }
    }
}
