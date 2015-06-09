using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnotherBlog.IntegrationService.Models
{
    public class BlogElement
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SubFolder { get; set; }
        public string About { get; set; }
        public string WelcomeMessage { get; set; }
        public string ContactEmail { get; set; }
        public string Theme { get; set; }
    }
}
