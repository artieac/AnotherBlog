using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers.API
{
    public class BlogPostController : BaseApiController
    {
        [Route("api/BlogPosts"), HttpGet()]
        public IEnumerable<BlogPost> Get()
        {
            return this.Services.BlogEntryService.GetAll();
        }

        // GET api/<controller>/<blogSubFolder>/5
        public BlogPost Get(string blogSubFolder, int id)
        {
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            return this.Services.BlogEntryService.GetById(targetBlog, id);
        }

        // GET api/<controller>/<blogSubFolder>/1999/1/published
        public IList<BlogPost> Get(string blogSubFolder, int year, int month, bool? published)
        {
            bool isPublished = false;

            DateTime targetDate = new DateTime(year, month, 1);
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            return this.Services.BlogEntryService.GetByMonth(targetBlog, targetDate, true);
        }

        // GET api/<controller>/<blogSubFolder>/1999/1/1
        public IList<BlogPost> Get(string blogSubFolder, int year, int month, int day)
        {
            DateTime targetDate = new DateTime(year, month, day);
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            return this.Services.BlogEntryService.GetByDate(targetBlog, targetDate, true);
        }

        [Route]
        // GET api/<controller>/<blogsubfolder>/1999/1/1/title
        public BlogPost Get(string blogSubfolder, int year, int month, int day, string title)
        {
            DateTime targetDate = new DateTime(year, month, day);
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubfolder);
            return this.Services.BlogEntryService.GetByDateAndTitle(targetBlog, targetDate, title);
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}