using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map
{
    public interface IBlogPost
    {
        int EntryId { get; set; }
        bool IsPublished { get; set; }
        int BlogId { get; set; }
        int AuthorId { get; set; }
        string EntryText { get; set; }
        string Title { get; set; }
        DateTime DatePosted { get; set; }
        DateTime DateCreated { get; set; }
        int TimesViewed { get; set; }

        int GetCommentCount();
    }
}
