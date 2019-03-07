using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using PucksAndProgramming.Common.DataLayer;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Common.DataLayer.Repositories;

namespace PucksAndProgramming.AnotherBlog.UnitTest.MOQ.Repositories
{
    public class MOQBlogRepositoryHelper
    {
        public static void ConfigureGetBySubFolder(Mock<IBlogRepository> moqObject)
        {
            moqObject.Setup(x=> x.GetBySubFolder(It.IsAny<string>()))
                .Returns((string s) => new Mock<Blog>()
                    .SetupProperty(f => f.Name, s)
                    .SetupProperty(f => f.SubFolder, s)
                    .Object);
            ;
        }
    }
}
