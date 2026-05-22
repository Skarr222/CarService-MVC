using CarService_MVC.Data.Data;
using Microsoft.AspNetCore.Mvc;

namespace CarService_MVC.Portal.Controllers;

public abstract class PortalBaseController : Controller
{
    protected readonly AutoSerwisContext _db;

    protected PortalBaseController(AutoSerwisContext db)
    {
        _db = db;
    }

    protected Dictionary<string, string> GetCms(string section)
    {
        return _db.CmsContents
            .Where(c => c.Section == section && c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ToDictionary(c => c.Key, c => c.Content);
    }
}
