using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Utilities;
using Microsoft.Extensions.Options;

namespace AlwaysMoveForward.AnotherBlog.Web;

public static class WebApplicationState
{
    private static SiteInfo _siteInfo;
    private static WebSiteSettings _siteSettings;

    public static string Version => "1.2.0";

    public static WebSiteSettings WebSiteConfiguration => _siteSettings;

    public static SiteInfo SiteInfo
    {
        get => _siteInfo ?? new SiteInfo { Name = "Default" };
        set => _siteInfo = value;
    }

    public static void Initialize(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var settings = scope.ServiceProvider.GetService<IOptions<WebSiteSettings>>();
        _siteSettings = settings?.Value ?? new WebSiteSettings();

        try
        {
            var serviceManagerBuilder = scope.ServiceProvider.GetService<ServiceManagerBuilder>();
            if (serviceManagerBuilder != null)
            {
                var serviceManager = serviceManagerBuilder.CreateServiceManager();
                if (serviceManager != null)
                {
                    _siteInfo = serviceManager.SiteInfoService.GetSiteInfo();
                    if (_siteInfo == null)
                    {
                        _siteInfo = new SiteInfo { Name = "Default" };
                    }
                }
            }
        }
        catch
        {
            _siteInfo = new SiteInfo { Name = "Default" };
        }
    }
}
