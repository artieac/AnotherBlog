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
                "root",
                string.Empty,
                new { controller = "Home", action = "Index" });
        }
    }
}
