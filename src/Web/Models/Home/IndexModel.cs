using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PucksAndProgramming.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Web.Models.BlogModels;

namespace PucksAndProgramming.AnotherBlog.Web.Models.Home
{
    public class IndexModel : ModelBase
    {
        public string ContentMessage { get; set; }

        public IList<BlogPostModel> BlogEntries { get; set; }
    }
}