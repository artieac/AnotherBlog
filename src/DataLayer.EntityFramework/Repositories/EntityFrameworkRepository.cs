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
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.DataLayer.Entities;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public abstract class EntityFrameworkRepository<TDomainType, TIdType>
        where TDomainType : class, new()
    {
        protected IUnitOfWork UnitOfWork { get; private set; }

        public EntityFrameworkRepository(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        public abstract string IdPropertyName { get; }

        public virtual string BlogIdPropertyName
        {
            get { return "BlogId"; }
        }

        public virtual string TableName
        {
            get { return typeof(TDomainType).Name + "s"; }
        }

        protected UnitOfWork GetUnitOfWork()
        {
            return (UnitOfWork)this.UnitOfWork;
        }

        protected DbSet<TDomainType> GetDbSet()
        {
            return this.GetUnitOfWork().DataContext.Set<TDomainType>();
        }

        public virtual TDomainType Create()
        {
            return new TDomainType();
        }

        public virtual TIdType GetId(TDomainType domainEntity)
        {
            return (TIdType)typeof(TDomainType).GetProperty(this.IdPropertyName).GetValue(domainEntity, null);
        }

        protected TDomainType GetEntityByProperty(string propertyName, object idValue)
        {
            TDomainType retVal = default(TDomainType);

            ParameterExpression entityParameter = Expression.Parameter(typeof(TDomainType), "entityParam");

            Expression<Func<TDomainType, bool>> whereExpression = Expression.Lambda<Func<TDomainType, bool>>
            (
                Expression.Equal
                (
                    Expression.Property(entityParameter, propertyName),
                    Expression.Constant(idValue)
                ),
                new[] { entityParameter }
            );

            IQueryable<TDomainType> entities = this.GetDbSet().Where(whereExpression);

            if (entities != null && entities.Any())
            {
                retVal = entities.First();
            }

            return retVal;
        }

        public virtual TDomainType GetById(TIdType id)
        {
            return this.GetEntityByProperty(this.IdPropertyName, id);
        }

        public virtual TDomainType GetByProperty(string propertyName, object idValue)
        {
            return this.GetEntityByProperty(propertyName, idValue);
        }

        public virtual TDomainType GetByProperty(string propertyName, object idValue, long blogId)
        {
            TDomainType retVal = default(TDomainType);

            string sql = "SELECT * FROM " + this.TableName;
            sql += " WHERE " + propertyName + " = {0}";
            sql += " AND " + this.BlogIdPropertyName + " = {1}";

            IEnumerable<TDomainType> entities = this.GetDbSet().FromSqlRaw(sql, idValue, blogId);

            if (entities != null && entities.Any())
            {
                retVal = entities.First();
            }

            return retVal;
        }

        public virtual IList<TDomainType> GetAll()
        {
            return this.GetDbSet().ToList();
        }

        public virtual IList<TDomainType> GetAll(long blogId)
        {
            string sql = "SELECT * FROM " + this.TableName;
            sql += " WHERE " + this.BlogIdPropertyName + " = {0}";

            return this.GetDbSet().FromSqlRaw(sql, blogId).ToList();
        }

        public virtual IList<TDomainType> GetAllByProperty(string propertyName, object idValue)
        {
            ParameterExpression entityParameter = Expression.Parameter(typeof(TDomainType), "entityParam");

            Expression<Func<TDomainType, bool>> whereExpression = Expression.Lambda<Func<TDomainType, bool>>
            (
                Expression.Equal
                (
                    Expression.Property(entityParameter, propertyName),
                    Expression.Constant(idValue)
                ),
                new[] { entityParameter }
            );

            return this.GetDbSet().Where(whereExpression).ToList();
        }

        public virtual IList<TDomainType> GetAllByProperty(string propertyName, object idValue, long blogId)
        {
            string sql = "SELECT * FROM " + this.TableName;
            sql += " WHERE " + propertyName + " = {0}";
            sql += " AND " + this.BlogIdPropertyName + " = {1}";

            return this.GetDbSet().FromSqlRaw(sql, idValue, blogId).ToList();
        }

        public virtual TDomainType Save(TDomainType itemToSave)
        {
            if (itemToSave != null)
            {
                TIdType id = this.GetId(itemToSave);
                TDomainType existingEntity = this.GetEntityByProperty(this.IdPropertyName, id);

                if (existingEntity == null)
                {
                    this.GetDbSet().Add(itemToSave);
                }
                else
                {
                    this.GetUnitOfWork().DataContext.Entry(existingEntity).CurrentValues.SetValues(itemToSave);
                }

                this.GetUnitOfWork().DataContext.SaveChanges();
            }

            return itemToSave;
        }

        public virtual bool Delete(TDomainType itemToDelete)
        {
            bool retVal = false;

            if (itemToDelete != null)
            {
                TIdType id = this.GetId(itemToDelete);
                TDomainType entity = this.GetEntityByProperty(this.IdPropertyName, id);

                if (entity != null)
                {
                    this.GetDbSet().Remove(entity);
                    this.GetUnitOfWork().DataContext.SaveChanges();
                    retVal = true;
                }
            }

            return retVal;
        }

        protected IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop);
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
