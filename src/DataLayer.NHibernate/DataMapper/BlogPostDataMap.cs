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
        static BlogPostDataMap()
        {
            BlogPostDataMap.ConfigureAutoMapper();
        }

        internal static void ConfigureAutoMapper()
        {
            BlogDataMap.ConfigureAutoMapper();
            UserDataMap.ConfigureAutoMapper();
            TagDataMap.ConfigureAutoMapper();

            if (AutoMapper.Mapper.FindTypeMapFor<Comment, EntryCommentsDTO>() == null)
            {
                AutoMapper.Mapper.CreateMap<Comment, EntryCommentsDTO>()
                    .ForMember(dest => dest.BlogPost, opt => opt.Ignore())
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
                    .ForMember(dest => dest.Link, opt => opt.MapFrom(src => (int)src.Status));
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
