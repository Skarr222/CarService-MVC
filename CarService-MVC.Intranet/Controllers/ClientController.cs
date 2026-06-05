using CarService_MVC.Data.Data;
using CarService_MVC.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarService_MVC.Intranet.Controllers;

public class ClientController : Controller
{
    private readonly ILogger<ClientController> _logger;
    private readonly AutoSerwisContext dbAutoSerwisContext;

    public ClientController(AutoSerwisContext dbAutoSerwisContext, ILogger<ClientController> logger)
    {
        _logger = logger;
        this.dbAutoSerwisContext = dbAutoSerwisContext;
    }

    public IActionResult Index(string? search)
    {
        var query = dbAutoSerwisContext.Clients
            .Include(c => c.Vehicles)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c =>
                c.FirstName.Contains(search) ||
                c.LastName.Contains(search) ||
                (c.Email != null && c.Email.Contains(search)) ||
                (c.Phone != null && c.Phone.Contains(search)));
        }

        ViewBag.Search = search;
        var clients = query
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .ToList();
        return View(clients);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Client model)
    {
        model.CreatedAt = DateTime.Now;
        dbAutoSerwisContext.Clients.Add(model);
        dbAutoSerwisContext.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var client = dbAutoSerwisContext.Clients.Find(id);
        if (client == null) return NotFound();
        return View(client);
    }

    [HttpPost]
    public IActionResult Edit(Client model)
    {
        var client = dbAutoSerwisContext.Clients.Find(model.Id);
        if (client == null) return NotFound();
        client.FirstName = model.FirstName;
        client.LastName = model.LastName;
        client.Email = model.Email;
        client.Phone = model.Phone;
        client.IsActive = model.IsActive;
        dbAutoSerwisContext.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var client = dbAutoSerwisContext.Clients.Find(id);
        if (client == null) return NotFound();
        return View(client);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        var client = dbAutoSerwisContext.Clients.Find(id);
        if (client != null)
        {
            dbAutoSerwisContext.Clients.Remove(client);
            dbAutoSerwisContext.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}
