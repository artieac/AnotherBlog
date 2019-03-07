using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PucksAndProgramming.AnotherBlog.Web.Models.API
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string AuthorName { get; set;}
        public string AuthorEmail { get; set;}
        public string CommentText { get; set;}
        public String CommentLink { get; set; }
    }
}