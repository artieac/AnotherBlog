using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Web.Models.API;

namespace PucksAndProgramming.AnotherBlog.Web.Controllers.API
{
    public class TagController : BaseApiController
    {
        [Route("api/Blog/{blogSubFolder}/Tags")]
        public IList<TagCountModel> Get(string blogSubFolder)
        {
            IList<TagCountModel> retVal = new List<TagCountModel>();

            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);

            if (targetBlog != null)
            {
                System.Collections.IList tagCounts = new System.Collections.ArrayList();
                tagCounts = this.Services.TagService.GetAllWithCount(targetBlog);

                foreach(TagCount tagCount in tagCounts)
                {
                    retVal.Add(new TagCountModel(blogSubFolder, tagCount));
                }
            }

            return retVal;
        }
    }
}
