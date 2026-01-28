using System;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.MappingDomainObjects
{
    public class BlogExtension
    {
        public BlogExtension()
        {
            this.Id = -1;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string AssemblyName { get; set; }
        public string ClassName { get; set; }
    }
}
