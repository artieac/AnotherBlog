using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.Common.DataLayer;

namespace AlwaysMoveForward.AnotherBlog.DataLayer
{
    /// <summary>
    /// A unit of work implementation to co locate the NHibernate configuration with the DTOs
    /// </summary>
    public class UnitOfWork : AlwaysMoveForward.Common.DataLayer.NHibernate.UnitOfWork, IUnitOfWork, IDisposable
    {
        /// <summary>
        /// The default constructor
        /// </summary>
        public UnitOfWork()
            : base()
        {

        }

        /// <summary>
        /// A constructor that takes database connection strings to vistaprint.  These need to go away long term.
        /// </summary>
        /// <param name="vistaprintConnectionString">The connection string for the vistaprint database</param>
        public UnitOfWork(string connectionString)
            : base(connectionString)
        {

        }

        protected override void StartSession()
        {
            this.CurrentSession = AlwaysMoveForward.Common.DataLayer.NHibernate.NHibernateSessionFactory.BuildSessionFactory(this.NHibernateConfiguration, System.Reflection.Assembly.GetExecutingAssembly()).OpenSession(); ;
        }
    }
}
