using System;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.MappingDomainObjects
{
    public class Role
    {
        public Role()
        {
            this.Id = 0;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
