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
    public class BlogPostDataMap : DataMapBase<BlogPost, BlogPostDTO>
    {
        private class CommentDTOListResolver : MappedListResolver<Comment, EntryCommentsDTO>
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

        static BlogPostDataMap()
        {
            BlogPostDataMap.ConfigureAutoMapper();
        }

        internal static void ConfigureAutoMapper()
        {
            UserDataMap.ConfigureAutoMapper();
            TagDataMap.ConfigureAutoMapper();

            if (AutoMapper.Mapper.FindTypeMapFor<Comment, EntryCommentsDTO>() == null)
            {
                AutoMapper.Mapper.CreateMap<Comment, EntryCommentsDTO>()
                    .ForMember(dest => dest.BlogPost, opt => opt.Ignore());
            }

            if (AutoMapper.Mapper.FindTypeMapFor<EntryCommentsDTO, Comment>() == null)
            {
                AutoMapper.Mapper.CreateMap<EntryCommentsDTO, Comment>();
            }

            if (AutoMapper.Mapper.FindTypeMapFor<BlogPost, BlogPostDTO>() == null)
            {
                AutoMapper.Mapper.CreateMap<BlogPost, BlogPostDTO>()
                    .ForMember(dest => dest.Tags, src => src.ResolveUsing<TagDTOListResolver>())
                    .ForMember(dest => dest.Comments, src => src.ResolveUsing<CommentDTOListResolver>());
            }

            if (AutoMapper.Mapper.FindTypeMapFor<BlogPostDTO, BlogPost>() == null)
            {
                AutoMapper.Mapper.CreateMap<BlogPostDTO, BlogPost>()
                   .ForMember(bp => bp.CommentCount, opt => opt.Ignore());
            }

#if DEBUG
            Mapper.AssertConfigurationIsValid();
#endif
        }

        public override BlogPost Map(BlogPostDTO source, BlogPost destination)
        {
            if (destination == null)
            {
                destination = new BlogPost();
            }

            return AutoMapper.Mapper.Map(source, destination);
        }

        public override BlogPostDTO Map(BlogPost source, BlogPostDTO destination)
        {
            if (destination == null)
            {
                destination = new BlogPostDTO();
            }

            destination = AutoMapper.Mapper.Map(source, destination);

            foreach (EntryCommentsDTO currentComment in destination.Comments)
            {
                currentComment.BlogPost = destination;
            }

            return destination;
        }
    }
}
