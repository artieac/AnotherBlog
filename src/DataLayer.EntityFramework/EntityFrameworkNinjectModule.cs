using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.Repositories;

namespace AlwaysMoveForward.AnotherBlog.DataLayer
{
    class EntityFrameworkNinjectModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IRepositoryManager>().To<RepositoryManager>();
        }
    }
}
