using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnotherBlog.IntegrationService.Models
{
    public class CommentElement
    {
        public int CommentId { get; set; }
        public int Status { get; set; }
        public string Link { get; set; }
        public string AuthorEmail { get; set; }
        public string Text { get; set; }
        public string AuthorName { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
