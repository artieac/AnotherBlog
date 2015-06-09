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
using System.Collections.Generic;
using System.Collections.Specialized;

using NHibernate.Collection.Generic;
using NHibernate.Engine;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Utilities
{
    public class PersistentDomainCollection<T>:PersistentGenericBag<T>, IDomainCollection<T> where T : class
    {    
        #region constructors    
        public PersistentDomainCollection(ISessionImplementor session, IList<T> coll) : base(session, coll)    
        {    }    
        
        public PersistentDomainCollection(ISessionImplementor session) : base(session)    
        {    }    
        #endregion    
        
        #region INotifyCollectionChanged Members    
        public event NotifyCollectionChangedEventHandler CollectionChanged;    
        /// <summary>    
        /// Fires the <see cref="CollectionChanged"/> event to indicate an item has been    
        /// added to the end of the collection.    
        /// </summary>    
        /// <param name="item">Item added to the collection.</param>    
        protected void OnItemAdded(T item)    
        {        
            if (this.CollectionChanged != null)        
            {            
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add, item, this.Count - 1));        
            }    
        }    
        /// <summary>    
        /// Fires the <see cref="CollectionChanged"/> event to indicate the collection    
        /// has been reset.  This is used when the collection has been cleared or    
        /// entirely replaced.    
        /// </summary>    
        protected void OnCollectionReset()    
        {        
            if (this.CollectionChanged != null)        
            {            
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Reset));        
            }    
        }    
        /// <summary>    
        /// Fires the <see cref="CollectionChanged"/> event to indicate an item has    
        /// been inserted into the collection at the specified index.    
        /// </summary>    
        /// <param name="index">Index the item has been inserted at.</param>    
        /// <param name="item">Item inserted into the collection.</param>    
        protected void OnItemInserted(int index, T item)    
        {        
            if (this.CollectionChanged != null)        
            {            
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add, item, index));        
            }    
        }    
        /// <summary>    
        /// Fires the <see cref="CollectionChanged"/> event to indicate an item has    
        /// been removed from the collection at the specified index.    
        /// </summary>    
        /// <param name="item">Item removed from the collection.</param>    
        /// <param name="index">Index the item has been removed from.</param>    
        protected void OnItemRemoved(T item, int index)    
        {        
            if (this.CollectionChanged != null)        
            {            
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove, item, index));        
            }    
        }    
        
        #endregion    
        /// <summary>    
        /// we need to re-implement the IList methods to support observability    
        /// </summary>    
        /// <param name="item"></param>    
        #region IList<T> members    
        
        public void Add(T item)    
        {        
            base.Add(item);        
            this.OnItemAdded(item);    
        }    
        
        public new void Clear()    
        {        
            base.Clear();        
            this.OnCollectionReset();    
        }    
        
        public void Insert(int index, T item)    
        {        
            base.Insert(index, item);        
            this.OnItemInserted(index, item);    
        }    
        
        public bool Remove(T item)    
        {        
            int index = this.IndexOf(item);        
            base.Remove(item);        
            this.OnItemRemoved(item, index);        
            return true;    
        }    
        
        public new void RemoveAt(int index)    
        {        
            T item = this[index] as T;        
            base.RemoveAt(index);        
            this.OnItemRemoved(item, index);    
        }    
        
        #endregion
    }
}
