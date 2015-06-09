using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.DataLayer.Map;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Entities
{
    public partial class BlogEntryTagDTO : IPostTag
    {
        public int PostTagId
        {
            get { return this.BlogEntryTagId; }
            set { this.BlogEntryTagId = value; }
        }

        public BlogPost Post
        {
            get { return BlogPostMapper.GetInstance().Map(this.BlogEntryDTO); }
            set { this.BlogEntryDTO = BlogPostMapper.GetInstance().Map(value); }
        }

        public Tag Tag
        {
            get { return TagMapper.GetInstance().Map(this.TagDTO); }
            set { this.TagDTO = TagMapper.GetInstance().Map(value); }
        }
    }
}
