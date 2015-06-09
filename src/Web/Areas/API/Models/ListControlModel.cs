using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Web.Areas.API.Models
{
    public class ListControlModel
    {
        public ListControlModel()
        {
            this.OpenLinkInNewWindow = true;
        }

        public bool OpenLinkInNewWindow { get; set; }
        public bool ShowOrdered { get; set; }
        public string Title { get; set; }
        public IList<BlogListItem> ListItems { get; set; }
    }
}