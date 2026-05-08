using Microsoft.AspNetCore.Mvc;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Web.Controllers;

namespace AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Controllers;

[Area("Admin")]
[AdminAuthorizationFilterAttribute(RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, false)]
public class AppController : BaseController
{
    public AppController(ServiceManagerBuilder serviceManagerBuilder)
        : base(serviceManagerBuilder)
    {
    }

    public IActionResult Index()
    {
        return View();
    }
}
