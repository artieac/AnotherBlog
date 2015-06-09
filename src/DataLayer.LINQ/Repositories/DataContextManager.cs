using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheOffWing.AnotherBlog.Core.Entity
{
    /// <summary>
    /// This class contains the Database context.  This way it can hopefully be changed
    /// from LINQ to something else (such as NHibernate) without forcing massive changes in the Manager layer.
    /// </summary>
    public class DataContextManager
    {
        string connectionString = "";
        private BlogDbDataContext dataContext = null;
        /// <summary>
        /// The connection string to the databsae is required to establish database connections as needed.
        /// </summary>
        /// <param name="connectionString"></param>
        public DataContextManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool IsConnected
        {
            get
            {
                bool retVal = false;

                if (this.DataContext != null)
                {
                    if (this.DataContext.Connection.State == System.Data.ConnectionState.Open)
                    {
                        retVal = true;
                    }
                }

                return retVal;
            }
        }
        /// <summary>
        /// The actual database conneciton isn't established until it is required (lazy load)
        /// </summary>
        public BlogDbDataContext DataContext
        {
            get
            {
                if (this.dataContext == null)
                {
                    this.dataContext = new BlogDbDataContext(connectionString);
                }

                return this.dataContext;
            }
        }
    }
}
