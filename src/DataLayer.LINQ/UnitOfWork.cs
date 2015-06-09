using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.DataLayer.Entities;

namespace AlwaysMoveForward.AnotherBlog.DataLayer
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        static AlwaysMoveForward.Common.Configuration.DatabaseConfiguration dbConfiguration;
        AnotherBlogDbDataContext dataContext;

        static UnitOfWork()
        {
            dbConfiguration = AlwaysMoveForward.Common.Configuration.DatabaseConfiguration.GetInstance();
        }

        System.Data.Common.DbTransaction currentTransaction;

        #region IUnitOfWork Members

        public void StartSession()
        {
        }

        public void EndSession()
        {
        }

        public IDisposable BeginTransaction()
        {
            return this.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public IDisposable BeginTransaction(IsolationLevel isolationLevel)
        {
            //if (this.currentTransaction == null)
            //{
            //    if (this.DataContext.Connection.State != System.Data.ConnectionState.Open)
            //    {
            //        this.DataContext.Connection.Open();
            //    }

            //    currentTransaction = this.DataContext.Connection.BeginTransaction(isolationLevel);
            //}

            return currentTransaction;
        }

        public void EndTransaction(bool canCommit)
        {
            if (currentTransaction != null)
            {
                if (canCommit)
                {
                    currentTransaction.Commit();
                }
                else
                {
                    currentTransaction.Rollback();
                }

                currentTransaction.Dispose();
                currentTransaction = null;
            }
        }

        public void Flush()
        {
            if (this.dataContext != null)
            {
                this.dataContext.SubmitChanges();
            }
        }

        public AnotherBlogDbDataContext DataContext
        {
            get
            {
                if (this.dataContext == null)
                {
                    this.dataContext = new AnotherBlogDbDataContext(new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[UnitOfWork.dbConfiguration.ConnectionString].ConnectionString));
                }

                return this.dataContext;
            }
            set { this.dataContext = value; }
        }

        #endregion

        public void Dispose()
        {
            if (this.currentTransaction != null)
            {
                this.currentTransaction.Dispose();
            }
        }
    }
}
