using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.DataLayer.DTO;
using AlwaysMoveForward.Common.DataLayer;
using AutoMapper;
using AutoMapper.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.DataMapper
{
    class BlogUserListResolver : MappedListResolverBase<BlogUserDTO, IDictionary<long, RoleType.Id>, IDictionary<long, RoleType.Id>>
    {
        protected override BlogUserDTO FindItemInList(IList<BlogUserDTO> destinationList, IDictionary<long, RoleType.Id> searchTarget)
        {
            return destinationList.FirstOrDefault(t => t.Id == searchTarget.Id);
        }

        protected override IDictionary<long, RoleType.Id> FindItemInList(IDictionary<long, RoleType.Id> sourceList, BlogUserDTO searchTarget)
        {
            if (sourceList!=null && sourceList.ContainsKey(searchTarget.Id))
            {
                IDictionary<long, RoleType.Id> retVal = new Dictionary<long, RoleType.Id>();
                retVal.Add(searchTarget.Id, sourceList[searchTarget.Id]);
                return retVal;
            }

            return new Dictionary<long, RoleType.Id>();
        }
    }
}
