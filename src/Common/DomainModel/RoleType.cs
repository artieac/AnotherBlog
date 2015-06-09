using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlwaysMoveForward.AnotherBlog.Common.DomainModel
{
    public class RoleType
    {
        public static Dictionary<RoleType.Id, string> Roles;

        static RoleType()
        {
            RoleType.Roles = new Dictionary<RoleType.Id, string>();
            RoleType.Roles.Add(RoleType.Id.Reader, RoleType.Id.Reader.ToString());
            RoleType.Roles.Add(RoleType.Id.Administrator, RoleType.Id.Administrator.ToString());
            RoleType.Roles.Add(RoleType.Id.Blogger, RoleType.Id.Blogger.ToString());
        }

        public enum Id
        {
            Administrator = 1,
            Blogger = 2,
            Reader = 3
        }

        public class Names
        {
            public const string Reader = "Reader";
            public const string SiteAdministrator = "SiteAdministrator";
            public const string Administrator = "Administrator";
            public const string Blogger = "Blogger";
        }
    }
}
