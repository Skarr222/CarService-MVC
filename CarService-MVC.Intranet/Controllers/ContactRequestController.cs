using CarService_MVC.Data.Data;
using CarService_MVC.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarService_MVC.Intranet.Controllers;

public class ContactRequestController : Controller
{
    private readonly ILogger<ContactRequestController> _logger;
    private readonly AutoSerwisContext dbAutoSerwisContext;

    public ContactRequestController(AutoSerwisContext dbAutoSerwisContext, ILogger<ContactRequestController> logger)
    {
        _logger = logger;
        this.dbAutoSerwisContext = dbAutoSerwisContext;
    }

    public IActionResult Index(string? search)
    {
        var query = dbAutoSerwisContext.ContactRequests.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c =>
                c.Name.Contains(search) ||
                c.Email.Contains(search) ||
                (c.Subject != null && c.Subject.Contains(search)) ||
                (c.Phone != null && c.Phone.Contains(search)));
        }

        ViewBag.Search = search;
        var requests = query
            .OrderByDescending(c => c.CreatedAt)
            .ToList();
        return View(requests);
    }

    public IActionResult Edit(int id)
    {
        var request = dbAutoSerwisContext.ContactRequests.Find(id);
        if (request == null) return NotFound();
        return View(request);
    }

    [HttpPost]
    public IActionResult Edit(ContactRequest model)
    {
        var request = dbAutoSerwisContext.ContactRequests.Find(model.Id);
        if (request == null) return NotFound();
        request.IsRead = model.IsRead;
        request.ResponseNote = model.ResponseNote;
        dbAutoSerwisContext.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var request = dbAutoSerwisContext.ContactRequests.Find(id);
        if (request == null) return NotFound();
        return View(request);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        var request = dbAutoSerwisContext.ContactRequests.Find(id);
        if (request != null)
        {
            dbAutoSerwisContext.ContactRequests.Remove(request);
            dbAutoSerwisContext.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}
