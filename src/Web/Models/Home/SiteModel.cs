using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PucksAndProgramming.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Web.Models;

namespace PucksAndProgramming.AnotherBlog.Web.Models.Home
{
    public class SiteModel : ModelBase
    {
        public IList<string> FoundExtensions { get; set; }
        public SiteInfo SiteInfo { get; set; }
    }
}