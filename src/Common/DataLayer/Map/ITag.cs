using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PucksAndProgramming.AnotherBlog.Common.DomainModel;

namespace PucksAndProgramming.AnotherBlog.Common.DataLayer.Map
{
    public interface ITag
    {
        int Id { get; set; }
        string Name { get; set; }
        int BlogId { get; set; }
    }
}
