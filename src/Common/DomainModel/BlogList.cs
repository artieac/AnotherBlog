using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;

namespace AlwaysMoveForward.AnotherBlog.Common.DomainModel
{
    public class BlogList 
    {
        public BlogList()
        {
            this.Id = -1;
        }

        public int Id { get; set; }
        public int BlogId { get; set; }
        public string Name { get; set; }
        public bool ShowOrdered { get; set; }
        public IList<BlogListItem> Items { get; set; }

        public BlogListItem AddItem(int listItemId, string itemName, string relatedLink, int displayOrder)
        {
            BlogListItem targetItem = this.Items.FirstOrDefault(t => t.Id == listItemId);

            if (targetItem == null)
            {
                targetItem = new BlogListItem();
                this.Items.Add(targetItem);
            }

            targetItem.Name = itemName;
            targetItem.RelatedLink = relatedLink;
            targetItem.DisplayOrder = displayOrder;

            return targetItem;
        }

        public bool RemoveItem(int listItemId)
        {
            bool retVal = false;

            BlogListItem targetItem = this.Items.FirstOrDefault(t => t.Id == listItemId);

            if (targetItem != null)
            {
                retVal = this.RemoveItem(targetItem);
            }

            return retVal;
        }

        public bool RemoveItem(BlogListItem listItem)
        {
            bool retVal = false;

            if (listItem != null)
            {
                this.Items.Remove(listItem);
                retVal = true;
            }

            return retVal;
        }
    }
}
