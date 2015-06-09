using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.Common.DomainModel.Poll;

namespace AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Models
{
    public class ManagePollsModel
    {
        public ManagePollsModel()
        {
            this.Common = new AdminCommon();
        }

        public AdminCommon Common { get; set; }
        public IPagedList<PollQuestion> Polls { get; set; }
        public PollQuestion CurrentPoll { get; set; }
    }
}