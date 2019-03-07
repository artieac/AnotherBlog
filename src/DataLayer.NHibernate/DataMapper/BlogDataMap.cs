using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.DataLayer.DTO;
using PucksAndProgramming.Common.DataLayer;

namespace PucksAndProgramming.AnotherBlog.DataLayer.DataMapper
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
