using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;

namespace PucksAndProgramming.AnotherBlog.Common.Factories
{
    public class BlogFactory
    {
        public static Blog CreateBlog()
        {
            Blog retVal = new Blog();
            return retVal;
        }
    }
}
