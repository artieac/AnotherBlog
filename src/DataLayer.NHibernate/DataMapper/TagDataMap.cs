using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.DataLayer.DTO;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.DataMapper
{
    public class TagDataMap : DataMapBase<Tag, TagDTO>
    {
        static TagDataMap()
        {
            TagDataMap.ConfigureAutoMapper();
        }

        internal static void ConfigureAutoMapper()
        {
            if (AutoMapper.Mapper.FindTypeMapFor<Tag, TagDTO>() == null)
            {
                AutoMapper.Mapper.CreateMap<Tag, TagDTO>()
                    .ForMember(dest => dest.BlogEntries, opt => opt.Ignore());
            }

            if (AutoMapper.Mapper.FindTypeMapFor<TagDTO, Tag>() == null)
            {
                AutoMapper.Mapper.CreateMap<TagDTO, Tag>();
            }

#if DEBUG
            AutoMapper.Mapper.AssertConfigurationIsValid();
#endif
        }

        public override Tag Map(TagDTO source, Tag destination)
        {
            if (destination == null)
            {
                destination = new Tag();
            }

            return AutoMapper.Mapper.Map(source, destination);
        }

        public override TagDTO Map(Tag source, TagDTO destination)
        {
            if (destination == null)
            {
                destination = new TagDTO();
            }

            return AutoMapper.Mapper.Map(source, destination);
        }
    }
}
