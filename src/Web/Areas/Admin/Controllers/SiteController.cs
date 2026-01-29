using Microsoft.AspNetCore.Mvc;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Models;
using AlwaysMoveForward.AnotherBlog.Web.Code.Utilities;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;

namespace AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class SiteController : AdminBaseController
{
    public SiteController(ServiceManagerBuilder serviceManagerBuilder)
        : base(serviceManagerBuilder)
    {
    }

    [AdminAuthorizationFilterAttribute(RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, false)]
    public IActionResult Landing()
    {
        SiteModel model = new SiteModel();
        return this.View(model);
    }

    [BlogMVCAuthorizationAttribute(RoleType.Names.SiteAdministrator)]
    public IActionResult Index()
    {
        SiteModel model = new SiteModel();
        model.SiteInfo = this.Services.SiteInfoService.GetSiteInfo();

        if (model.SiteInfo == null)
        {
            model.SiteInfo = new SiteInfo();
        }

        return this.View(model);
    }

    [BlogMVCAuthorizationAttribute(RoleType.Names.SiteAdministrator)]
    public IActionResult Edit(string blogSubFolder, string siteName, string siteAbout, string siteContact, string defaultTheme, string siteAnalyticsId, string defaultAuthor, string defaultKeywords)
    {
        if (string.IsNullOrEmpty(siteName))
        {
            ModelState.AddModelError("siteName", "Please enter a name for your site.");
        }

        if (string.IsNullOrEmpty(siteAbout))
        {
            ModelState.AddModelError("siteAbout", "Please enter an about message for your site.");
        }

        if (ModelState.IsValid)
        {
            using (this.Services.UnitOfWork.BeginTransaction())
            {
                try
                {
                    WebApplicationState.SiteInfo = Services.SiteInfoService.Save(siteName, siteAbout, siteContact, defaultTheme, siteAnalyticsId, defaultAuthor, defaultKeywords);
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
