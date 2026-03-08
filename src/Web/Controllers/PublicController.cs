using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;
using AlwaysMoveForward.AnotherBlog.Web.Code.Utilities;
using AlwaysMoveForward.AnotherBlog.Web.Models;
using AlwaysMoveForward.AnotherBlog.Web.Models.BlogModels;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;
using AlwaysMoveForward.AnotherBlog.Web.Code.Extensions;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers;

public class PublicController : BaseController
{
    public PublicController(ServiceManagerBuilder serviceManagerBuilder)
        : base(serviceManagerBuilder)
    {
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        base.OnActionExecuting(filterContext);

        string blogSubFolder = "All";

        if (this.RouteData.Values.ContainsKey("blogSubFolder"))
        {
            blogSubFolder = this.RouteData.Values["blogSubFolder"]?.ToString() ?? "All";
        }
    }

    public Blog GetTargetBlog()
    {
        Blog retVal = null;

        if (this.RouteData.Values.ContainsKey("blogSubFolder"))
        {
            retVal = this.GetTargetBlog(this.RouteData.Values["blogSubFolder"]?.ToString() ?? string.Empty);
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
        retVal.RouteInformation = "/BlogPosts";
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
