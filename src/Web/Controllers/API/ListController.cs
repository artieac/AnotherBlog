using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;
using AlwaysMoveForward.AnotherBlog.Web.Models.API;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers.API
{
    public class ListController : BaseApiController
    {
        [Route("api/Blog/{blogSubFolder}/Lists"), HttpGet()]
        public IEnumerable<BlogList> Get(string blogSubFolder)
        {
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            return this.Services.BlogListService.GetByBlog(targetBlog);
        }

        [Route("api/Blog/{blogSubFolder}/List/{id:int}"), HttpGet()]
        public BlogList Get(string blogSubFolder, int id)
        {
            return this.Services.BlogListService.GetById(id);
        }

        [Route("api/Blog/{blogSubFolder}/List"), HttpPost()]
        public BlogList Post(string blogSubFolder, [FromBody]ListInputModel input)
        {
            BlogList retVal = new BlogList();
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);

            if (targetBlog != null)
            {
                using (this.Services.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        retVal = Services.BlogListService.Save(targetBlog, -1, input.Name, input.ShowOrdered);
                        this.Services.UnitOfWork.EndTransaction(true);
                    }
                    catch (Exception e)
                    {
                        LogManager.GetLogger().Error(e);
                        this.Services.UnitOfWork.EndTransaction(false);
                    }
                }                
            }

            return retVal;
        }

        [Route("api/Blog/{blogSubFolder}/List/{id:int}"), HttpPut()]
        public BlogList Put(string blogSubFolder, int id, [FromBody]ListInputModel input)
        {
            BlogList retVal = null;
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);

            if (targetBlog != null)
            {
                using (this.Services.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        retVal = Services.BlogListService.Save(targetBlog, id, input.Name, input.ShowOrdered);
                        this.Services.UnitOfWork.EndTransaction(true);
                    }
                    catch (Exception e)
                    {
                        LogManager.GetLogger().Error(e);
                        this.Services.UnitOfWork.EndTransaction(false);
                    }
                }
            }

            return retVal;
        }

        [Route("api/Blog/{blogSubFolder}/List/{id:int}/Item"), HttpPost()]
        public BlogList Post(string blogSubFolder, int id, [FromBody]ListItemInputModel input)
        {
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            BlogList retVal = this.Services.BlogListService.GetById(id);

            if (retVal != null)
            {
                using (this.Services.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        retVal = this.Services.BlogListService.UpdateItem(retVal, -1, input.Name, input.RelatedLink, input.DisplayOrder);
                        this.Services.UnitOfWork.EndTransaction(true);
                    }
                    catch (Exception e)
                    {
                        LogManager.GetLogger().Error(e);
                        this.Services.UnitOfWork.EndTransaction(false);
                    }
                }
            }

            return retVal;
        }

        [Route("api/Blog/{blogSubFolder}/List/{id:int}/Item/{itemId:int}"), HttpPut()]
        public BlogList Put(string blogSubFolder, int id, int itemId, [FromBody]ListItemInputModel input)
        {
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            BlogList retVal = this.Services.BlogListService.GetById(id);

            if (retVal != null)
            {
                using (this.Services.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        retVal = this.Services.BlogListService.UpdateItem(retVal, itemId, input.Name, input.RelatedLink, input.DisplayOrder);
                        this.Services.UnitOfWork.EndTransaction(true);
                    }
                    catch (Exception e)
                    {
                        LogManager.GetLogger().Error(e);
                        this.Services.UnitOfWork.EndTransaction(false);
                    }
                }
            }

            return retVal;
        }

        [Route("api/Blog/{blogSubFolder}/List/{id:int}"), HttpDelete()]
        public bool Delete(string blogSubFolder, int id)
        {
            bool retVal = false;
            BlogList blogList = this.Services.BlogListService.GetById(id);

            if (blogList != null)
            {
                using (this.Services.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        retVal = Services.BlogListService.Delete(blogList);
                        this.Services.UnitOfWork.EndTransaction(true);
                    }
                    catch (Exception e)
                    {
                        LogManager.GetLogger().Error(e);
                        this.Services.UnitOfWork.EndTransaction(false);
                    }
                }
            }

            return retVal;
        }

        [Route("api/Blog/{blogSubFolder}/List/{id:int}/Item/{itemId:int}"), HttpDelete()]
        public BlogList Delete(string blogSubFolder, int id, int itemId)
        {
            BlogList retVal = null;

            using (this.Services.UnitOfWork.BeginTransaction())
            {
                try
                {
                    retVal = this.Services.BlogListService.GetById(id);

                    if (retVal != null)
                    {
                        retVal.RemoveItem(itemId);
                    }

                    retVal = this.Services.BlogListService.Save(retVal);
                    this.Services.UnitOfWork.EndTransaction(true);
                }
                catch (Exception e)
                {
                    LogManager.GetLogger().Error(e);
                    this.Services.UnitOfWork.EndTransaction(false);
                }
            }

            return retVal;
        }
    }
}