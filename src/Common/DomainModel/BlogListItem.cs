using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;

namespace AlwaysMoveForward.AnotherBlog.Common.DomainModel
{
    public class BlogListItem 
    {
        public BlogListItem()
        {
            this.Id = -1;
        }

        public long Id { get; set; }
        public long BlogListId { get; set; }
        public BlogList BlogList { get; set; }
        public string Name { get; set; }
        public string RelatedLink { get; set; }
        public int DisplayOrder { get; set; }
    }
}
