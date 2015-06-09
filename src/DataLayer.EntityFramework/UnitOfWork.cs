using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.DataLayer.Entities;

namespace AlwaysMoveForward.AnotherBlog.DataLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        static AlwaysMoveForward.Common.Configuration.DatabaseConfiguration dbConfiguration;

        static UnitOfWork()
        {
            dbConfiguration = AlwaysMoveForward.Common.Configuration.DatabaseConfiguration.GetInstance();
        }

        AnotherBlogDataContextCF dataContext;
        TransactionScope currentTransaction;

        #region IUnitOfWork Members

        public IDisposable BeginTransaction()
        {
            return this.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        }

        public IDisposable BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            if (this.currentTransaction == null)
            {
                System.Transactions.IsolationLevel isoLevel = System.Transactions.IsolationLevel.Unspecified;

                switch (isolationLevel)
                {
                    case System.Data.IsolationLevel.Chaos:
                        isoLevel = System.Transactions.IsolationLevel.Chaos;
                        break;
                    case System.Data.IsolationLevel.ReadCommitted:
                        isoLevel = System.Transactions.IsolationLevel.ReadCommitted;
                        break;
                    case System.Data.IsolationLevel.ReadUncommitted:
                        isoLevel = System.Transactions.IsolationLevel.ReadUncommitted;
                        break;
                    case System.Data.IsolationLevel.RepeatableRead:
                        isoLevel = System.Transactions.IsolationLevel.RepeatableRead;
                        break;
                    case System.Data.IsolationLevel.Serializable:
                        isoLevel = System.Transactions.IsolationLevel.Serializable;
                        break;
                    case System.Data.IsolationLevel.Snapshot:
                        isoLevel = System.Transactions.IsolationLevel.Snapshot;
                        break;
                }

                this.currentTransaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = isoLevel });
            }

            return currentTransaction;
        }

        public void EndTransaction(bool canCommit)
        {
            if (currentTransaction != null)
            {
                if (canCommit)
                {
                    currentTransaction.Complete();
                }

                currentTransaction.Dispose();
                currentTransaction = null;
            }
        }

        public void Commit()
        {
            if (this.DataContext != null)
            {
                this.DataContext.SaveChanges();
            }
        }

        public void Flush()
        {

        }

        public AnotherBlogDataContextCF DataContext
        {
            get
            {
                if (this.dataContext == null)
                {
                    this.dataContext = new AnotherBlogDataContextCF(System.Configuration.ConfigurationManager.ConnectionStrings[UnitOfWork.dbConfiguration.ConnectionString].ConnectionString);
                }

                return this.dataContext;
            }
            set { this.dataContext = value; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {

        }

        #endregion
    }
}
