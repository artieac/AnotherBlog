using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using PucksAndProgramming.Common.Utilities;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Web.Code.Filters;
using PucksAndProgramming.AnotherBlog.Web.Models.API;

namespace PucksAndProgramming.AnotherBlog.Web.Controllers.API
{
    public class BlogPostController : BaseApiController
    {
        [Route("api/BlogPosts"), HttpGet()]
        public IEnumerable<BlogPost> Get()
        {
            return this.Services.BlogEntryService.GetAll();
        }

        [Route("api/BlogPosts/{amountToGet:int}"), HttpGet()]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IEnumerable<ExternalBlogPostModel> Get(int amountToGet)
        {
            IList<ExternalBlogPostModel> retVal = new List<ExternalBlogPostModel>();

            IList<BlogPost> foundPosts = this.Services.BlogEntryService.GetMostRecent(amountToGet);

            foreach(BlogPost blogPost in foundPosts)
            {
                retVal.Add(new ExternalBlogPostModel(blogPost));
            }

            return retVal;
        }

        [Route("api/BlogPost/MostRecent"), HttpGet()]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public ExternalBlogPostModel GetMostRecent()
        {
            ExternalBlogPostModel retVal = null;
            IList<BlogPost> foundPosts = this.Services.BlogEntryService.GetMostRecent(1);

            if(foundPosts != null && foundPosts.Count > 0)
            {
                retVal = new ExternalBlogPostModel(foundPosts[0]);
            }

            return retVal;
        }

        [Route("api/Blog/{blogSubFolder}/BlogPosts"), HttpGet()]
        public BlogPost Get(string blogSubFolder)
        {
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            return this.Services.BlogEntryService.GetMostRecent(targetBlog);
        }

        // GET api/<blogSubFolder>/<controller>/5
        [Route("api/Blog/{blogSubFolder}/BlogPost/{id:int}"), HttpGet()]
        public BlogPost Get(string blogSubFolder, int id)
        {
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            return this.Services.BlogEntryService.GetById(targetBlog, id);
        }

        // GET api/<blogSubFolder>/<controller>/1999/1
        [Route("api/Blog/{blogSubFolder}/BlogPost/{year:int}/{month:int}"), HttpGet()]
        public IList<BlogPost> Get(string blogSubFolder, int year, int month)
        {
            DateTime targetDate = new DateTime(year, month, 1);
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            return this.Services.BlogEntryService.GetByMonth(targetBlog, targetDate, true);
        }

        // GET api/<blogSubFolder>/<controller>/1999/1/1
        [Route("api/Blog/{blogSubFolder}/BlogPost/{year:int}/{month:int}/{day:int}"), HttpGet()]
        public IList<BlogPost> Get(string blogSubFolder, int year, int month, int day)
        {
            DateTime targetDate = new DateTime(year, month, day);
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            return this.Services.BlogEntryService.GetByDate(targetBlog, targetDate, true);
        }

        // GET api/<blogSubFolder>/<controller>/1999/1/1/title
        [Route("api/Blog/{blogSubFolder}/BlogPost/{year:int}/{month:int}/{day:int}/{title}"), HttpGet()]
        public BlogPost Get(string blogSubfolder, int year, int month, int day, string title)
        {
            DateTime targetDate = new DateTime(year, month, day);
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubfolder);
            return this.Services.BlogEntryService.GetByDateAndTitle(targetBlog, targetDate, title);
        }

        // POST api/<blogSubFolder>/<controller>
        [Route("api/Blog/{blogSubFolder}/BlogPost"), HttpPost()]
        [WebAPIAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public BlogPost Post(string blogSubFolder, [FromBody]BlogPostInput input)
        {
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            BlogPost retVal = new BlogPost();

            if (targetBlog != null)
            {
                using (this.Services.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        if (input.Tags == null)
                        {
                            input.Tags = string.Empty;
                        }

                        retVal = Services.BlogEntryService.Save(targetBlog, input.Title, input.Text, -1, input.IsPublished, input.Tags.Split(','));
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

        // PUT api/<blogSubFolder>/<controller>/5
        [Route("api/Blog/{blogSubFolder}/BlogPost/{id:int}")]
        [HttpPut]
        [WebAPIAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public BlogPost Put(string blogSubFolder, int id, [FromBody]BlogPostInput input)
        {
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            BlogPost retVal = new BlogPost();

            if (targetBlog != null)
            {                
                using (this.Services.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        if(input.Tags == null)
                        {
                            input.Tags = string.Empty;
                        }

                        retVal = Services.BlogEntryService.Save(targetBlog, input.Title, input.Text, id, input.IsPublished, input.Tags.Split(','));
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

        // DELETE api/<blogSubFolder>/<controller>/5
        [Route("api/{blogSubFolder}/BlogPost/{id:int}"), HttpDelete()]
        [WebAPIAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public void Delete(int id)
        {
        }
    }
}