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
    public partial class BlogUserDTO : IBlogUser
    {
        public User User
        {
            get { return UserMapper.GetInstance().Map(this.UserDTO); }
            set { this.UserDTO = UserMapper.GetInstance().Map(value); }
        }

        public Blog Blog
        {
            get { return BlogMapper.GetInstance().Map(this.BlogDTO); }
            set { this.BlogDTO = BlogMapper.GetInstance().Map(value); }
        }

        public Role UserRole
        {
            get { return RoleMapper.GetInstance().Map(this.RoleDTO); }
            set { this.RoleDTO = RoleMapper.GetInstance().Map(value); }
        }
    }
}
