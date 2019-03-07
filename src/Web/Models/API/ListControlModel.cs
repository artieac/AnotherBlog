using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;

namespace PucksAndProgramming.AnotherBlog.Web.Models.API
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