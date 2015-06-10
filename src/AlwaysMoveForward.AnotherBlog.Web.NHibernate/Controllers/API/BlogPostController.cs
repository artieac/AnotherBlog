using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers.API
{
    public class BlogPostController : BaseApiController
    {
        [Route("api/BlogPosts"), HttpGet()]
        public IEnumerable<BlogPost> Get()
        {
            return this.Services.BlogEntryService.GetAll();
        }

        [Route("api/BlogPosts/{amountToGet}"), HttpGet()]
        public IEnumerable<BlogPost> Get(int amountToGet)
        {
            return this.Services.BlogEntryService.GetMostRecent(amountToGet);
        }

        [Route("api/{blogSubFolder}/BlogPosts"), HttpGet()]
        public BlogPost Get(string blogSubFolder)
        {
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            return this.Services.BlogEntryService.GetMostRecent(targetBlog);
        }

        // GET api/<blogSubFolder>/<controller>/5
        [Route("api/{blogSubFolder}/BlogPost/{id}"), HttpGet()]
        public BlogPost Get(string blogSubFolder, int id)
        {
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            return this.Services.BlogEntryService.GetById(targetBlog, id);
        }

        // GET api/<blogSubFolder>/<controller>/1999/1
        [Route("api/{blogSubFolder}/BlogPost/{year}/{month}"), HttpGet()]
        public IList<BlogPost> Get(string blogSubFolder, int year, int month)
        {
            DateTime targetDate = new DateTime(year, month, 1);
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            return this.Services.BlogEntryService.GetByMonth(targetBlog, targetDate, true);
        }

        // GET api/<blogSubFolder>/<controller>/1999/1/1
        [Route("api/{blogSubFolder}/BlogPost/{year}/{month}/{day}"), HttpGet()]
        public IList<BlogPost> Get(string blogSubFolder, int year, int month, int day)
        {
            DateTime targetDate = new DateTime(year, month, day);
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            return this.Services.BlogEntryService.GetByDate(targetBlog, targetDate, true);
        }

        // GET api/<blogSubFolder>/<controller>/1999/1/1/title
        [Route("api/{blogSubFolder}/BlogPost/{year}/{month}/{day}/{title}"), HttpGet()]
        public BlogPost Get(string blogSubfolder, int year, int month, int day, string title)
        {
            DateTime targetDate = new DateTime(year, month, day);
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubfolder);
            return this.Services.BlogEntryService.GetByDateAndTitle(targetBlog, targetDate, title);
        }

        // POST api/<blogSubFolder>/<controller>
        [Route("api/{blogSubFolder}/BlogPost"), HttpPost()]
        [WebAPIAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<blogSubFolder>/<controller>/5
        [Route("api/{blogSubFolder}/BlogPost/{id}"), HttpPut()]
        [WebAPIAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<blogSubFolder>/<controller>/5
        [Route("api/{blogSubFolder}/BlogPost/{id}"), HttpDelete()]
        [WebAPIAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public void Delete(int id)
        {
        }
    }
}