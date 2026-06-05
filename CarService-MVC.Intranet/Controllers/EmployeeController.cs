using CarService_MVC.Data.Data;
using CarService_MVC.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarService_MVC.Intranet.Controllers;

public class EmployeeController : Controller
{
    private readonly ILogger<EmployeeController> _logger;
    private readonly AutoSerwisContext dbAutoSerwisContext;

    public EmployeeController(AutoSerwisContext dbAutoSerwisContext, ILogger<EmployeeController> logger)
    {
        _logger = logger;
        this.dbAutoSerwisContext = dbAutoSerwisContext;
    }

    public IActionResult Index(string? search)
    {
        var query = dbAutoSerwisContext.Employees.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e =>
                e.FirstName.Contains(search) ||
                e.LastName.Contains(search) ||
                (e.Position != null && e.Position.Contains(search)) ||
                (e.Email != null && e.Email.Contains(search)) ||
                (e.Phone != null && e.Phone.Contains(search)));
        }

        ViewBag.Search = search;
        var employees = query
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ToList();
        return View(employees);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Employee model)
    {
        dbAutoSerwisContext.Employees.Add(model);
        dbAutoSerwisContext.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var employee = dbAutoSerwisContext.Employees.Find(id);
        if (employee == null) return NotFound();
        return View(employee);
    }

    [HttpPost]
    public IActionResult Edit(Employee model)
    {
        var employee = dbAutoSerwisContext.Employees.Find(model.Id);
        if (employee == null) return NotFound();
        employee.FirstName = model.FirstName;
        employee.LastName = model.LastName;
        employee.Position = model.Position;
        employee.Phone = model.Phone;
        employee.Email = model.Email;
        employee.HireDate = model.HireDate;
        employee.IsActive = model.IsActive;
        dbAutoSerwisContext.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var employee = dbAutoSerwisContext.Employees.Find(id);
        if (employee == null) return NotFound();
        return View(employee);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        var employee = dbAutoSerwisContext.Employees.Find(id);
        if (employee != null)
        {
            dbAutoSerwisContext.Employees.Remove(employee);
            dbAutoSerwisContext.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}
