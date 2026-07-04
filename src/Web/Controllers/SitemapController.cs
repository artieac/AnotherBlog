using Microsoft.AspNetCore.Mvc;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using System.Text;
using System.Xml.Linq;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers;

public class SitemapController : PublicController
{
    public SitemapController(ServiceManagerBuilder serviceManagerBuilder)
        : base(serviceManagerBuilder)
    {
    }

    [Route("sitemap.xml")]
    [HttpGet]
    public IActionResult Index()
    {
        XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        var urlset = new XElement(xmlns + "urlset");

        var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value}";

        // Add home page
        urlset.Add(new XElement(xmlns + "url",
            new XElement(xmlns + "loc", $"{baseUrl}/"),
            new XElement(xmlns + "changefreq", "daily"),
            new XElement(xmlns + "priority", "1.0")));

        // Add about page
        urlset.Add(new XElement(xmlns + "url",
            new XElement(xmlns + "loc", $"{baseUrl}/Home/About"),
            new XElement(xmlns + "changefreq", "monthly"),
            new XElement(xmlns + "priority", "0.5")));

        // Get all blogs and their posts
        IList<Blog> allBlogs = this.Services.BlogService.GetAll();
        foreach (var blog in allBlogs)
        {
            // Add blog home page
            urlset.Add(new XElement(xmlns + "url",
                new XElement(xmlns + "loc", $"{baseUrl}/Blog/{blog.SubFolder}"),
                new XElement(xmlns + "changefreq", "daily"),
                new XElement(xmlns + "priority", "0.8")));

            IList<BlogPost> blogEntries = Services.BlogEntryService.GetAllByBlog(blog, true);
            foreach (var post in blogEntries)
            {
                urlset.Add(new XElement(xmlns + "url",
                    new XElement(xmlns + "loc", $"{baseUrl}/Blog/{blog.SubFolder}/Post/{post.Id}"),
                    new XElement(xmlns + "lastmod", post.DatePosted.ToString("yyyy-MM-dd")),
                    new XElement(xmlns + "changefreq", "monthly"),
                    new XElement(xmlns + "priority", "0.6")));
            }
        }

        var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), urlset);
        
        return Content(doc.Declaration.ToString() + "\n" + doc.ToString(), "application/xml", Encoding.UTF8);
    }
}
