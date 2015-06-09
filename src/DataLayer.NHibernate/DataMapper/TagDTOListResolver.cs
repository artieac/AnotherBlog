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
    public class TagDTOListResolver : MappedListResolver<Tag, TagDTO>
    {
        protected override IList<TagDTO> GetDestinationList(ResolutionResult source)
        {
            return ((BlogPostDTO)source.Context.DestinationValue).Tags;
        }

        protected override IList<Tag> GetSourceList(ResolutionResult source)
        {
            IList<Tag> retVal = null;

            if (source.Value != null)
            {
                retVal = ((BlogPost)source.Value).Tags;
            }

            return retVal;
        }

        protected override TagDTO FindItemInList(IList<TagDTO> destinationList, Tag searchTarget)
        {
            return destinationList.FirstOrDefault(t => t.Id == searchTarget.Id && t.Name == searchTarget.Name);
        }

        protected override Tag FindItemInList(IList<Tag> sourceList, TagDTO searchTarget)
        {
            return sourceList.FirstOrDefault(t => t.Id == searchTarget.Id);
        }
    }
}
