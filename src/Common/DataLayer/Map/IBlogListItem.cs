using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map
{
    public interface IBlogListItem
    {
        int Id { get; set; }
        BlogList BlogList { get; set; }
        string Name { get; set; }
        string RelatedLink { get; set; }
        int DisplayOrder { get; set; }
    }
}
