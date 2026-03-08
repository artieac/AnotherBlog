using AlwaysMoveForward.Common.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.DataLayer.DTO;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.DataMapper
{
    class BlogListItemDTOResolver : MappedListResolver<BlogListItem, BlogListItemDTO>
    { 
        protected override BlogListItemDTO FindItemInList(IList<BlogListItemDTO> destinationList, BlogListItem searchTarget)
        {
            return destinationList.FirstOrDefault(t => t.Id == searchTarget.Id);
        }

        protected override BlogListItem FindItemInList(IList<BlogListItem> sourceList, BlogListItemDTO searchTarget)
        {
            return sourceList.FirstOrDefault(t => t.Id == searchTarget.Id);
        }
    }
}
