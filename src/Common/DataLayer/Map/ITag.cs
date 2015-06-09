using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map
{
    public interface ITag
    {
        int Id { get; set; }
        string Name { get; set; }
        int BlogId { get; set; }
    }
}
