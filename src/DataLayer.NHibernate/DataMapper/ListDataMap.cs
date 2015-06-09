using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.DataLayer.DTO;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.DataMapper
{
    public class ListDataMap : DataMapBase<BlogList, BlogListDTO>
    {
        private class BlogListItemDTOResolver : MappedListResolver<BlogListItem, BlogListItemDTO> 
        {
            protected override IList<BlogListItemDTO> GetDestinationList(ResolutionResult source)
            {
                return ((BlogListDTO)source.Context.DestinationValue).Items;
            }

            protected override IList<BlogListItem> GetSourceList(ResolutionResult source)
            {
                IList<BlogListItem> retVal = null;

                if(source.Value != null)
                {
                    retVal = ((BlogList)source.Value).Items;
                }

                return retVal;
            }

            protected override BlogListItemDTO FindItemInList(IList<BlogListItemDTO> destinationList, BlogListItem searchTarget)
            {
                return destinationList.FirstOrDefault(t => t.Id == searchTarget.Id);
            }

            protected override BlogListItem FindItemInList(IList<BlogListItem> sourceList, BlogListItemDTO searchTarget)
            {
                return sourceList.FirstOrDefault(t => t.Id == searchTarget.Id);
            }
        }

        static ListDataMap()
        {

            if (AutoMapper.Mapper.FindTypeMapFor<BlogList, DbInfoDTO>() == null)
            {
                AutoMapper.Mapper.CreateMap<BlogList, BlogListDTO>()
                    .ForMember(bl => bl.Items, blogListItems => blogListItems.ResolveUsing<BlogListItemDTOResolver>());
                AutoMapper.Mapper.CreateMap<BlogListItem, BlogListItemDTO>()
                    .ForMember(va => va.BlogList, opt => opt.Ignore());
            }

            if (AutoMapper.Mapper.FindTypeMapFor<BlogListDTO, BlogList>() == null)
            {
                AutoMapper.Mapper.CreateMap<BlogListDTO, BlogList>();
                AutoMapper.Mapper.CreateMap<BlogListItemDTO, BlogListItem>();
            }
#if DEBUG
            AutoMapper.Mapper.AssertConfigurationIsValid();
#endif
        }

        public override BlogListDTO Map(BlogList source, BlogListDTO destination)
        {
            BlogListDTO retVal = AutoMapper.Mapper.Map(source, destination);

            foreach (BlogListItemDTO currentListItem in retVal.Items)
            {
                currentListItem.BlogList = retVal;
            }

            return retVal;
        }

        public override BlogList Map(BlogListDTO source, BlogList destination)
        {
            return AutoMapper.Mapper.Map(source, destination);
        }
    }
}
