using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.DataLayer.Entities;
using Microsoft.Extensions.Options;
using AlwaysMoveForward.Common.Configuration;

namespace AlwaysMoveForward.AnotherBlog.DataLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        AnotherBlogDataContextCF dataContext;
        IDbContextTransaction currentTransaction;
        private DatabaseConfiguration DatabaseConfiguration {  get; set; }

        public UnitOfWork(DatabaseConfiguration databaseConfiguration)
        {
            this.DatabaseConfiguration = databaseConfiguration;
        }

        #region IUnitOfWork Members

        public IDisposable BeginTransaction()
        {
            return this.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        }

        public IDisposable BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            if (this.currentTransaction == null)
            {
                this.currentTransaction = this.DataContext.Database.BeginTransaction(isolationLevel);
            }

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
                    string connString = this.DatabaseConfiguration.GetDecryptedConnectionString();
                    this.dataContext = new AnotherBlogDataContextCF(connString);
                }

                return this.dataContext;
            }
            set { this.dataContext = value; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (this.dataContext != null)
            {
                this.dataContext.Dispose();
                this.dataContext = null;
            }
        }

        #endregion
    }
}
