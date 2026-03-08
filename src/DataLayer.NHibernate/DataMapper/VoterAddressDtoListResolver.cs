using AlwaysMoveForward.AnotherBlog.DataLayer.DTO;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DomainModel.Poll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.DataMapper
{
    public class VoterAddressDtoListResolver : MappedListResolver<VoterAddress, VoterAddressDTO>
    {
        protected override VoterAddressDTO FindItemInList(IList<VoterAddressDTO> destinationList, VoterAddress searchTarget)
        {
            return destinationList.FirstOrDefault(t => t.Id == searchTarget.Id);
        }

        protected override VoterAddress FindItemInList(IList<VoterAddress> sourceList, VoterAddressDTO searchTarget)
        {
            return sourceList.FirstOrDefault(t => t.Id == searchTarget.Id);
        }
    }
}
