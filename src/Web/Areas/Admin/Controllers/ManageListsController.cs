using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PucksAndProgramming.Common.Utilities;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.BusinessLayer.Service;
using PucksAndProgramming.AnotherBlog.Web.Areas.Admin.Models;
using PucksAndProgramming.AnotherBlog.Web.Code.Utilities;
using PucksAndProgramming.AnotherBlog.Web.Controllers;
using PucksAndProgramming.AnotherBlog.Web.Code.Filters;

namespace PucksAndProgramming.AnotherBlog.Web.Areas.Admin.Controllers
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
