using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map
{
    public interface IComment
    {
        int CommentId { get; set; }
        int Status { get; set; }
        Blog Blog { get; set; }
        string Link { get; set; }
        string AuthorEmail { get; set; }
        string Text { get; set; }
        string AuthorName { get; set; }
        BlogPost Post { get; set; }
        DateTime DatePosted { get; set; }
    }
}
