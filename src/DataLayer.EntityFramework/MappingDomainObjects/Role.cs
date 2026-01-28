using System;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.MappingDomainObjects
{
    public class Role
    {
        public Role()
        {
            this.Id = -1;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
