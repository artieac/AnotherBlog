using System;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.MappingDomainObjects
{
    public class PostTag
    {
        public PostTag()
        {
            this.Id = -1;
        }

        public int Id { get; set; }
        public Tag Tag { get; set; }
        public BlogPost Post { get; set; }
    }
}
