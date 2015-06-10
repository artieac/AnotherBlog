using System.Web.Mvc;

namespace AlwaysMoveForward.AnotherBlog.Web.Areas.API
{
    public class APIAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "API";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "API_default",
                "API/{controller}/{action}",
                new { action = "Index"}
            );

            context.MapRoute(
                "API_BlogSubFolderDefault",
                "{blogSubFolder}/API/{controller}/{action}",
                new { blogSubFolder = string.Empty, controller = string.Empty, action = "Index"}
            );

            context.MapRoute(
                "API_BlogSubFolderWithId",
                "{blogSubFolder}/API/{controller}/{action}/{id}",
                new { blogSubFolder = string.Empty, action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
