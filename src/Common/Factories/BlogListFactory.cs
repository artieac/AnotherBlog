using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Common.Factories
{
    public class BlogListFactory
    {
        public static BlogList Create(Blog ownerBlog)
        {
            BlogList retVal = new BlogList();
            retVal.BlogId = ownerBlog.Id;
            retVal.InitializeItems();
            return retVal;
        }
    }
}
