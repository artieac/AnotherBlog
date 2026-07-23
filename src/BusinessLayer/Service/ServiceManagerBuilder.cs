using System;
using AlwaysMoveForward.Common.Configuration;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Events.BlogEntry;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace AlwaysMoveForward.AnotherBlog.BusinessLayer.Service
{
    public class ServiceManagerBuilder
    {
        private const string DefaultEncryptionKey = "4ADDEBFF7C3D4F6FA455D1D1285387EC53D29CCDCFED4C56ADD65EB24F3D1C68D4C4D4683EA3436880DFBEF684F5DC51F26875A89AAD49DCB74B1DDFD6A7AF53";
        private const string DefaultSalt = "36E336FABA034E47B6CEEF9BEF1E0D57";

        // Static once-only guard so the event listener is registered exactly
        // once for the lifetime of the application, regardless of how many
        // ServiceManager instances are created per request.
        private static volatile bool _eventListenersRegistered = false;
        private static readonly object _eventListenerLock = new object();

        /// <summary>
        /// Static factory method for legacy code that doesn't use DI.
        /// </summary>

        private DatabaseConfiguration DatabaseConfiguration { get; set; }

        public ServiceManagerBuilder(IOptions<DatabaseConfiguration> databaseConfiguration)
        {
            this.DatabaseConfiguration = databaseConfiguration.Value;
        }

        public ServiceManager CreateServiceManager()
        {
            ServiceManager retVal = null;

            try
            {
                UnitOfWork unitOfWork = this.CreateUnitOfWork();
                IAnotherBlogRepositoryManager repositoryManager = this.CreateRepositoryManager(unitOfWork);
                retVal = new ServiceManager(unitOfWork, repositoryManager);

                // Register domain event listeners exactly once at first use.
                this.EnsureEventListenersRegistered();
            }
            catch(Exception e)
            {
                LogManager.GetLogger().Error(e);
            }

            return retVal;
        }

        /// <summary>
        /// Registers application-wide domain event listeners the first time
        /// a ServiceManager is created. Each listener receives a factory so
        /// it can open a fresh DbContext per event invocation.
        /// </summary>
        private void EnsureEventListenersRegistered()
        {
            if (!_eventListenersRegistered)
            {
                lock (_eventListenerLock)
                {
                    if (!_eventListenersRegistered)
                    {
                        var listener = new BlogPostViewedEventListener(() =>
                        {
                            // Each event gets its own UnitOfWork so the repository
                            // is never sharing a disposed or in-flight DbContext.
                            var eventUoW = this.CreateUnitOfWork();
                            return this.CreateRepositoryManager(eventUoW).BlogPostViews;
                        });

                        listener.Register();
                        _eventListenersRegistered = true;
                    }
                }
            }
        }

        protected virtual UnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(this.DatabaseConfiguration);
        }

        protected virtual IAnotherBlogRepositoryManager CreateRepositoryManager(IUnitOfWork unitOfWork)
        {
            return new RepositoryManager(unitOfWork as UnitOfWork);
        }
    }
}
