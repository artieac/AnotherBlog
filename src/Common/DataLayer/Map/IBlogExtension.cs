using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map
{
    public interface IBlogExtension
    {
        int ExtensionId { get; set; }
        int PageLocation { get; set; }
        int SectionOrder { get; set; }
        string AssemblyName { get; set; }
        string ClassName { get; set; }
        string AssemblyPath { get; set; }
    }
}
