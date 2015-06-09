using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlwaysMoveForward.AnotherBlog.Web.Code.Filters
{
    public class AdminFilterProperty
    {
        public AdminFilterProperty(string requiredRole, bool blogSpecific)
        {
            this.RequiredRole = requiredRole;
            this.BlogSpecific = blogSpecific;
        }

        public string RequiredRole { get; set; }

        public bool BlogSpecific { get; set; }
    }
}