using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;
using AlwaysMoveForward.AnotherBlog.Web.Models.API;

using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers.API;

[Route("api/[controller]")]
public class BlogController : BaseApiController
{
    public BlogController(ServiceManagerBuilder serviceManagerBuilder)
        : base(serviceManagerBuilder)
    {
    }

    [Route("/api/Blogs")]
    [HttpGet]
    public IEnumerable<Blog> Get()
    {
        return this.Services.BlogService.GetAll();
    }

    [Route("/api/Blog/{id:int}")]
    [HttpGet]
    public Blog GetById(int id)
    {
        return this.Services.BlogService.GetById(id);
    }

    [Route("/api/Blog/{blogSubFolder}")]
    [HttpGet]
    public Blog GetBySubFolder(string blogSubFolder)
    {
        return this.Services.BlogService.GetBySubFolder(blogSubFolder);
    }

    [Route("/api/Blog")]
    [HttpPost]
    public Blog Post([FromBody] BlogInputModel input)
    {
        Blog retVal = null;

        if (!string.IsNullOrEmpty(input.Name) &&
            !string.IsNullOrEmpty(input.SubFolder))
        {
            retVal = this.Services.BlogService.Save(-1, input.Name, input.SubFolder, input.Description, input.About, input.Welcome, input.Theme);
        }

        return retVal;
    }

    [Route("/api/Blog/{id:int}")]
    [HttpPut]
    [WebAPIAuthorizationAttribute(RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, true)]
    public Blog Put(int id, [FromBody] BlogInputModel input)
    {
        Blog retVal = null;

        if (this.CurrentPrincipal.CurrentUser.IsSiteAdministrator == true)
        {
            if (!string.IsNullOrEmpty(input.Name) &&
                !string.IsNullOrEmpty(input.SubFolder))
            {
                retVal = this.Services.BlogService.Save(id, input.Name, input.SubFolder, input.Description, input.About, input.Welcome, input.Theme);
            }
        }
        else
        {
            Blog targetBlog = this.Services.BlogService.GetById(id);

            if (targetBlog == null)
            {
                if (this.CurrentPrincipal.IsInRole(RoleType.Names.Administrator, targetBlog.SubFolder) ||
                    this.CurrentPrincipal.IsInRole(RoleType.Names.Blogger, targetBlog.SubFolder))
                {
                    retVal = this.Services.BlogService.Save(id, input.Description, input.About, input.Welcome);
                }
            }
        }

        return retVal;
    }
}
