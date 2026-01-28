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
        get
        {
            if (_siteInfo == null)
            {
                try
                {
                    ServiceManager serviceManager = ServiceManagerBuilder.BuildServiceManager();

                    if (serviceManager != null)
                    {
                        _siteInfo = serviceManager.SiteInfoService.GetSiteInfo();

                        if (_siteInfo == null)
                        {
                            _siteInfo = new SiteInfo { Name = "Default" };
                        }
                    }
                    else
                    {
                        _siteInfo = new SiteInfo { Name = "Default" };
                    }
                }
                catch
                {
                    _siteInfo = new SiteInfo { Name = "Default" };
                }
            }

            return _siteInfo;
        }
        set => _siteInfo = value;
    }

    public static void Initialize(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var settings = scope.ServiceProvider.GetService<IOptions<WebSiteSettings>>();
        _siteSettings = settings?.Value ?? new WebSiteSettings();
    }
}
