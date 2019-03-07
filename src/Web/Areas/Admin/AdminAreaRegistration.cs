using System.Web.Mvc;

namespace PucksAndProgramming.AnotherBlog.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DefaultAdmin",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_BlogSubFolder",
                "Admin/{controller}/{action}/{blogSubFolder}/{id}",
                new { id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_PostSpecific",
                "Admin/{controller}/{action}/{blogSubFolder}/{blogPostId}/{filter}",
                new { filter = UrlParameter.Optional }
);

        }
    }
}
