using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PucksAndProgramming.Common.Configuration;
using PucksAndProgramming.Common.DataLayer;
using PucksAndProgramming.Common.Utilities;
using PucksAndProgramming.OAuth.Client;
using PucksAndProgramming.OAuth.Client.Configuration;
using PucksAndProgramming.AnotherBlog.DataLayer;
using PucksAndProgramming.AnotherBlog.Common.DataLayer.Repositories;
using PucksAndProgramming.AnotherBlog.BusinessLayer.Service;

namespace PucksAndProgramming.AnotherBlog.BusinessLayer.Service
{
    public class ServiceManagerBuilder
    {
        /// <summary>
        /// A default encryption key value
        /// </summary>
        private const string DefaultEncryptionKey = "4ADDEBFF7C3D4F6FA455D1D1285387EC53D29CCDCFED4C56ADD65EB24F3D1C68D4C4D4683EA3436880DFBEF684F5DC51F26875A89AAD49DCB74B1DDFD6A7AF53";

        /// <summary>
        /// A default salt value
        /// </summary>
        private const string DefaultSalt = "36E336FABA034E47B6CEEF9BEF1E0D57";

        public static ServiceManager BuildServiceManager()
        {
            ServiceManagerBuilder serviceManagerBuilder = new ServiceManagerBuilder();
            return serviceManagerBuilder.CreateServiceManager();
        }

        public ServiceManagerBuilder()
        {

        }

        public ServiceManager CreateServiceManager()
        {
            ServiceManager retVal = null;

            try
            {
                DatabaseConfiguration databaseConfiguration = DatabaseConfiguration.GetInstance();

                UnitOfWork unitOfWork = null;

                if (databaseConfiguration.EncryptionMethod == PucksAndProgramming.Common.Encryption.EncryptedConfigurationSection.EncryptionMethodOptions.Internal)
                {
                    unitOfWork = this.CreateUnitOfWork(databaseConfiguration.GetDecryptedConnectionString(DefaultEncryptionKey, DefaultSalt));
                }
                else
                {
                    unitOfWork = this.CreateUnitOfWork(databaseConfiguration.GetDecryptedConnectionString());
                }

                IAnotherBlogRepositoryManager repositoryManager = this.CreateRepositoryManager(unitOfWork);

                retVal = new ServiceManager(unitOfWork, repositoryManager, this.CreateOAuthClient());
            }
            catch(Exception e)
            {
                LogManager.GetLogger().Error(e);
            }

            return retVal;
        }

        protected virtual UnitOfWork CreateUnitOfWork(string connectionString)
        {
            return new UnitOfWork(connectionString);
        }

        protected virtual IAnotherBlogRepositoryManager CreateRepositoryManager(IUnitOfWork unitOfWork)
        {
            return new RepositoryManager(unitOfWork as UnitOfWork);
        }

        protected virtual OAuthClientBase CreateOAuthClient()
        {
            OAuthKeyConfiguration keyConfiguration =  OAuthKeyConfiguration.GetInstance();
            EndpointConfiguration oauthEndpoints = EndpointConfiguration.GetInstance();
            return new PucksAndProgramming.OAuth.Client.RestSharp.OAuthClient(oauthEndpoints.ServiceUri, keyConfiguration.ConsumerKey, keyConfiguration.ConsumerSecret, oauthEndpoints);
        }
    }
}
