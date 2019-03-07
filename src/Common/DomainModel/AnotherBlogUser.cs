using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PucksAndProgramming.Common.DomainModel;

namespace PucksAndProgramming.AnotherBlog.Common.DomainModel
{
    public class AnotherBlogUser : RemoteOAuthUser
    {
        public AnotherBlogUser() : base()
        {

        }

        public bool ApprovedCommenter { get; set; }
        public bool IsSiteAdministrator { get; set; }
        public string About { get; set; }

        public IDictionary<long, RoleType.Id> Roles { get; set; }

        public void AddRole(long blogId, RoleType.Id roleId)
        {
            if(this.Roles == null)
            {
                this.Roles = new Dictionary<long, RoleType.Id>();
            }

            if (this.Roles.ContainsKey(blogId))
            {
                this.Roles[blogId] = roleId;
            }
            else
            {
                this.Roles.Add(blogId, roleId);
            }
        }

        public void RemoveRole(long blogId)
        {
            if (this.Roles == null)
            {
                this.Roles = new Dictionary<long, RoleType.Id>();
            }

            if (this.Roles.ContainsKey(blogId))
            {
                this.Roles.Remove(blogId);
            }
        }
    }
}
