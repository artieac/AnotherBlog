using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlwaysMoveForward.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.DataLayer.Map;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Entities
{
    public partial class BlogEntryDTO : IBlogPost
    {
        public int AuthorId
        {
            get { return this.UserId; }
            set { this.UserId = value; }
        }

        public Blog Blog
        {
            get { return BlogMapper.GetInstance().Map(this.BlogDTO); }
            set { this.BlogDTO = BlogMapper.GetInstance().Map(value); }
        }

        public User Author
        {
            get { return UserMapper.GetInstance().Map(this.UserDTO); }
            set { this.UserDTO = UserMapper.GetInstance().Map(value); }
        }

        public int GetCommentCount()
        {
            int retVal = 0;

            if (this.EntryCommentDTOs != null)
            {
                retVal = this.EntryCommentDTOs.Count();
            }

            return retVal;
        }
    }
}
