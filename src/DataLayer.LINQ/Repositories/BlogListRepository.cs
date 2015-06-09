using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer.Entities;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class BlogListRepository : LINQRepository<BlogList, BlogListDTO>, IBlogListRepository
    {
        internal BlogListRepository(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
            : base(unitOfWork, repositoryManager)
        {
        }

        public override string IdPropertyName
        {
            get { return "Id"; }
        }

        protected override BlogListDTO GetDTOByDomain(BlogList targetItem)
        {
            return this.GetDtoById(targetItem.Id);
        }

        public IList<BlogList> GetByBlog(int blogId)
        {
            IQueryable<BlogListDTO> dtoList = null;

            dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.BlogListDTOs
                        where foundItem.BlogId == blogId
                        select foundItem;

            return this.Map(dtoList.ToList<BlogListDTO>());
        }

        public override BlogList Save(BlogList itemToSave)
        {
            BlogListDTO targetItem = this.GetDTOByDomain(itemToSave);

            if (targetItem == null)
            {
                targetItem = this.Map(itemToSave);
                ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<BlogListDTO>().InsertOnSubmit(targetItem);
            }
            else
            {
                targetItem.Name = itemToSave.Name;
                targetItem.ShowOrdered = itemToSave.ShowOrdered;
            }

            this.UnitOfWork.Flush();

            return this.Map(targetItem);
        }
    }
}
