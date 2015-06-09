using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.IO;

using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.Common.DomainModel.Poll;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.Web.Models.BlogModels;
using AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Models;
using AlwaysMoveForward.AnotherBlog.Web.Code.Utilities;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;

namespace AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Controllers
{
    [RequestAuthenticationFilter]
    public class ManagePollsController : AdminBaseController
    {
        private const int PollPageSize = 25;

        [AdminAuthorizationFilter(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = false)]
        public ActionResult Index(int? page)
        {
            int currentPageIndex = 0;

            if (page.HasValue == true)
            {
                currentPageIndex = page.Value - 1;
            }

            ManagePollsModel model = new ManagePollsModel();
            model.Common = this.InitializeCommonModel();
            model.Polls = Pagination.ToPagedList(this.Services.PollService.GetAll(), currentPageIndex, PollPageSize);
            return this.View(model);
        }

        [AdminAuthorizationFilter(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = false)]
        public JsonResult GetAll(int? page)
        {
            int currentPageIndex = 0;

            if (page.HasValue == true)
            {
                currentPageIndex = page.Value - 1;
            }

            IPagedList<PollQuestion> retVal = Pagination.ToPagedList(this.Services.PollService.GetAll(), currentPageIndex, PollPageSize);
            return this.Json(retVal, JsonRequestBehavior.AllowGet);
         }

        [AdminAuthorizationFilter(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = false)]
        public JsonResult GetById(int pollQuestionId)
        {
            PollQuestion retVal = this.Services.PollService.GetById(pollQuestionId);
            return this.Json(retVal, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorizationFilter(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = false)]
        public JsonResult Add(string title, string question)
        {
            IList<PollQuestion> retVal = new List<PollQuestion>();

            if (string.IsNullOrEmpty(title))
            {
                ViewData.ModelState.AddModelError("title", "Please enter a title");
            }

            if (string.IsNullOrEmpty(question))
            {
                ViewData.ModelState.AddModelError("question", "Please enter a question");
            }

            if (ViewData.ModelState.IsValid == true)
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

            return this.Json(this.Services.PollService.GetAll(), JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorizationFilter(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = false)]
        public JsonResult PutOption(int pollQuestionId, string optionText)
        {
            PollQuestion retVal = null;

            if (string.IsNullOrEmpty(optionText))
            {
                ViewData.ModelState.AddModelError("optionText", "Please enter a text for the option");
            }

            if (ViewData.ModelState.IsValid == true)
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

        public ActionResult Delete(int id)
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

        public JsonResult DeleteOption(int id, int optionId)
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
}