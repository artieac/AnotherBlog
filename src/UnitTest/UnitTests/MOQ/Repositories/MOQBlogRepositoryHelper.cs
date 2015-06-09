using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.MOQ.Repositories
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
