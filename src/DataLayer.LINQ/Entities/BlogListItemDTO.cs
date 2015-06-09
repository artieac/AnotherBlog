using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.DataLayer.Map;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Entities
{
    public partial class BlogListItemDTO : IBlogListItem
    {
        public BlogList BlogList
        {
            get { return BlogListMapper.GetInstance().Map(this.BlogListDTO); }
            set { this.BlogListDTO = BlogListMapper.GetInstance().Map(value); }
        }
    }
}
