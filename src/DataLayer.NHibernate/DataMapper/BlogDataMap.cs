using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.DataLayer.DTO;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.DataMapper
{
    public class BlogDataMap : DataMapBase<Blog, BlogDTO>
    {
        static BlogDataMap()
        {
            BlogDataMap.ConfigureAutoMapper();
        }

        internal static void ConfigureAutoMapper()
        {
            if (AutoMapper.Mapper.FindTypeMapFor<Blog, BlogDTO>() == null)
            {
                AutoMapper.Mapper.CreateMap<Blog, BlogDTO>()
                    .ForMember(dest => dest.Posts, opt => opt.Ignore());
            }

            if (AutoMapper.Mapper.FindTypeMapFor<BlogDTO, Blog>() == null)
            {
                AutoMapper.Mapper.CreateMap<BlogDTO, Blog>();
            }

#if DEBUG
            AutoMapper.Mapper.AssertConfigurationIsValid();
#endif
        }

        public override Blog Map(BlogDTO source, Blog destination)
        {
            return AutoMapper.Mapper.Map(source, destination);
        }

        public override BlogDTO Map(Blog source, BlogDTO destination)
        {
            return AutoMapper.Mapper.Map(source, destination);
        }
    }
}
