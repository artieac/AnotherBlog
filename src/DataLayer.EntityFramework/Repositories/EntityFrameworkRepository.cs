/**
 * Copyright (c) 2009 Arthur Correa.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Common Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/cpl1.0.php
 *
 * Contributors:
 *    Arthur Correa – initial contribution
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Objects;
using System.Data.Entity;

using log4net;
using log4net.Config;

using AutoMapper;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.Entities;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer;
using AlwaysMoveForward.AnotherBlog.DataLayer.Entities;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class EntityFrameworkRepository<DomainClass, DTOClass> : RepositoryBase<DomainClass, DTOClass> 
        where DomainClass : class, new()
        where DTOClass : class, DomainClass, new()
    {
        public EntityFrameworkRepository(IUnitOfWork _unitOfWork, RepositoryManager repositoryManager) : 
            base(_unitOfWork, repositoryManager)
        {

        }

        public override DomainClass Create()
        {
            return new DTOClass();
        }

        protected virtual DbSet<DTOClass> GetEntityInstance()
        {
            return ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<DTOClass>();
        }

        public virtual string BlogIdPropertyName
        {
            get { return "BlogId"; }
        }

        public virtual String TableName
        {
            get { return typeof(DTOClass).Name + "s"; }
        }

        public DTOClass GetDTOByProperty(String propertyName, object idValue)
        {
            DTOClass retVal = null;

            ParameterExpression dtoParameter = Expression.Parameter(typeof(DTOClass), "dtoParam");

            Expression<Func<DTOClass, bool>> whereExpression = Expression.Lambda<Func<DTOClass, bool>>
            (
                Expression.Equal
                (
                    Expression.Property
                    (
                            dtoParameter,
                            propertyName
                    ),
                    Expression.Constant(idValue)
                ),
                new[] { dtoParameter }
            );

            IQueryable<DTOClass> dtoItems = ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<DTOClass>().Where(whereExpression);

            if (dtoItems != null && dtoItems.Count() > 0)
            {
                retVal = dtoItems.Single();
            }

            return retVal;
        }

        public virtual DTOClass GetDTOByDomain(DomainClass domainEntity)
        {
            Object idValue = typeof(DomainClass).GetProperty(this.IdPropertyName).GetValue(domainEntity, null);
            return this.GetDTOByProperty(this.IdPropertyName, idValue);
        }

        public override DomainClass GetByProperty(string propertyName, object idValue)
        {
            return this.GetDTOByProperty(propertyName, idValue);
        }

        public override DomainClass GetByProperty(string propertyName, object idValue, int blogId)
        {
            DTOClass retVal = null;

            String sql = "SELECT * FROM " + this.TableName;
            sql += " WHERE " + propertyName + " =@propertyValue ";
            sql += " AND " + this.BlogIdPropertyName + "=@blogId";

            IDictionary<String, object> queryParams = new Dictionary<String, object>();
            queryParams.Add("propertyValue", idValue);
            queryParams.Add("blogId", blogId);

            IEnumerable<DTOClass> dtoItems = ((UnitOfWork)this.UnitOfWork).DataContext.ExecuteSQL<DTOClass>(sql, queryParams);

            if (dtoItems != null)
            {
                retVal = dtoItems.FirstOrDefault();
            }

            return retVal;
        }

        public override IList<DomainClass> GetAll()
        {
            IQueryable<DTOClass> dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<DTOClass>() select foundItem;
            return dtoList.ToList<DomainClass>();
        }

        public override IList<DomainClass> GetAll(int blogId)
        {
            String sql = "SELECT * FROM " + this.TableName;
            sql += " WHERE " + this.BlogIdPropertyName + "=@blogId";

            IDictionary<String, object> queryParams = new Dictionary<String, object>();
            queryParams.Add("blogId", blogId);

            IEnumerable<DTOClass> dtoList = ((UnitOfWork)this.UnitOfWork).DataContext.ExecuteSQL<DTOClass>(sql, queryParams);
            return dtoList.ToList<DomainClass>();
        }

        public override IList<DomainClass> GetAllByProperty(string propertyName, object idValue)
        {
            ParameterExpression dtoParameter = Expression.Parameter(typeof(DTOClass), "dtoParam");

            Expression<Func<DTOClass, bool>> whereExpression = Expression.Lambda<Func<DTOClass, bool>>
            (
                Expression.Equal
                (
                    Expression.Property
                    (
                            dtoParameter,
                            propertyName
                    ),
                    Expression.Constant(idValue)
                ),
                new[] { dtoParameter }
            );

            IQueryable<DTOClass> dtoList = ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<DTOClass>().Where(whereExpression);

            return dtoList.ToList<DomainClass>();
        }

        public override IList<DomainClass> GetAllByProperty(string propertyName, object idValue, int blogId)
        {
            String sql = "SELECT * FROM " + this.TableName;
            sql += " WHERE " + propertyName + " =@propertyValue ";
            sql += " AND " + this.BlogIdPropertyName + "=@blogId";

            IDictionary<String, object> queryParams = new Dictionary<String, object>();
            queryParams.Add("propertyValue", idValue);
            queryParams.Add("blogId", blogId);

            IEnumerable<DTOClass> dtoList = ((UnitOfWork)this.UnitOfWork).DataContext.ExecuteSQL<DTOClass>(sql, queryParams);
            return dtoList.ToList<DomainClass>();
        }

        public override DomainClass Save(DomainClass itemToSave)
        {
            if (itemToSave != null)
            {
                DTOClass dtoItemToSave = this.GetDTOByDomain(itemToSave);

                if (dtoItemToSave == null)
                {
                    dtoItemToSave = itemToSave as DTOClass;

                    if (dtoItemToSave != null)
                    {
                        ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<DTOClass>().Add(dtoItemToSave);
                    }
                }

                ((UnitOfWork)this.UnitOfWork).DataContext.SaveChanges();
            }

            return itemToSave;
        }

        /// <summary>
        /// Remove the blog entry
        /// </summary>
        /// <param name="saveItem"></param>
        public override bool Delete(DomainClass itemToDelete)
        {
            bool retVal = false;

            if (itemToDelete != null)
            {
                DTOClass dtoItemToDelete = this.GetDTOByDomain(itemToDelete);

                if (dtoItemToDelete != null)
                {
                    ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<DTOClass>().Remove(dtoItemToDelete);
                    ((UnitOfWork)this.UnitOfWork).DataContext.SaveChanges();
                }

                retVal = true;
            }

            return retVal;
        }

        public IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                System.Reflection.PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }
    }
}
