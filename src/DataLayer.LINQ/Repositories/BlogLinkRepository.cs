using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Criterion;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;

using AnotherBlog.Data.Entities;

namespace AnotherBlog.Data.ActiveRecord.Repositories
{
    /// <summary>
    /// The BlogRoll is used to contain all links related to the blog.  This gateway class
    /// contains all the LINQ code to perform the CRUD operations on the class.
    /// </summary>
    public class BlogRollGateway : RepositoryBase
    {
        public BlogRollGateway(DataContextManager dataContext)
            : base(dataContext)
        {

        }
        /// <summary>
        /// Get all blog roll inks for a specified blog.
        /// </summary>
        /// <param name="targetBlog"></param>
        /// <returns></returns>
        public IList<BlogRollLink> GetAllByBlogId(Blog targetBlog)
        {
            DetachedCriteria criteria = DetachedCriteria.For<BlogEntry>();
            criteria.Add(Expression.Eq("BlogId", targetBlog.BlogId));

            return Castle.ActiveRecord.ActiveRecordMediator<BlogRollLink>.FindAll(criteria);
        }
        /// <summary>
        /// Get a specific blog roll link as specified by the URL (where is this called from, seems a bit silly if we already know the URL why look it up?)
        /// </summary>
        /// <param name="targetBlog"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public BlogRollLink GetByUrlAndBlogId(Blog targetBlog, string url)
        {
            SimpleQuery<BlogRollLink> query = new SimpleQuery<BlogRollLink>(@"from BlogRollLinks where BlogRollLinks.BlogId = ? AND BlogRollLinks.Url = ?", targetBlog.BlogId, url);
            return query.Execute().First();
        }
        /// <summary>
        /// Save the blog roll link.
        /// </summary>
        /// <param name="targetBlog"></param>
        /// <param name="itemToSave"></param>
        /// <returns></returns>
        public BlogRollLink Save(BlogRollLink itemToSave)
        {
            if (itemToSave != null)
            {
                try
                {
                    Castle.ActiveRecord.ActiveRecordMediator<BlogRollLink>.Save(itemToSave);
                    SessionScope.Current.Flush();
                }
                catch (Exception e)
                {
                    this.Logger.Error(e.Message, e);
                }
            }

            return itemToSave;
        }
    }
}
