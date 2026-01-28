using System;
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.MappingDomainObjects
{
    public class BlogUser
    {
        public BlogUser()
        {
            this.Id = -1;
        }

        public int Id { get; set; }
        public Blog Blog { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
