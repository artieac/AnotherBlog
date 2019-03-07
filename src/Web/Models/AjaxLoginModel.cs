using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PucksAndProgramming.AnotherBlog.Web.Models
{
    public class AjaxLoginModel
    {
        public bool ProcessedLogin { get; set; }
        public bool IsAuthorized { get; set; }
    }
}