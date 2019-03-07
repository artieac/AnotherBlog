using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PucksAndProgramming.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;

namespace PucksAndProgramming.AnotherBlog.Web.Areas.Admin.Models
{
    public class SiteModel
    {
        public AdminCommon Common { get; set; }
        public SiteInfo SiteInfo { get; set; }
        public IList<Blog> Blogs { get; set; }
    }
}
