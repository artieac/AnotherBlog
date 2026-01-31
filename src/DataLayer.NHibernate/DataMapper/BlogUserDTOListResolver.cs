using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.DataLayer.DTO;
using AlwaysMoveForward.Common.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.DataMapper
{
    public class BlogUserDTOListResolver : MappedListResolver<BlogUserDTO, AnotherBlogUser>
    {
        protected override BlogUserDTO FindItemInList(IList<BlogUserDTO> destinationList, AnotherBlogUser searchTarget)
        {
            return destinationList.FirstOrDefault(t => t.Id == searchTarget.Id);
        }

        protected override AnotherBlogUser FindItemInList(IList<AnotherBlogUser> sourceList, BlogUserDTO searchTarget)
        {
            return sourceList.FirstOrDefault(t => t.Id == searchTarget.Id);
        }
    }
}
