using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Web.Models.BlogModels;

namespace AlwaysMoveForward.AnotherBlog.Web.Models.Home
{
    public class IndexModel : ModelBase
    {
        public string ContentMessage { get; set; }

        public IList<BlogPostModel> BlogEntries { get; set; }
    }
}