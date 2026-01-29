using Microsoft.AspNetCore.Mvc;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Web.Models.API;

using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers.API;

[Route("api/[controller]")]
public class TagController : BaseApiController
{
    public TagController(ServiceManagerBuilder serviceManagerBuilder)
        : base(serviceManagerBuilder)
    {
    }

    [Route("/api/Blog/{blogSubFolder}/Tags")]
    [HttpGet]
    public IList<TagCountModel> Get(string blogSubFolder)
    {
        IList<TagCountModel> retVal = new List<TagCountModel>();

        Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);

        if (targetBlog != null)
        {
            System.Collections.IList tagCounts = new System.Collections.ArrayList();
            tagCounts = this.Services.TagService.GetAllWithCount(targetBlog);

            foreach (TagCount tagCount in tagCounts)
            {
                retVal.Add(new TagCountModel(blogSubFolder, tagCount));
            }
        }

        return retVal;
    }
}
