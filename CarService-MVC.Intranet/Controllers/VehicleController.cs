using CarService_MVC.Data.Data;
using CarService_MVC.Data.Models;
using CarService_MVC.Intranet.Helpers;
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

    public IActionResult Index(string? search)
    {
        var query = dbAutoSerwisContext.Vehicles
            .Include(v => v.Client)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(v =>
                v.Brand.Contains(search) ||
                v.Model.Contains(search) ||
                (v.LicensePlate != null && v.LicensePlate.Contains(search)) ||
                (v.Vin != null && v.Vin.Contains(search)) ||
                v.Client.LastName.Contains(search));
        }

        ViewBag.Search = search;
        var vehicles = query
            .OrderBy(v => v.Brand)
            .ThenBy(v => v.Model)
            .ToList();
        return View(vehicles);
    }

    public IActionResult Create()
    {
        ViewBag.Clients     = VehicleDropdowns.Clients(dbAutoSerwisContext);
        ViewBag.EngineTypes = VehicleDropdowns.EngineTypes();
        return View();
    }

    [HttpPost]
    public IActionResult Create(Vehicle vehicle)
    {
        dbAutoSerwisContext.Vehicles.Add(vehicle);
        dbAutoSerwisContext.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var vehicle = dbAutoSerwisContext.Vehicles.Find(id);
        if (vehicle == null) return NotFound();
        ViewBag.Clients     = VehicleDropdowns.Clients(dbAutoSerwisContext, vehicle.ClientId);
        ViewBag.EngineTypes = VehicleDropdowns.EngineTypes(vehicle.EngineType);
        return View(vehicle);
    }

    [HttpPost]
    public IActionResult Edit(Vehicle posted)
    {
        var vehicle = dbAutoSerwisContext.Vehicles.Find(posted.Id);
        if (vehicle == null) return NotFound();
        vehicle.ClientId = posted.ClientId;
        vehicle.Brand = posted.Brand;
        vehicle.Model = posted.Model;
        vehicle.Year = posted.Year;
        vehicle.LicensePlate = posted.LicensePlate;
        vehicle.Vin = posted.Vin;
        vehicle.EngineType = posted.EngineType;
        dbAutoSerwisContext.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var vehicle = dbAutoSerwisContext.Vehicles
            .Include(v => v.Client)
            .FirstOrDefault(v => v.Id == id);
        if (vehicle == null) return NotFound();
        return View(vehicle);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        var vehicle = dbAutoSerwisContext.Vehicles.Find(id);
        if (vehicle != null)
        {
            dbAutoSerwisContext.Vehicles.Remove(vehicle);
            dbAutoSerwisContext.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}
