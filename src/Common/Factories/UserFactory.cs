using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;

namespace PucksAndProgramming.AnotherBlog.Common.Factories
{
    public class UserFactory
    {
        private const string GuestUserName = "guest";

        public static AnotherBlogUser CreateGuestUser()
        {
            AnotherBlogUser retVal = new AnotherBlogUser();
            retVal.ApprovedCommenter = false;
            retVal.IsSiteAdministrator = false;
            retVal.Roles = new Dictionary<long, RoleType.Id>();

            return retVal;
        }

        public static AnotherBlogUser Create(PucksAndProgramming.Common.DomainModel.User oauthUser)
        {
            AnotherBlogUser retVal = new AnotherBlogUser();
            retVal.OAuthServiceUserId = oauthUser.Id;
            retVal.FirstName = oauthUser.FirstName;
            retVal.LastName = oauthUser.LastName;
            retVal.IsSiteAdministrator = false;
            retVal.ApprovedCommenter = false;

            return retVal;
        }
    }
}
