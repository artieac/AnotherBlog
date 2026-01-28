using System;
using System.Collections.Generic;
using System.Linq;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;

namespace AlwaysMoveForward.AnotherBlog.DataLayer
{
    public class EntityFrameworkNinjectModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IAnotherBlogRepositoryManager>().To<RepositoryManager>();
        }
    }
}
