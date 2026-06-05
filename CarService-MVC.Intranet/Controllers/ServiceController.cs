using CarService_MVC.Data.Data;
using CarService_MVC.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarService_MVC.Intranet.Controllers;

public class ServiceController : Controller
{
    private readonly ILogger<ServiceController> _logger;
    private readonly AutoSerwisContext dbAutoSerwisContext;

    public ServiceController(AutoSerwisContext dbAutoSerwisContext, ILogger<ServiceController> logger)
    {
        _logger = logger;
        this.dbAutoSerwisContext = dbAutoSerwisContext;
    }

    public IActionResult Index(string? search)
    {
        var query = dbAutoSerwisContext.Services
            .Include(s => s.ServiceCategory)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(s =>
                s.Name.Contains(search) ||
                (s.Description != null && s.Description.Contains(search)) ||
                s.ServiceCategory.Name.Contains(search));
        }

        ViewBag.Search = search;
        var services = query
            .OrderBy(s => s.ServiceCategory.Name)
            .ThenBy(s => s.Name)
            .ToList();
        return View(services);
    }

    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(dbAutoSerwisContext.ServiceCategories.OrderBy(c => c.Name), "Id", "Name");
        return View();
    }

    [HttpPost]
    public IActionResult Create(Service model)
    {
        dbAutoSerwisContext.Services.Add(model);
        dbAutoSerwisContext.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var service = dbAutoSerwisContext.Services.Find(id);
        if (service == null) return NotFound();
        ViewBag.Categories = new SelectList(dbAutoSerwisContext.ServiceCategories.OrderBy(c => c.Name), "Id", "Name", service.ServiceCategoryId);
        return View(service);
    }

    [HttpPost]
    public IActionResult Edit(Service model)
    {
        var service = dbAutoSerwisContext.Services.Find(model.Id);
        if (service == null) return NotFound();
        service.ServiceCategoryId = model.ServiceCategoryId;
        service.Name = model.Name;
        service.Description = model.Description;
        service.EstimatedPrice = model.EstimatedPrice;
        service.EstimatedDuration = model.EstimatedDuration;
        dbAutoSerwisContext.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var service = dbAutoSerwisContext.Services
            .Include(s => s.ServiceCategory)
            .FirstOrDefault(s => s.Id == id);
        if (service == null) return NotFound();
        return View(service);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        var service = dbAutoSerwisContext.Services
            .Include(s => s.RepairOrderServices)
            .FirstOrDefault(s => s.Id == id);
        if (service != null)
        {
            dbAutoSerwisContext.RepairOrderServices.RemoveRange(service.RepairOrderServices);
            dbAutoSerwisContext.Services.Remove(service);
            dbAutoSerwisContext.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}
