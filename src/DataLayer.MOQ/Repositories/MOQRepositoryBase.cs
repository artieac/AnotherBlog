using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Moq;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.Repositories;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class MOQRepositoryBase<DomainType, DTOType> : RepositoryBase<DomainType, DTOType>,
                                                          IRepository<DomainType>
        where DomainType : class, new() where DTOType : class, DomainType, new()
    {
        public MOQRepositoryBase(IUnitOfWork _unitOfWork, IRepositoryManager repositoryManager)
            : base(_unitOfWork, repositoryManager) {}

        public override DomainType Create()
        {
            Mock<DomainType> retVal = new Mock<DomainType>();
            return retVal.Object;
        }

        public override DomainType GetByProperty(string idPropertyName, object idValue)
        {
            Mock<DomainType> retVal = new Mock<DomainType>();
            return retVal.Object;
        }

        public override DomainType GetByProperty(string idPropertyName, object idValue, int blogId)
        {
            Mock<DomainType> retVal = new Mock<DomainType>();
            return retVal.Object;
        }

        public override IList<DomainType> GetAll()
        {
            Mock<IList<DomainType>> retVal = new Mock<IList<DomainType>>();
            return retVal.Object;
        }

        public override IList<DomainType> GetAll(int blogId)
        {
            Mock<IList<DomainType>> retVal = new Mock<IList<DomainType>>();
            return retVal.Object;
        }

        public override IList<DomainType> GetAllByProperty(string idPropertyName, object idValue)
        {
            Mock<IList<DomainType>> retVal = new Mock<IList<DomainType>>();
            return retVal.Object;
        }

        public override IList<DomainType> GetAllByProperty(string idPropertyName, object idValue, int blogId)
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
