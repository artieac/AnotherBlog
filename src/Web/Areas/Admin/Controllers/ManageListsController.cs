using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Models;
using AlwaysMoveForward.AnotherBlog.Web.Code.Utilities;
using AlwaysMoveForward.AnotherBlog.Web.Controllers;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;

namespace AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Controllers
{
    [BlogMVCAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator)]
    public class ManageListsController : AdminBaseController
    {
        public ActionResult Index(string id)
        {
            BlogListModel model = new BlogListModel();
            model.Common = this.InitializeCommonModel(id);

            if (model.Common.TargetBlog != null)
            {
                model.BlogLists = Services.BlogListService.GetByBlog(model.Common.TargetBlog);
            }

            return this.View(model);
        }    
    }
}
