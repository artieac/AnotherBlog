using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using AlwaysMoveForward.Common.Configuration;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;
using AlwaysMoveForward.AnotherBlog.Web.Code.Utilities;

namespace AlwaysMoveForward.AnotherBlog.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static SiteInfo siteInfo;
        private static WebSiteConfiguration siteConfig;

        static MvcApplication()
        {
            MvcApplication.siteConfig = (WebSiteConfiguration)System.Configuration.ConfigurationManager.GetSection("AnotherBlog/WebSiteConfiguration");
        }

        public static string Version
        {
            get { return "1.2.0"; }
        }

        public static WebSiteConfiguration WebSiteConfiguration
        {
            get { return MvcApplication.siteConfig; }
        }

        public static SiteInfo SiteInfo
        {
            get
            {
                if (MvcApplication.siteInfo == null)
                {
                    ServiceManager serviceManager = ServiceManagerBuilder.BuildServiceManager();

                    if (serviceManager != null)
                    {
                        MvcApplication.siteInfo = serviceManager.SiteInfoService.GetSiteInfo();

                        if (MvcApplication.siteInfo == null)
                        {
                            MvcApplication.siteInfo = new SiteInfo();
                            siteInfo.Name = "Default";
                        }
                    }
                    else
                    {

                        MvcApplication.siteInfo = new SiteInfo();
                        siteInfo.Name = "Default";
                    }
                }

                return MvcApplication.siteInfo;
            }
            set
            {
                MvcApplication.siteInfo = value;
            }
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

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
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}