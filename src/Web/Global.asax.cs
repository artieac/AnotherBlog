using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using PucksAndProgramming.Common.Configuration;
using PucksAndProgramming.Common.DataLayer;
using PucksAndProgramming.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.BusinessLayer.Service;
using PucksAndProgramming.AnotherBlog.BusinessLayer.Utilities;
using PucksAndProgramming.AnotherBlog.Web.Code.Utilities;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace PucksAndProgramming.AnotherBlog.Web
{
    public class MvcApplication : HttpApplication
    {
        private static SiteInfo siteInfo;
        private static WebSiteConfiguration siteConfig;

        static MvcApplication()
        {
            MvcApplication.siteConfig = WebSiteConfiguration.GetInstance();
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

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
        }
    }
}