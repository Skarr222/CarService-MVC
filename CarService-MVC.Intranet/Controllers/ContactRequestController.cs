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

    public IActionResult Index()
    {
        var requests = dbAutoSerwisContext.ContactRequests
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
