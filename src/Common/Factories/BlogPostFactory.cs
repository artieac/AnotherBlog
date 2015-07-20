using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Common.Factories
{
    public class BlogPostFactory
    {
        public static BlogPost Create(Blog ownerBlog, AnotherBlogUser currentUser)
        {
            BlogPost retVal = new BlogPost();
            retVal.DateCreated = DateTime.Now;
            retVal.Author = currentUser;
            retVal.Blog = ownerBlog;

            return retVal;
        }

        public static Comment CreateComment(string authorName, string authorEmail, string commentLink, string commentText)
        {
            Comment retVal = new Comment();
            retVal.AuthorName = authorName;
            retVal.AuthorEmail = authorEmail;
            retVal.DatePosted = DateTime.Now;
            retVal.Link = commentLink;
            retVal.Status = CommentStatus.Unapproved;
            retVal.Text = commentText;

            return retVal;
        }
    }
}
