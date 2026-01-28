namespace AlwaysMoveForward.AnotherBlog.Web;

public class WebSiteSettings
{
    public bool UpdateDb { get; set; }
    public bool EnableSSL { get; set; }
    public string DefaultSiteName { get; set; } = "Default";
}

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}

public class OAuthSettings
{
    public string ServiceUri { get; set; } = string.Empty;
    public string RequestTokenUri { get; set; } = string.Empty;
    public string AccessTokenUri { get; set; } = string.Empty;
    public string AuthorizationUri { get; set; } = string.Empty;
    public string ConsumerKey { get; set; } = string.Empty;
    public string ConsumerSecret { get; set; } = string.Empty;
}
