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
using System.Data.Linq;
using System.Linq.Expressions;

using log4net;
using log4net.Config;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.Map;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer.Entities;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public abstract class LINQRepository<DomainClass, DTOClass> : AutoMappedRepository<DomainClass, DTOClass> where DomainClass : class, new() where DTOClass : class, new()
    {
        public LINQRepository(IUnitOfWork _unitOfWork, IRepositoryManager repositoryManager) : 
            base(_unitOfWork, repositoryManager)
        {

        }

        public LambdaExpression GenerateSortBy(String sortColumn, bool sortAscending)
        {
            ParameterExpression sortParameter = Expression.Parameter(typeof(DTOClass), "sortParam");

            // Now we'll make our lambda function that returns the
            // "DateOfBirth" property by it's name.
            return Expression.Lambda<Func<DTOClass, object>>(Expression.Property(sortParameter, sortColumn), sortParameter);
        }

        public virtual string BlogIdPropertyName
        {
            get { return "BlogId"; }
        }

        protected DTOClass GetDtoById(object idValue)
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
                            this.IdPropertyName
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

        public override DomainClass GetByProperty(string propertyName, object idValue)
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

            DTOClass retVal = null;

            IQueryable<DTOClass> dtoItems = ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<DTOClass>().Where(whereExpression);

            if (dtoItems != null && dtoItems.Count() > 0)
            {
                retVal = dtoItems.Single();
            }

            return this.Map(retVal);
        }

        public override DomainClass GetByProperty(string propertyName, object idValue, int blogId)
        {
            ParameterExpression dtoParameter = Expression.Parameter(typeof(DTOClass), "dtoParam");

            Expression<Func<DTOClass, bool>> whereExpression = Expression.Lambda<Func<DTOClass, bool>>
            (
                Expression.And
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
                    Expression.Equal
                    (
                        Expression.Property
                        (
                            dtoParameter,
                            this.BlogIdPropertyName
                        ),
                        Expression.Constant(blogId)
                    )
                ),
                new[] { dtoParameter }
            );

            DTOClass retVal = null;
            IQueryable<DTOClass> dtoItems = ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<DTOClass>().Where(whereExpression);

            if (dtoItems != null && dtoItems.Count() > 0)
            {
                retVal = dtoItems.Single();
            }

            return this.Map(retVal);
        }

        public override IList<DomainClass> GetAll()
        {
            IQueryable<DTOClass> dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<DTOClass>() select foundItem;
            return this.Map(dtoList.ToList());
        }

        public override IList<DomainClass> GetAll(int blogId)
        {
            ParameterExpression dtoParameter = Expression.Parameter(typeof(DTOClass), "dtoParam");

            Expression<Func<DTOClass, bool>> whereExpression = Expression.Lambda<Func<DTOClass, bool>>
            (
                Expression.Equal
                (
                    Expression.Property
                    (
                            dtoParameter,
                            this.BlogIdPropertyName
                    ),
                    Expression.Constant(blogId)
                ),
                new[] { dtoParameter }
            );

            IQueryable<DTOClass> dtoList = ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<DTOClass>().Where(whereExpression);
            return this.Map(dtoList.ToList());
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
            return this.Map(dtoList.ToList());
        }

        public override IList<DomainClass> GetAllByProperty(string propertyName, object idValue, int blogId)
        {
            ParameterExpression dtoParameter = Expression.Parameter(typeof(DTOClass), "dtoParam");

            Expression<Func<DTOClass, bool>> whereExpression = Expression.Lambda<Func<DTOClass, bool>>
            (
                Expression.And
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
                    Expression.Equal
                    (
                        Expression.Property
                        (
                            dtoParameter,
                            this.BlogIdPropertyName
                        ),
                        Expression.Constant(blogId)
                    )
                ),
                new[] { dtoParameter }
            );

            IQueryable<DTOClass> dtoList = ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<DTOClass>().Where(whereExpression);
            return this.Map(dtoList.ToList());
        }

        public override DomainClass Save(DomainClass itemToSave)
        {
            DTOClass targetItem = this.GetDTOByDomain(itemToSave);

            if (targetItem == null)
            {
                targetItem = this.Map(itemToSave);
                ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<DTOClass>().InsertOnSubmit(targetItem);
            }
            else
            {
                targetItem = this.Map(itemToSave);
            }

            this.UnitOfWork.Flush();

            return this.Map(targetItem);
        }

        protected abstract DTOClass GetDTOByDomain(DomainClass targetItem);

        /// <summary>
        /// Remove the blog entry
        /// </summary>
        /// <param name="saveItem"></param>
        public override bool Delete(DomainClass itemToDelete)
        {
            bool retVal = false;

            DTOClass targetItem = this.GetDTOByDomain(itemToDelete);

            if (targetItem != null)
            {
                ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<DTOClass>().DeleteOnSubmit(targetItem);
                retVal = true;
                this.UnitOfWork.Flush();
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
