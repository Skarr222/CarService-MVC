using System.Diagnostics;
using CarService_MVC.Data.Data;
using CarService_MVC.Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarService_MVC.Portal.Controllers;

public class HomeController : PortalBaseController
{
    public HomeController(AutoSerwisContext db) : base(db) { }

    public IActionResult Index()
    {
        ViewBag.Cms = GetCms("home");
        return View();
    }

    public IActionResult Services()
    {
        ViewBag.Cms = GetCms("services");
        var categories = _db.ServiceCategories
            .Include(c => c.Services)
            .OrderBy(c => c.Name)
            .ToList();
        return View(categories);
    }

    public IActionResult About()
    {
        ViewBag.Cms = GetCms("about");
        var employees = _db.Employees
            .Where(e => e.IsActive)
            .OrderBy(e => e.LastName)
            .ToList();
        return View(employees);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
