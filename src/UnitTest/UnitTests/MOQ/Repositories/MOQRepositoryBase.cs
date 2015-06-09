using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.ActiveRecord;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.MOQ.Repositories
{
    public abstract class MOQRepositoryBase<DomainType, DTOType, TIDType> : ActiveRecordRepositoryBase<DomainType, DTOType, TIDType>,
                                                          IRepository<DomainType, TIDType>
        where DomainType : class, new() where DTOType : class, DomainType, new()
    {
        public MOQRepositoryBase(UnitOfWork _unitOfWork)
            : base(_unitOfWork) {}

        public override DomainType GetByProperty(string idPropertyName, object idValue)
        {
            Mock<DomainType> retVal = new Mock<DomainType>();
            return retVal.Object;
        }

        public override IList<DomainType> GetAll()
        {
            Mock<IList<DomainType>> retVal = new Mock<IList<DomainType>>();
            return retVal.Object;
        }

        public override IList<DomainType> GetAllByProperty(string idPropertyName, object idValue)
        {
            Mock<IList<DomainType>> retVal = new Mock<IList<DomainType>>();
            return retVal.Object;
        }

        public override DomainType Save(DomainType itemTosave)
        {
            return itemTosave;
        }

        public DTOType Save(DTOType itemToSave)
        {
            return itemToSave;
        }

        public bool Delete(DTOType itemToDelete)
        {
            return true;
        }

        /// <summary>
        /// Remove the blog entry
        /// </summary>
        /// <param name="saveItem"></param>
        public override bool Delete(DomainType itemToDelete)
        {
            throw new NotImplementedException();
        }
    }
}
