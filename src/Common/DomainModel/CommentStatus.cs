using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlwaysMoveForward.AnotherBlog.Common.DomainModel
{
    /// <summary>
    /// What are the allowed comment statuses?  
    /// </summary>
    public enum CommentStatus
    {
        Unapproved = 0,
        Approved = 1,
        Deleted = 2,
        None = 99
    }
}
