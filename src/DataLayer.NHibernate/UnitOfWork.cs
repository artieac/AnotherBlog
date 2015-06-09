using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AlwaysMoveForward.Common.DataLayer;

namespace AlwaysMoveForward.AnotherBlog.DataLayer
{
    /// <summary>
    /// A unit of work implementation to co locate the NHibernate configuration with the DTOs
    /// </summary>
    public class UnitOfWork : AlwaysMoveForward.Common.DataLayer.NHibernate.UnitOfWork, IUnitOfWork, IDisposable
    {
        private static System.Object initializationLock = new System.Object();
        private static bool mappingsInitialized = false;

        /// <summary>
        /// The default constructor
        /// </summary>
        public UnitOfWork()
            : base()
        {
            this.InitializeNHibernateMappings();
        }

        /// <summary>
        /// A constructor that takes database connection strings to vistaprint.  These need to go away long term.
        /// </summary>
        /// <param name="vistaprintConnectionString">The connection string for the vistaprint database</param>
        public UnitOfWork(string connectionString)
            : base(connectionString)
        {
            this.InitializeNHibernateMappings();
        }

        /// <summary>
        /// Initialize nhibernate with teh dto elements found in this assembly.
        /// </summary>
        private void InitializeNHibernateMappings()
        {
            // Not initialized yet
            if (UnitOfWork.mappingsInitialized == false)
            {
                // Enter a lock zone
                lock (initializationLock)
                {
                    // Make one more check that the mappings are initialized in case anything blocked and made it through to here as a result
                    if (UnitOfWork.mappingsInitialized == false)
                    {
                        UnitOfWork.mappingsInitialized = true;

                        // Enable validation (optional)
                        // Here, we serialize all decorated classes (but you can also do it class by class)
                        NHibernate.Mapping.Attributes.HbmSerializer.Default.Validate = true;
                        this.NHibernateConfiguration.AddInputStream(NHibernate.Mapping.Attributes.HbmSerializer.Default.Serialize(System.Reflection.Assembly.GetExecutingAssembly()));
                    }
                }
            }
        }
    }
}
