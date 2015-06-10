using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            string[] blogControllerNamespace = new string[] { "AlwaysMoveForward.AnotherBlog.Web.Controllers" };

            routes.MapRoute(
                "HomeMonthIndex",
                "Home/Month/{yearFilter}/{monthFilter}",
                new { controller = "Home", action = "Month", yearFilter = DateTime.Now.Year, monthFilter = DateTime.Now.Month},
                blogControllerNamespace);

            routes.MapRoute(
                "HomeDayIndex",
                "Home/Day/{yearFilter}/{monthFilter}/{dayFilter}",
                new { controller = "Home", action = "Day", yearFilter = DateTime.Now.Year, monthFilter = DateTime.Now.Month, dayFilter = DateTime.Now.Day },
                blogControllerNamespace
               );

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
                "BlogMonthIndex",
                "{blogSubFolder}/Month/{yearFilter}/{monthFilter}",
                new { controller = "Blog", action = "Month", yearFilter = DateTime.Now.Year, monthFilter = DateTime.Now.Month },
                blogControllerNamespace);

            routes.MapRoute(
                "BlogDayIndex",
                "{blogSubFolder}/Day/{yearFilter}/{monthFilter}/{dayFilter}",
                new { controller = "Blog", action = "Day", yearFilter = DateTime.Now.Year, monthFilter = DateTime.Now.Month, dayFilter = DateTime.Now.Day },
                blogControllerNamespace);

            routes.MapRoute(
                "BlogSpecificWithId",                                              // Route name
                "{blogSubFolder}/{controller}/{action}/{id}",                           // URL with parameters
                new { blogSubFolder = string.Empty, controller = string.Empty, action = string.Empty, id = "0"},   // Parameter defaults
                blogControllerNamespace);

            routes.MapRoute(
                "BlogEntry",                                              // Route name
                "{blogSubFolder}/{controller}/{action}/{year}/{month}/{day}/{title}",                           // URL with parameters
                new { blogSubFolder = string.Empty, controller = string.Empty, action = string.Empty, year = string.Empty, month = string.Empty, day = string.Empty, title = string.Empty },   // Parameter defaults
                blogControllerNamespace);
            );
        }
    }
}
