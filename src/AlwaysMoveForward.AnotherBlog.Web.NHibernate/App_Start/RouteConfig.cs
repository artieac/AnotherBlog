using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AlwaysMoveForward.AnotherBlog.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = true;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes(); 

            string[] blogControllerNamespace = new string[] { "AlwaysMoveForward.AnotherBlog.Web.Controllers" };

            routes.MapRoute(
                "BlogTagSearch",
                "{blogSubFolder}/Tag/{targetTag}",
                new { blogSubFolder = string.Empty, controller = "Blog", action = "Tag", targetTag = string.Empty },   // Parameter defaults
                blogControllerNamespace);

            routes.MapRoute(
                "root",
                string.Empty,
                new { controller = "Home", action = "Index" });

            routes.MapRoute(
                "Default",
                "{controller}/{action}",
                new { controller = string.Empty, action = string.Empty });

            routes.MapRoute(
                "BlogSpecific",                                              // Route name
                "{blogSubFolder}/{controller}/{action}",                           // URL with parameters
                new { blogSubFolder = string.Empty, controller = string.Empty, action = string.Empty },   // Parameter defaults
                blogControllerNamespace);

            routes.MapRoute(
                "BlogSpecificWithId",                                              // Route name
                "{blogSubFolder}/{controller}/{action}/{id}",                           // URL with parameters
                new { blogSubFolder = string.Empty, controller = string.Empty, action = string.Empty, id = "0" },   // Parameter defaults
                blogControllerNamespace);

            routes.MapRoute(
                "BlogEntry",                                              // Route name
                "{blogSubFolder}/{controller}/{action}/{year}/{month}/{day}/{title}",                           // URL with parameters
                new { blogSubFolder = string.Empty, controller = string.Empty, action = string.Empty, year = string.Empty, month = string.Empty, day = string.Empty, title = string.Empty },   // Parameter defaults
                blogControllerNamespace);
        }
    }
}
