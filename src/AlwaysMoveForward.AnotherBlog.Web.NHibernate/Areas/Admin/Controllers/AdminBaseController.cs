using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.Web.Controllers;
using AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Models;

namespace AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Controllers
{
    public class AdminBaseController : BaseController
    {
        public AdminCommon InitializeCommonModel()
        {
            return this.InitializeCommonModel(string.Empty);
        }

        public AdminCommon InitializeCommonModel(string targetBlog)
        {
            AdminCommon retVal = new AdminCommon();

            retVal.UserBlogs = Services.BlogService.GetByUserId(this.CurrentPrincipal.CurrentUser.Id);

            if (targetBlog != null)
            {
                retVal.TargetBlog = Services.BlogService.GetBySubFolder(targetBlog);
            }
            else
            {
                retVal.TargetBlog = null;
            }

            if (retVal.TargetBlog == null)
            {
                if (retVal.UserBlogs.Count > 0)
                {
                    retVal.TargetBlog = retVal.UserBlogs[0];
                }
            }

            retVal.SortColumn = string.Empty;
            retVal.SortAscending = true;
            return retVal;
        }
    }
}
