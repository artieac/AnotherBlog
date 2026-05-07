using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;
using AlwaysMoveForward.AnotherBlog.Web.Models.API;

using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers.API;

[Route("api/[controller]")]
public class BlogPostController : BaseApiController
{
    public BlogPostController(ServiceManagerBuilder serviceManagerBuilder)
        : base(serviceManagerBuilder)
    {
    }

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

    [Route("/api/Blog/{blogSubFolder}/BlogPosts/All")]
    [HttpGet]
    [WebAPIAuthorizationAttribute(RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, true)]
    public IEnumerable<BlogPost> GetAllByBlog(string blogSubFolder)
    {
        Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
        return this.Services.BlogEntryService.GetAllByBlog(targetBlog, false);
    }

    [Route("/api/Blog/{blogSubFolder}/BlogPost/{id:int}")]
    [HttpGet]
    public BlogPost GetById(string blogSubFolder, int id)
    {
        Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
        BlogPost retVal = this.Services.BlogEntryService.GetById(targetBlog, id);
        Console.WriteLine($"Retrieved blog post ID: {id}. Tag count: {retVal?.Tags?.Count ?? 0}");
        return retVal;
    }

    [Route("/api/Blog/{blogSubFolder}/BlogPost/{id:int}/Tags")]
    [HttpGet]
    public IEnumerable<Tag> GetTags(string blogSubFolder, int id)
    {
        Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
        BlogPost post = this.Services.BlogEntryService.GetById(targetBlog, id);
        return post?.Tags ?? new List<Tag>();
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
    [WebAPIAuthorizationAttribute(RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, true)]
    public BlogPost Post(string blogSubFolder, [FromBody] BlogPostInput input)
    {
        Console.WriteLine($"Attempting to create blog post for blog: {blogSubFolder}");
        Console.WriteLine($"Input Title: {input.Title}");
        
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

                    Console.WriteLine($"Input Tags: {input.Tags}");
                    retVal = Services.BlogEntryService.Save(targetBlog, input.Title, input.Text, 0, input.IsPublished, input.Tags.Split(','), this.CurrentPrincipal.CurrentUser);
                    this.Services.UnitOfWork.EndTransaction(true);
                    Console.WriteLine($"Successfully saved blog post with ID: {retVal.Id} and Title: {retVal.Title}. Tag count: {retVal.Tags?.Count ?? 0}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error saving blog post: {e.Message}");
                    Console.WriteLine(e.StackTrace);
                    LogManager.GetLogger().Error(e);
                    this.Services.UnitOfWork.EndTransaction(false);
                }
            }
        }
        else
        {
            Console.WriteLine($"Blog not found for subfolder: {blogSubFolder}");
        }

        return retVal;
    }

    [Route("/api/Blog/{blogSubFolder}/BlogPost/{id:int}")]
    [HttpPut]
    [WebAPIAuthorizationAttribute(RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, true)]
    public BlogPost Put(string blogSubFolder, int id, [FromBody] BlogPostInput input)
    {
        Console.WriteLine($"Attempting to update blog post ID: {id} for blog: {blogSubFolder}");
        Console.WriteLine($"Input Title: {input.Title}");

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

                    Console.WriteLine($"Input Tags: {input.Tags}");
                    retVal = Services.BlogEntryService.Save(targetBlog, input.Title, input.Text, id, input.IsPublished, input.Tags.Split(','), this.CurrentPrincipal.CurrentUser);
                    this.Services.UnitOfWork.EndTransaction(true);
                    Console.WriteLine($"Successfully updated blog post with ID: {retVal.Id} and Title: {retVal.Title}. Tag count: {retVal.Tags?.Count ?? 0}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error updating blog post: {e.Message}");
                    Console.WriteLine(e.StackTrace);
                    LogManager.GetLogger().Error(e);
                    this.Services.UnitOfWork.EndTransaction(false);
                }
            }
        }

        return retVal;
    }

    [Route("/api/Blog/{blogSubFolder}/BlogPost/{id:int}")]
    [HttpDelete]
    [WebAPIAuthorizationAttribute(RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, true)]
    public void Delete(string blogSubFolder, int id)
    {
        Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);

        if (targetBlog != null)
        {
            BlogPost targetPost = this.Services.BlogEntryService.GetById(targetBlog, id);

            if (targetPost != null)
            {
                using (this.Services.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        this.Services.BlogEntryService.Delete(targetPost);
                        this.Services.UnitOfWork.EndTransaction(true);
                    }
                    catch (Exception e)
                    {
                        LogManager.GetLogger().Error(e);
                        this.Services.UnitOfWork.EndTransaction(false);
                    }
                }
            }
        }
    }
}
