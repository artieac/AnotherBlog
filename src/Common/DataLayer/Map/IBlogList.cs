﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map
{
    public interface IBlogList
    {
        int Id { get; set; }
        Blog Blog { get; set; }
        string Name { get; set; }
        bool ShowOrdered { get; set; }
    }
}
