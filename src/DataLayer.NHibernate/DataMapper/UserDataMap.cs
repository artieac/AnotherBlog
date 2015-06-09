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
    public class UserDataMap : DataMapBase<AnotherBlogUser, UserDTO>
    {
        private class BlogUserListResolver : IValueResolver
        {
            public ResolutionResult Resolve(ResolutionResult source)
            {
                IDictionary<int, RoleType.Id> destinationList = ((AnotherBlogUser)source.Context.DestinationValue).Roles;

                if (destinationList == null)
                {
                    destinationList = new Dictionary<int, RoleType.Id>();
                }

                if (source.Value != null)
                {
                    IList<BlogUserDTO> sourceList = ((UserDTO)source.Value).Roles;

                    if (sourceList != null)
                    {
                        // go through and remove any items that were removed in the domain and need to be removed in the dto
                        foreach (int blogId in destinationList.Keys)
                        {
                            BlogUserDTO destinationItem = sourceList.FirstOrDefault(t => t.BlogId == blogId);

                            if (destinationItem == null)
                            {
                                destinationList.Remove(blogId);
                            }
                        }

                        // add in all of the new items, or update items already in the list
                        for(int i = 0; i < sourceList.Count; i++)
                        {
                            if(destinationList.ContainsKey(sourceList[i].BlogId))
                            {
                                destinationList[sourceList[i].BlogId] = (RoleType.Id)sourceList[i].RoleId;
                            }
                            else
                            {
                                destinationList.Add(sourceList[i].BlogId, (RoleType.Id)sourceList[i].RoleId);
                            }
                        }
                    }
                }

                return source.New(destinationList, typeof(IDictionary<int, RoleType.Id>));
            }
        }
        
        private class BlogUserDTOListResolver : IValueResolver
        {
            public ResolutionResult Resolve(ResolutionResult source)
            {
                IList<BlogUserDTO> destinationList = ((UserDTO)source.Context.DestinationValue).Roles;

                if (destinationList == null)
                {
                    destinationList = new List<BlogUserDTO>();
                }

                if (source.Value != null)
                {
                    IDictionary<int, RoleType.Id> sourceList = ((AnotherBlogUser)source.Value).Roles;

                    if (sourceList != null)
                    {
                        // go through and remove any items that were removed in the domain and need to be removed in the dto
                        for (int i = destinationList.Count - 1; i > -1; i--)
                        {
                            if (!sourceList.ContainsKey(destinationList[i].BlogId))
                            {
                                BlogUserDTO destinationItem = destinationList.FirstOrDefault(t => t.BlogId == destinationList[i].BlogId);

                                if (destinationItem == null)
                                {
                                    destinationList.RemoveAt(i);
                                }
                            }
                        }

                        // add in all of the new items, or update items already in the list
                        foreach(int blogId in sourceList.Keys)
                        {
                            BlogUserDTO blogUserDTO = destinationList.FirstOrDefault(t => t.BlogId == blogId);

                            if (blogUserDTO == null)
                            {
                                blogUserDTO = new BlogUserDTO();
                                blogUserDTO.RoleId = (int)sourceList[blogId];
                                blogUserDTO.User = (UserDTO)source.Context.DestinationValue;
                                blogUserDTO.BlogId = blogId;
                                destinationList.Add(blogUserDTO);
                            }
                            else
                            {
                                blogUserDTO.RoleId = (int)sourceList[blogId];
                            }
                        }
                    }
                }

                return source.New(destinationList, typeof(IList<BlogUserDTO>));
            }
        }

        static UserDataMap()
        {
            UserDataMap.ConfigureAutoMapper();
        }

        public static void ConfigureAutoMapper()
        {
            if (AutoMapper.Mapper.FindTypeMapFor<AnotherBlogUser, UserDTO>() == null)
            {
                AutoMapper.Mapper.CreateMap<AnotherBlogUser, UserDTO>()
                    .ForMember(dest => dest.Roles, opt => opt.ResolveUsing<BlogUserDTOListResolver>())
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            }

            if (AutoMapper.Mapper.FindTypeMapFor<UserDTO, AnotherBlogUser>() == null)
            {
                AutoMapper.Mapper.CreateMap<UserDTO, AnotherBlogUser>()
                    .ForMember(dest => dest.Roles, opt => opt.ResolveUsing<BlogUserListResolver>())
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            }

#if DEBUG
            AutoMapper.Mapper.AssertConfigurationIsValid();
#endif
        }

        public override AnotherBlogUser Map(UserDTO source, AnotherBlogUser destination)
        {
            return AutoMapper.Mapper.Map(source, destination);
        }

        public override UserDTO Map(AnotherBlogUser source, UserDTO destination)
        {
            return AutoMapper.Mapper.Map(source, destination);
        }
    }
}
