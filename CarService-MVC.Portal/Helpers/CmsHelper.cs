using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CarService_MVC.Portal.Helpers;

public static class CmsHelper
{
    public static Dictionary<string, string> FromViewBag(ViewDataDictionary viewData)
        => viewData["Cms"] as Dictionary<string, string> ?? new Dictionary<string, string>();

    public static string Get(this Dictionary<string, string> cms, string key, string fallback = "")
    {
        return cms.TryGetValue(key, out var value) ? value : fallback;
    }
}