using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Web.Models;

namespace AlwaysMoveForward.AnotherBlog.Web.Models.Home
{
    public class SiteModel : ModelBase
    {
        public IList<string> FoundExtensions { get; set; }
        public SiteInfo SiteInfo { get; set; }
    }
}