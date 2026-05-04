using CarService_MVC.Data.Data;
using CarService_MVC.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            .Include(v => v.Client)
            .ToList();
        return View(vehicles);
    }

    public IActionResult Create()
    {
        PopulateDropdowns();
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
        PopulateDropdowns(vehicle.ClientId, vehicle.EngineType);
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
    [ActionName("Delete")]
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

    private void PopulateDropdowns(int? selectedClientId = null, EngineType? selectedEngineType = null)
    {
        ViewBag.Clients = new SelectList(
            dbAutoSerwisContext.Clients.Where(c => c.IsActive).OrderBy(c => c.LastName)
                .Select(c => new { c.Id, Name = c.FirstName + " " + c.LastName }),
            "Id", "Name", selectedClientId);

        ViewBag.EngineTypes = new SelectList(
            Enum.GetValues<EngineType>().Select(e => new { Value = e.ToString(), Text = e.ToString() }),
            "Value", "Text", selectedEngineType?.ToString());
    }
}
