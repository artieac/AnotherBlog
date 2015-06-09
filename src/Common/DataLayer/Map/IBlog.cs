using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map
{
    public interface IBlog
    {
        int BlogId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string SubFolder { get; set; }
        string About { get; set; }
        string WelcomeMessage { get; set; }
        string ContactEmail { get; set; }
        string Theme { get; set; }
    }
}
