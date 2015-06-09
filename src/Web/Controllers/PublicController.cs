using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Security.Permissions;

using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;
using AlwaysMoveForward.AnotherBlog.Web.Code.Utilities;
using AlwaysMoveForward.AnotherBlog.Web.Models;
using AlwaysMoveForward.AnotherBlog.Web.Models.BlogModels;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;
using AlwaysMoveForward.AnotherBlog.Web.Code.Extensions;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers
{
    [RequestAuthenticationFilter]
    public class PublicController : BaseController
    {
        public PublicController()
            : base()
        {

        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            string blogSubFolder = "All";

            if (this.ControllerContext.RouteData.Values.ContainsKey("blogSubFolder"))
            {
                blogSubFolder = this.ControllerContext.RouteData.Values["blogSubFolder"].ToString();
            }
        }

        public Blog GetTargetBlog()
        {
            Blog retVal = null;

            if (this.ControllerContext.RouteData.Values.ContainsKey("blogSubFolder"))
            {
                retVal = this.GetTargetBlog(this.ControllerContext.RouteData.Values["blogSubFolder"].ToString());
            }

            return retVal;
        }

        public Blog GetTargetBlog(string blogSubFolder)
        {
            return Services.BlogService.GetBySubFolder(blogSubFolder);
        }

        public CommonModel InitializeCommonModel()
        {
            CommonModel retVal = new CommonModel();
            return this.InitializeCommonModel(null, retVal);
        }

        public CommonModel InitializeCommonModel(Blog targetBlog)
        {
            CommonModel retVal = new CommonModel();
            return this.InitializeCommonModel(targetBlog, retVal);
        }

        public CommonModel InitializeCommonModel(Blog targetBlog, CommonModel instanceToPopulate)
        {
            CommonModel retVal = new CommonModel();

            retVal.TargetMonth = DateTime.Now;

            return retVal;
        }

        public CalendarModel InitializeCalendarModel(DateTime targetMonth)
        {
            CalendarModel retVal = new CalendarModel();
            retVal.TargetBlog = null;
            retVal.RouteInformation = "/Home";
            retVal.TargetMonth = targetMonth;
            retVal.CurrentMonthBlogDates = new List<DateTime>();

            IList<DateTime> blogDates = Services.BlogEntryService.GetPublishedDatesByMonth(retVal.TargetMonth);

            for (int i = 0; i < blogDates.Count; i++)
            {
                retVal.CurrentMonthBlogDates.Add(blogDates[i].Date);
            }

            return retVal;
        }
    }
}
