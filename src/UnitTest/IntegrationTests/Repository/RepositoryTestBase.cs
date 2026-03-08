using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using AlwaysMoveForward.Common.Configuration;
using AlwaysMoveForward.Common.Encryption;
using AlwaysMoveForward.AnotherBlog.DataLayer;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.IntegrationTests.Repository
{
    public class RepositoryTestBase
    {
        UnitOfWork unitOfWork;
        RepositoryManager repositoryManager;
        DatabaseConfiguration databaseConfiguration;

        protected DatabaseConfiguration DatabaseConfiguration
        {
            get
            {
                if (databaseConfiguration == null)
                {
                    databaseConfiguration = new DatabaseConfiguration(
                        Options.Create(new AESConfiguration()),
                        Options.Create(new KeyFileConfiguration()),
                        Options.Create(new KeyStoreConfiguration()),
                        Options.Create(new RSAXmlKeyFileConfiguration()))
                    {
                        ConnectionString = "Data Source=localhost\\DBLocal;Initial Catalog=AMForwardDb;User ID=test;Password=test;"
                    };
                }

                return databaseConfiguration;
            }
        }

        public UnitOfWork UnitOfWork
        {
            get
            {
                if(unitOfWork==null)
                {
                    unitOfWork = new UnitOfWork(this.DatabaseConfiguration);
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
