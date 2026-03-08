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
    public class PollOptionDtoListResolver : MappedListResolver<PollOption, PollOptionDTO>
    {
        protected override PollOptionDTO FindItemInList(IList<PollOptionDTO> destinationList, PollOption searchTarget)
        {
            return destinationList.FirstOrDefault(t => t.Id == searchTarget.Id);
        }

        protected override PollOption FindItemInList(IList<PollOption> sourceList, PollOptionDTO searchTarget)
        {
            return sourceList.FirstOrDefault(t => t.Id == searchTarget.Id);
        }
    }
}
