using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers.API
{
    public class TagController : BaseApiController
    {
        [Route("api/Blog/{blogSubFolder}/Tags")]
        public System.Collections.IList Get(string blogSubFolder)
        {
            System.Collections.IList model = new System.Collections.ArrayList();

            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);

            if (targetBlog != null)
            {
                model = this.Services.TagService.GetAllWithCount(targetBlog);
            }

            return model;
        }
    }
}
