using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map
{
    public interface IExtensionConfiguration
    {
        int ConfigurationId { get; set; }
        int ExtensionId { get; set; }
        string ExtensionSettings { get; set; }
    }
}
