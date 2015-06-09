using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Models;
using AlwaysMoveForward.AnotherBlog.Web.Code.Utilities;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;

namespace AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Controllers
{
    [RequestAuthenticationFilter]
    public class SiteController : AdminBaseController
    {
        [AdminAuthorizationFilter(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = false)]
        public ActionResult Landing()
        {
            SiteModel model = new SiteModel();
            return this.View(model);
        }

        [CustomAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator)]
        public ActionResult Index()
        {
            SiteModel model = new SiteModel();
            model.SiteInfo = this.Services.SiteInfoService.GetSiteInfo();

            if (model.SiteInfo == null)
            {
                model.SiteInfo = new SiteInfo();
            }

            return this.View(model);
        }

        [CustomAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator)]
        public ActionResult Edit(string blogSubFolder, string siteName, string siteAbout, string siteContact, string defaultTheme, string siteAnalyticsId)
        {
            if (string.IsNullOrEmpty(siteName))
            {
                ViewData.ModelState.AddModelError("siteName", "Please enter a name for your site.");
            }

            if (string.IsNullOrEmpty(siteAbout))
            {
                ViewData.ModelState.AddModelError("siteAbout", "Please enter an about message for your site.");
            }

            if (ViewData.ModelState.IsValid)
            {
                using (this.Services.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        MvcApplication.SiteInfo = Services.SiteInfoService.Save(siteName, siteAbout, siteContact, defaultTheme, siteAnalyticsId);
                        this.Services.UnitOfWork.EndTransaction(true);
                    }
                    catch (Exception e)
                    {
                        LogManager.GetLogger().Error(e);
                        this.Services.UnitOfWork.EndTransaction(false);
                    }
                }
            }

            return this.RedirectToAction("Index");
        }
    }
}
