using CarService_MVC.Data.Data;
using CarService_MVC.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarService_MVC.Intranet.Controllers;

public class ServiceCategoryController : Controller
{
    private readonly ILogger<ServiceCategoryController> _logger;
    private readonly AutoSerwisContext dbAutoSerwisContext;

    public ServiceCategoryController(AutoSerwisContext dbAutoSerwisContext, ILogger<ServiceCategoryController> logger)
    {
        _logger = logger;
        this.dbAutoSerwisContext = dbAutoSerwisContext;
    }

    public IActionResult Index(string? search)
    {
        var query = dbAutoSerwisContext.ServiceCategories
            .Include(c => c.Services)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c =>
                c.Name.Contains(search) ||
                (c.Description != null && c.Description.Contains(search)));
        }

        ViewBag.Search = search;
        var categories = query
            .OrderBy(c => c.Name)
            .ToList();
        return View(categories);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(ServiceCategory model)
    {
        dbAutoSerwisContext.ServiceCategories.Add(model);
        dbAutoSerwisContext.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var category = dbAutoSerwisContext.ServiceCategories.Find(id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost]
    public IActionResult Edit(ServiceCategory model)
    {
        var category = dbAutoSerwisContext.ServiceCategories.Find(model.Id);
        if (category == null) return NotFound();
        category.Name = model.Name;
        category.Description = model.Description;
        category.IconCss = model.IconCss;
        dbAutoSerwisContext.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var category = dbAutoSerwisContext.ServiceCategories
            .Include(c => c.Services)
            .FirstOrDefault(c => c.Id == id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        var category = dbAutoSerwisContext.ServiceCategories
            .Include(c => c.Services)
            .ThenInclude(s => s.RepairOrderServices)
            .FirstOrDefault(c => c.Id == id);
        if (category != null)
        {
            foreach (var service in category.Services)
                dbAutoSerwisContext.RepairOrderServices.RemoveRange(service.RepairOrderServices);
            dbAutoSerwisContext.Services.RemoveRange(category.Services);
            dbAutoSerwisContext.ServiceCategories.Remove(category);
            dbAutoSerwisContext.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}
