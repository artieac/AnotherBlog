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
    public class MOQTagRepositoryHelper
    {
        public static IList<Tag> defaultList;

        static MOQTagRepositoryHelper()
        {
            defaultList = new List<Tag>();
        }

        public static void ConfigureGetAll(Mock<ITagRepository> moqObject)
        {
            moqObject.Setup(x => x.GetAll(It.IsAny<int>()))
            .Returns(MOQTagRepositoryHelper.defaultList);
        }

        public static void ConfigureGetByName(Mock<ITagRepository> moqObject)
        {
            moqObject.Setup(x => x.GetByName(It.IsAny<string>(), It.IsAny<int>())).Returns(
                ((string s, int i) => MOQTagRepositoryHelper.defaultList.FirstOrDefault(t => t.Name == s)));
        }

        public static void ConfigureSave(Mock<ITagRepository> moqObject)
        {
            moqObject.Setup(x => x.Save(It.IsAny<Tag>()))
                .Returns((Tag t) => new Mock<Tag>().SetupProperty(f => f.Name, t.Name).Object)
                .Callback<Tag>(f => MOQTagRepositoryHelper.defaultList.Add(f));
        }
    }
}
