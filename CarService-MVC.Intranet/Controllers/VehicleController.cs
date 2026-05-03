using CarService_MVC.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarService_MVC.Intranet.Controllers;

public class VehicleController : Controller
{
    private readonly ILogger<VehicleController> _logger;
    private readonly AutoSerwisContext dbAutoSerwisContext;

    public VehicleController(AutoSerwisContext dbAutoSerwisContext, ILogger<VehicleController> logger)
    {
        _logger = logger;
        this.dbAutoSerwisContext = dbAutoSerwisContext;
    }

    public IActionResult Index()
    {
        var vehicles = dbAutoSerwisContext.Vehicles
            .Include(vehicle => vehicle.Client)
            .ToList();
        return View(vehicles);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(object model)
    {
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        return View();
    }

    [HttpPost]
    public IActionResult Edit(object model)
    {
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        return View();
    }

    [HttpPost]
    [ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        return RedirectToAction("Index");
    }
}