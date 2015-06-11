using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Web.Models.API
{
    public class GetCommentModel : Comment
    {
        public GetCommentModel(Comment comment)
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