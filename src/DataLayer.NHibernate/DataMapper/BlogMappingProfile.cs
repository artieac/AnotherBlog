using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.DataLayer.DataMapper;
using AlwaysMoveForward.AnotherBlog.DataLayer.DTO;
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.Common.DomainModel.Poll;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.DataMapper
{
    internal class BlogMappingProfile
    {
    }

    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<DbInfo, DbInfoDTO>();
            CreateMap<DbInfoDTO, DbInfo>();

            CreateMap<SiteInfo, SiteInfoDTO>();
            CreateMap<SiteInfoDTO, SiteInfo>();

            CreateMap<Blog, BlogDTO>()
                .ForMember(dest => dest.Posts, opt => opt.Ignore());

            CreateMap<AnotherBlogUser, UserDTO>()
//                .ForMember(dest => dest.Roles, opt => opt.ResolveUsing<BlogUserDTOListResolver>())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<UserDTO, AnotherBlogUser>()
//                .ForMember(dest => dest.Roles, opt => opt.ResolveUsing<BlogUserListResolver>())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<Tag, TagDTO>()
                .ForMember(dest => dest.BlogEntries, opt => opt.Ignore());
            CreateMap<TagDTO, Tag>();

            CreateMap<Comment, EntryCommentsDTO>()
                    .ForMember(dest => dest.BlogPost, opt => opt.Ignore())
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
                    .ForMember(dest => dest.Link, opt => opt.MapFrom(src => (int)src.Status));

            CreateMap<EntryCommentsDTO, Comment>();
            CreateMap<BlogPost, BlogPostDTO>();
//                    .ForMember(dest => dest.Tags, src => src.MapFrom<TagDTOListResolver>())
  //                  .ForMember(dest => dest.Comments, src => src.MapFrom<CommentDT;OListResolver>());

            CreateMap<BlogPostDTO, BlogPost>()
                   .ForMember(bp => bp.CommentCount, opt => opt.Ignore());


            CreateMap<BlogList, BlogListDTO>();
//                    .ForMember(bl => bl.Items, blogListItems => blogListItems.ResolveUsing<BlogListItemDTOResolver>());
            CreateMap<BlogListItem, BlogListItemDTO>()
                    .ForMember(va => va.BlogList, opt => opt.Ignore());
            
            CreateMap<BlogListDTO, BlogList>();
            CreateMap<BlogListItemDTO, BlogListItem>();

            CreateMap<VoterAddress, VoterAddressDTO>()
                    .ForMember(dest => dest.IPAddress, opt => opt.MapFrom(src => src.Address))
                    .ForMember(dest => dest.Option, opt => opt.Ignore());
            CreateMap<PollOption, PollOptionDTO>()
                .ForMember(dest => dest.Question, opt => opt.Ignore());
            //                .ForMember(dest => dest.VoterAddresses, pollOptions => pollOptions.ResolveUsing<VoterAddressDtoListResolver>());
            CreateMap<PollQuestion, PollQuestionDTO>();
//                .ForMember(dest => dest.Options, pollOptions => pollOptions.ResolveUsing<PollOptionDtoListResolver>());

                CreateMap<VoterAddressDTO, VoterAddress>()
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.IPAddress));
                CreateMap<PollOptionDTO, PollOption>();
                CreateMap<PollQuestionDTO, PollQuestion>();
        }
    }
}