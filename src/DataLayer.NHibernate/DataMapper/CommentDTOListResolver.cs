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
    public class CommentDTOListResolver : MappedListResolver<Comment, EntryCommentsDTO>
    {
        protected override IList<EntryCommentsDTO> GetDestinationList(ResolutionResult source)
        {
            return ((BlogPostDTO)source.Context.DestinationValue).Comments;
        }

        protected override IList<Comment> GetSourceList(ResolutionResult source)
        {
            IList<Comment> retVal = null;

            if (source.Value != null)
            {
                retVal = ((BlogPost)source.Value).Comments;
            }

            return retVal;
        }

        protected override EntryCommentsDTO FindItemInList(IList<EntryCommentsDTO> destinationList, Comment searchTarget)
        {
            return destinationList.FirstOrDefault(t => t.Id == searchTarget.Id);
        }

        protected override Comment FindItemInList(IList<Comment> sourceList, EntryCommentsDTO searchTarget)
        {
            return sourceList.FirstOrDefault(t => t.Id == searchTarget.Id);
        }
    }
}
