using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CarService_MVC.Portal.Helpers;

public static class CmsHelper
{
    public static Dictionary<string, string> FromViewBag(ViewDataDictionary viewData)
        => viewData["Cms"] as Dictionary<string, string> ?? new Dictionary<string, string>();

    public static Dictionary<string, string> NavbarCms(ViewDataDictionary viewData)
        => viewData["NavbarCms"] as Dictionary<string, string> ?? new Dictionary<string, string>();

    public static Dictionary<string, string> FooterCms(ViewDataDictionary viewData)
        => viewData["FooterCms"] as Dictionary<string, string> ?? new Dictionary<string, string>();

    public static string Get(this Dictionary<string, string> cms, string key, string fallback = "")
    {
        return cms.TryGetValue(key, out var value) ? value : fallback;
    }
}