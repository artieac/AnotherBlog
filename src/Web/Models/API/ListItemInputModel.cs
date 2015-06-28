using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlwaysMoveForward.AnotherBlog.Web.Models.API
{
    public class ListItemInputModel
    {
        public string Name { get; set; }
        public string RelatedLink { get; set; }
        public int DisplayOrder { get; set; }
    }
}