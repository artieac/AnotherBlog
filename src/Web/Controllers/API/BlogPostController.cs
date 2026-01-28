using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;
using AlwaysMoveForward.AnotherBlog.Web.Models.API;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers.API;

[Route("api/[controller]")]
public class BlogPostController : BaseApiController
{
    [Route("/api/BlogPosts")]
    [HttpGet]
    public IEnumerable<BlogPost> Get()
    {
        return this.Services.BlogEntryService.GetAll();
    }

    [Route("/api/BlogPosts/{amountToGet:int}")]
    [HttpGet]
    [EnableCors]
    public IEnumerable<ExternalBlogPostModel> GetAmount(int amountToGet)
    {
        IList<ExternalBlogPostModel> retVal = new List<ExternalBlogPostModel>();

        IList<BlogPost> foundPosts = this.Services.BlogEntryService.GetMostRecent(amountToGet);

        foreach (BlogPost blogPost in foundPosts)
        {
            retVal.Add(new ExternalBlogPostModel(blogPost));
        }

        return retVal;
    }

    [Route("/api/BlogPost/MostRecent")]
    [HttpGet]
    [EnableCors]
    public ExternalBlogPostModel GetMostRecent()
    {
        ExternalBlogPostModel retVal = null;
        IList<BlogPost> foundPosts = this.Services.BlogEntryService.GetMostRecent(1);

        if (foundPosts != null && foundPosts.Count > 0)
        {
            retVal = new ExternalBlogPostModel(foundPosts[0]);
        }

        return retVal;
    }

    [Route("/api/Blog/{blogSubFolder}/BlogPosts")]
    [HttpGet]
    public BlogPost GetByBlog(string blogSubFolder)
    {
        Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
        return this.Services.BlogEntryService.GetMostRecent(targetBlog);
    }

    [Route("/api/Blog/{blogSubFolder}/BlogPost/{id:int}")]
    [HttpGet]
    public BlogPost GetById(string blogSubFolder, int id)
    {
        Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
        return this.Services.BlogEntryService.GetById(targetBlog, id);
    }

    [Route("/api/Blog/{blogSubFolder}/BlogPost/{year:int}/{month:int}")]
    [HttpGet]
    public IList<BlogPost> GetByMonth(string blogSubFolder, int year, int month)
    {
        DateTime targetDate = new DateTime(year, month, 1);
        Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
        return this.Services.BlogEntryService.GetByMonth(targetBlog, targetDate, true);
    }

    [Route("/api/Blog/{blogSubFolder}/BlogPost/{year:int}/{month:int}/{day:int}")]
    [HttpGet]
    public IList<BlogPost> GetByDay(string blogSubFolder, int year, int month, int day)
    {
        DateTime targetDate = new DateTime(year, month, day);
        Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
        return this.Services.BlogEntryService.GetByDate(targetBlog, targetDate, true);
    }

    [Route("/api/Blog/{blogSubFolder}/BlogPost/{year:int}/{month:int}/{day:int}/{title}")]
    [HttpGet]
    public BlogPost GetByTitle(string blogSubfolder, int year, int month, int day, string title)
    {
        DateTime targetDate = new DateTime(year, month, day);
        Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubfolder);
        return this.Services.BlogEntryService.GetByDateAndTitle(targetBlog, targetDate, title);
    }

    [Route("/api/Blog/{blogSubFolder}/BlogPost")]
    [HttpPost]
    [WebAPIAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
    public BlogPost Post(string blogSubFolder, [FromBody] BlogPostInput input)
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

    [Route("/api/Blog/{blogSubFolder}/BlogPost/{id:int}")]
    [HttpPut]
    [WebAPIAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
    public BlogPost Put(string blogSubFolder, int id, [FromBody] BlogPostInput input)
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

    [Route("/api/{blogSubFolder}/BlogPost/{id:int}")]
    [HttpDelete]
    [WebAPIAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
    public void Delete(int id)
    {
    }
}
