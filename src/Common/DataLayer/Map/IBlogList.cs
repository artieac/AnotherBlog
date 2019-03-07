using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PucksAndProgramming.AnotherBlog.Common.DomainModel;

namespace PucksAndProgramming.AnotherBlog.Common.DataLayer.Map
{
    public interface IBlogList
    {
        int Id { get; set; }
        Blog Blog { get; set; }
        string Name { get; set; }
        bool ShowOrdered { get; set; }
    }
}
