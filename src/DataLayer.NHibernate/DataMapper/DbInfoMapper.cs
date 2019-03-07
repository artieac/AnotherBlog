using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PucksAndProgramming.Common.DomainModel;
using PucksAndProgramming.Common.DomainModel.DataMap;
using PucksAndProgramming.Common.DataLayer;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Common.DataLayer.Map;
using PucksAndProgramming.AnotherBlog.DataLayer.DTO;

namespace PucksAndProgramming.AnotherBlog.DataLayer.DataMapper
{
    public class DbInfoMapper : DataMapBase<DbInfo, DbInfoDTO>
    {
        static DbInfoMapper()
        {
            if (AutoMapper.Mapper.FindTypeMapFor<DbInfo, DbInfoDTO>() == null)
            {
                AutoMapper.Mapper.CreateMap<DbInfo, DbInfoDTO>();
            }

            if (AutoMapper.Mapper.FindTypeMapFor<DbInfoDTO, DbInfo>() == null)
            {
                AutoMapper.Mapper.CreateMap<DbInfoDTO, DbInfo>();
            }
#if DEBUG
            AutoMapper.Mapper.AssertConfigurationIsValid();
#endif
        }

        public override DbInfoDTO Map(DbInfo source, DbInfoDTO destination)
        {
            return AutoMapper.Mapper.Map(source, destination);
        }

        public override DbInfo Map(DbInfoDTO source, DbInfo destination)
        {
            return AutoMapper.Mapper.Map(source, destination);
        }
    }
}
