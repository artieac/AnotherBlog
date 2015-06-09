using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlwaysMoveForward.Common.DataLayer.ActiveRecord;
using AlwaysMoveForward.AnotherBlog.DataLayer;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.IntegrationTests.Repository
{
    public class RepositoryTestBase
    {
        UnitOfWork unitOfWork;
        RepositoryManager repositoryManager;

        public UnitOfWork UnitOfWork
        {
            get
            {
                if(unitOfWork==null)
                {
                    unitOfWork = new UnitOfWork(true);
                }

                return unitOfWork;
            }
        }

        public RepositoryManager RepositoryManager
        {
            get
            {
                if(repositoryManager==null)
                {
                    repositoryManager = new RepositoryManager(this.UnitOfWork);
                }

                return repositoryManager;
            }
        }
    }
}
