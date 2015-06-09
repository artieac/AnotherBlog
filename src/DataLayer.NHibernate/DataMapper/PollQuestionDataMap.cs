using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DomainModel.Poll;
using AlwaysMoveForward.AnotherBlog.DataLayer.DTO;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.DataMapper
{
    public class PollQuestionDataMap : DataMapBase<PollQuestion, PollQuestionDTO>
    {
        private class VoterAddressDtoListResolver : MappedListResolver<VoterAddress, VoterAddressDTO> 
        {
            protected override IList<VoterAddressDTO> GetDestinationList(ResolutionResult source)
            {
                return ((PollOptionDTO)source.Context.DestinationValue).VoterAddresses;
            }

            protected override IList<VoterAddress> GetSourceList(ResolutionResult source)
            {
                IList<VoterAddress> retVal = null;

                if (source.Value != null)
                {
                    retVal = ((PollOption)source.Value).VoterAddresses;
                }

                return retVal;
            }

            protected override VoterAddressDTO FindItemInList(IList<VoterAddressDTO> destinationList, VoterAddress searchTarget)
            {
                return destinationList.FirstOrDefault(t => t.Id == searchTarget.Id);
            }

            protected override VoterAddress FindItemInList(IList<VoterAddress> sourceList, VoterAddressDTO searchTarget)
            {
                return sourceList.FirstOrDefault(t => t.Id == searchTarget.Id);
            }
        }

        private class PollOptionDtoListResolver : MappedListResolver<PollOption, PollOptionDTO> 
        {
            protected override IList<PollOptionDTO> GetDestinationList(ResolutionResult source)
            {
                return ((PollQuestionDTO)source.Context.DestinationValue).Options;
            }

            protected override IList<PollOption> GetSourceList(ResolutionResult source)
            {
                IList<PollOption> retVal = null;

                if (source.Value != null)
                {
                    retVal = ((PollQuestion)source.Value).Options;
                }

                return retVal;
            }

            protected override PollOptionDTO FindItemInList(IList<PollOptionDTO> destinationList, PollOption searchTarget)
            {
                return destinationList.FirstOrDefault(t => t.Id == searchTarget.Id);
            }

            protected override PollOption FindItemInList(IList<PollOption> sourceList, PollOptionDTO searchTarget)
            {
                return sourceList.FirstOrDefault(t => t.Id == searchTarget.Id);
            }
        }

        static PollQuestionDataMap()
        {
            if (AutoMapper.Mapper.FindTypeMapFor<PollQuestion, PollQuestionDTO>() == null)
            {
                AutoMapper.Mapper.CreateMap<VoterAddress, VoterAddressDTO>()
                    .ForMember(dest => dest.IPAddress, opt => opt.MapFrom( src => src.Address))
                    .ForMember(dest => dest.Option, opt => opt.Ignore());
                AutoMapper.Mapper.CreateMap<PollOption, PollOptionDTO>()
                    .ForMember(dest => dest.Question, opt => opt.Ignore())
                    .ForMember(dest => dest.VoterAddresses, pollOptions => pollOptions.ResolveUsing<VoterAddressDtoListResolver>());
                AutoMapper.Mapper.CreateMap<PollQuestion, PollQuestionDTO>()
                    .ForMember(dest => dest.Options, pollOptions => pollOptions.ResolveUsing<PollOptionDtoListResolver>());
            }

            if (AutoMapper.Mapper.FindTypeMapFor<PollQuestionDTO, PollQuestion>() == null)
            {
                AutoMapper.Mapper.CreateMap<VoterAddressDTO, VoterAddress>()
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.IPAddress));
                AutoMapper.Mapper.CreateMap<PollOptionDTO, PollOption>();
                AutoMapper.Mapper.CreateMap<PollQuestionDTO, PollQuestion>();
            }

#if DEBUG
            AutoMapper.Mapper.AssertConfigurationIsValid();
#endif
        }

        public override PollQuestion Map(PollQuestionDTO source, PollQuestion destination)
        {
            return AutoMapper.Mapper.Map(source, destination);
        }

        public override PollQuestionDTO Map(PollQuestion source, PollQuestionDTO destination)
        {
            PollQuestionDTO retVal = AutoMapper.Mapper.Map(source, destination);

            foreach (PollOptionDTO currentOption in retVal.Options)
            {
                currentOption.Question = retVal;

                foreach (VoterAddressDTO vote in currentOption.VoterAddresses)
                {
                    vote.Option = currentOption;
                }
            }

            return retVal;
        }
    }
}
