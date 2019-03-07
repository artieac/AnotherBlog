using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PucksAndProgramming.AnotherBlog.Common.DomainModel;

namespace PucksAndProgramming.AnotherBlog.Common.DataLayer.Map
{
    public interface IPostTag
    {
        int PostTagId { get; set; }
        int TagId { get; set; }
        int PostId { get; set; }
    }
}
