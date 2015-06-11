using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Web.Areas.Admin.Models
{
    public class CommentItemModel : Comment
    {
        public CommentItemModel(Comment comment)
        {
            this.AuthorEmail = comment.AuthorEmail;
            this.AuthorName = comment.AuthorName;
            this.Id = comment.Id;
            this.DatePosted = comment.DatePosted;
            this.Link = comment.Link;
            this.Status = comment.Status;
            this.Text = comment.Text;
        }

        public int BlogPostId { get; set; }
    }
}