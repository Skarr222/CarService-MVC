using CarService_MVC.Data.Data;
using CarService_MVC.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarService_MVC.Portal.Controllers;

public class ContactController : PortalBaseController
{
    public ContactController(AutoSerwisContext db) : base(db)
    {
    }

    public IActionResult Contact()
    {
        ViewBag.Cms = GetCms("contact");
        return View();
    }

    [HttpPost]
    public IActionResult Contact(ContactRequest model)
    {
        if (string.IsNullOrWhiteSpace(model.Name) ||
            string.IsNullOrWhiteSpace(model.Email) ||
            string.IsNullOrWhiteSpace(model.Message))
        {
            ViewBag.Cms = GetCms("contact");
            ViewBag.Error = "Wypełnij wszystkie wymagane pola.";
            return View(model);
        }

        if (string.IsNullOrWhiteSpace(model.Subject))
            model.Subject = "Wiadomość z Portalu";

        model.CreatedAt = DateTime.Now;
        model.IsRead = false;
        _db.ContactRequests.Add(model);
        _db.SaveChanges();
        TempData["Success"] = "Dziękujemy! Twoja wiadomość została wysłana.";
        return RedirectToAction(nameof(Contact));
    }
}