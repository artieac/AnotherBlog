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
    public class BlogController : BaseApiController
    {
        [Route("api/Blogs"), HttpGet()]
        public IEnumerable<Blog> Get()
        {
            return this.Services.BlogService.GetAll();
        }

        [Route("api/Blog/{id:int}"), HttpGet()]
        public Blog Get(int id)
        {
            return this.Services.BlogService.GetById(id);
        }

        [Route("api/Blog/{blogSubFolder}"), HttpGet()]
        public Blog Get(string blogSubFolder)
        {
            return this.Services.BlogService.GetBySubFolder(blogSubFolder);
        }

        [Route("api/Blog"), HttpPost()]
        public Blog Post([FromBody]BlogInputModel input)
        {
            Blog retVal = null;

            if (!string.IsNullOrEmpty(input.Name) &&
                !string.IsNullOrEmpty(input.SubFolder))
            {
                retVal = this.Services.BlogService.Save(-1, input.Name, input.SubFolder, input.Description, input.About, input.Welcome, input.Theme);
            }

            return retVal;
        }

        // PUT api/<controller>/5
        [Route("api/Blog/{id:int}"), HttpPut()]
        [WebAPIAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public Blog Put(int postId, [FromBody]BlogInputModel input)
        {
            Blog retVal = null;

            if (!string.IsNullOrEmpty(input.Name) &&
                !string.IsNullOrEmpty(input.SubFolder))
            {
                retVal = this.Services.BlogService.Save(-1, input.Name, input.SubFolder, input.Description, input.About, input.Welcome, input.Theme);
            }

            return retVal;
        }
    }
}