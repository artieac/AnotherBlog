using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.DataLayer.DTO;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.DataMapper
{
    public class SiteInfoDataMap : DataMapBase<SiteInfo, SiteInfoDTO>
    {
        static SiteInfoDataMap()
        {
            if (AutoMapper.Mapper.FindTypeMapFor<SiteInfo, SiteInfoDTO>() == null)
            {
                AutoMapper.Mapper.CreateMap<SiteInfo, SiteInfoDTO>();
            }

            if (AutoMapper.Mapper.FindTypeMapFor<SiteInfoDTO, SiteInfo>() == null)
            {
                AutoMapper.Mapper.CreateMap<SiteInfoDTO, SiteInfo>();
            }

#if DEBUG
            AutoMapper.Mapper.AssertConfigurationIsValid();
#endif
        }

        public override SiteInfo Map(SiteInfoDTO source, SiteInfo destination)
        {
            return AutoMapper.Mapper.Map(source, destination);
        }

        public override SiteInfoDTO Map(SiteInfo source, SiteInfoDTO destination)
        {
            return AutoMapper.Mapper.Map(source, destination);
        }
    }
}
