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
using System.Collections;
using System.Collections.Generic;

using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.UserTypes;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Utilities
{
    public class DomainCollectionFactory<T> :IUserCollectionType where T : class
    {    
        #region IUserCollectionType Members    


        public object Instantiate(int anticipatedSize)
        {
            throw new System.NotImplementedException();
        }

        public IPersistentCollection Instantiate(ISessionImplementor session, ICollectionPersister persister)    
        {        
            return new PersistentDomainCollection<T>(session);    
        }    
        
        public IPersistentCollection Wrap(ISessionImplementor session, object collection)    
        {        
            return new PersistentDomainCollection<T>(session,collection as IList<T>);    
        }    
        
        public object Instantiate()    
        {        
            return new TransientDomainCollection<T>();    
        }    
        
        public IEnumerable GetElements(object collection)    
        {        
            return (IEnumerable) collection;    
        }    
        
        public bool Contains(object collection, object entity)    
        {        
            return ((IList) collection).Contains(entity);    
        }    
        
        public object IndexOf(object collection, object entity)    
        {        
            return ((IList) collection).IndexOf(entity);    
        }    
        
        public object ReplaceElements(object original, object target, ICollectionPersister persister,  object owner, IDictionary copyCache, ISessionImplementor session)    
        {        
            IList result = (IList) target;        
            result.Clear();        
            
            foreach (object o in ((IEnumerable) original))        
            {            
                result.Add(o);        
            }        
            
            return result;    
        }    
        
        #endregion
    }
}
