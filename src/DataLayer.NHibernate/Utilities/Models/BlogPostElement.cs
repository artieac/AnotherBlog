using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnotherBlog.IntegrationService.Models
{
    public class BlogPostElement
    {
        public int EntryId { get; set; }
        public bool IsPublished { get; set; }
        public int BlogId { get; set; }
        public int AuthorId { get; set; }
        public String AuthorName { get; set; }
        public string EntryText { get; set; }
        public string Title { get; set; }
        public DateTime DatePosted { get; set; }
        public IList<TagElement> Tags{ get; set;}
    }
}
