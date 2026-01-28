using System;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.MappingDomainObjects
{
    public class ExtensionConfiguration
    {
        public ExtensionConfiguration()
        {
            this.Id = -1;
        }

        public int Id { get; set; }
        public int ExtensionId { get; set; }
        public int BlogId { get; set; }
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
    }
}
