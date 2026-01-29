using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.Common.DomainModel.Poll;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;
using AlwaysMoveForward.AnotherBlog.Web.Models.BlogModels;
using AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Models;
using AlwaysMoveForward.AnotherBlog.Web.Code.Utilities;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;

namespace AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class ManagePollsController : AdminBaseController
{
    private const int PollPageSize = 25;

    public ManagePollsController(ServiceManagerBuilder serviceManagerBuilder)
        : base(serviceManagerBuilder)
    {
    }

    [AdminAuthorizationFilterAttribute(RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, false)]
    public IActionResult Index(int? page)
    {
        int currentPageIndex = 0;

        if (page.HasValue == true)
        {
            currentPageIndex = page.Value - 1;
        }

        ManagePollsModel model = new ManagePollsModel();
        model.Common = this.InitializeCommonModel();
        model.Polls = X.PagedList.Extensions.PagedListExtensions.ToPagedList(this.Services.PollService.GetAll(), currentPageIndex + 1, PollPageSize);
        return this.View(model);
    }

    [AdminAuthorizationFilterAttribute(RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, false)]
    public IActionResult GetAll(int? page)
    {
        int currentPageIndex = 0;

        if (page.HasValue == true)
        {
            currentPageIndex = page.Value - 1;
        }

        X.PagedList.IPagedList<PollQuestion> retVal = X.PagedList.Extensions.PagedListExtensions.ToPagedList(this.Services.PollService.GetAll(), currentPageIndex + 1, PollPageSize);
        return this.Json(retVal);
    }

    [AdminAuthorizationFilterAttribute(RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, false)]
    public IActionResult GetById(int pollQuestionId)
    {
        PollQuestion retVal = this.Services.PollService.GetById(pollQuestionId);
        return this.Json(retVal);
    }

    [AdminAuthorizationFilterAttribute(RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, false)]
    public IActionResult Add(string title, string question)
    {
        IList<PollQuestion> retVal = new List<PollQuestion>();

        if (string.IsNullOrEmpty(title))
        {
            ModelState.AddModelError("title", "Please enter a title");
        }

        if (string.IsNullOrEmpty(question))
        {
            ModelState.AddModelError("question", "Please enter a question");
        }

        if (ModelState.IsValid == true)
        {
            using (this.Services.UnitOfWork.BeginTransaction())
            {
                try
                {
                    PollQuestion newPoll = Services.PollService.AddPollQuestion(question, title);
                    this.Services.UnitOfWork.EndTransaction(true);
                }
                catch (Exception e)
                {
                    LogManager.GetLogger().Error(e);
                    this.Services.UnitOfWork.EndTransaction(false);
                }
            }
        }

        return this.Json(this.Services.PollService.GetAll());
    }

    [AdminAuthorizationFilterAttribute(RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, false)]
    public IActionResult PutOption(int pollQuestionId, string optionText)
    {
        PollQuestion retVal = null;

        if (string.IsNullOrEmpty(optionText))
        {
            ModelState.AddModelError("optionText", "Please enter a text for the option");
        }

        if (ModelState.IsValid == true)
        {
            using (this.Services.UnitOfWork.BeginTransaction())
            {
                try
                {
                    retVal = Services.PollService.AddPollOption(pollQuestionId, optionText);
                    this.Services.UnitOfWork.EndTransaction(true);
                }
                catch (Exception e)
                {
                    LogManager.GetLogger().Error(e);
                    this.Services.UnitOfWork.EndTransaction(false);
                }
            }
        }

        return this.Json(retVal);
    }

    public IActionResult Delete(int id)
    {
        using (this.Services.UnitOfWork.BeginTransaction())
        {
            try
            {
                Services.PollService.Delete(Services.PollService.GetById(id));
                this.Services.UnitOfWork.EndTransaction(true);
            }
            catch (Exception e)
            {
                LogManager.GetLogger().Error(e);
                this.Services.UnitOfWork.EndTransaction(false);
            }
        }

        return this.RedirectToAction("Index");
    }

    public IActionResult DeleteOption(int id, int optionId)
    {
        PollQuestion retVal = null;

        using (this.Services.UnitOfWork.BeginTransaction())
        {
            try
            {
                retVal = this.Services.PollService.GetById(id);

                if (retVal != null)
                {
                    retVal.RemoveOption(optionId);
                }

                retVal = Services.PollService.Save(retVal);
                this.Services.UnitOfWork.EndTransaction(true);
            }
            catch (Exception e)
            {
                LogManager.GetLogger().Error(e);
                this.Services.UnitOfWork.EndTransaction(false);
            }
        }

        return this.Json(retVal);
    }
}
