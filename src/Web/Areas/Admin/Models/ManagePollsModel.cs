using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PucksAndProgramming.Common.Utilities;
using PucksAndProgramming.Common.DomainModel.Poll;

namespace PucksAndProgramming.AnotherBlog.Web.Areas.Admin.Models
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