using CarService_MVC.Data.Data;
using CarService_MVC.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarService_MVC.Portal.Controllers;

public class BookingController : PortalBaseController
{
    public BookingController(AutoSerwisContext db) : base(db)
    {
    }

    public IActionResult Book()
    {
        ViewBag.Cms = GetCms("book");
        ViewBag.ServiceCategories = _db.ServiceCategories.OrderBy(c => c.Name).ToList();
        return View();
    }

    [HttpPost]
    public IActionResult Book(string name, string email, string? phone,
        string? preferredDate, string? preferredTime,
        string? serviceCategory, string? carMake, string? carModel,
        string? licensePlate, string? message)
    {
        var msgs = GetCms("messages");
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
        {
            ViewBag.Cms = GetCms("book");
            ViewBag.ServiceCategories = _db.ServiceCategories.OrderBy(c => c.Name).ToList();
            ViewBag.Error = msgs.TryGetValue("validation.required", out var v) ? v : "Wypełnij wszystkie wymagane pola.";
            return View();
        }

        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(serviceCategory))
            parts.Add($"Kategoria usługi: {serviceCategory}");
        if (!string.IsNullOrWhiteSpace(carMake) || !string.IsNullOrWhiteSpace(carModel))
            parts.Add($"Pojazd: {carMake} {carModel}".Trim());
        if (!string.IsNullOrWhiteSpace(licensePlate))
            parts.Add($"Tablica rejestracyjna: {licensePlate}");
        if (!string.IsNullOrWhiteSpace(preferredDate))
        {
            var dateStr = string.IsNullOrWhiteSpace(preferredTime)
                ? preferredDate
                : $"{preferredDate} godz. {preferredTime}";
            parts.Add($"Preferowany termin: {dateStr}");
        }

        if (!string.IsNullOrWhiteSpace(message))
            parts.Add($"Uwagi: {message}");

        _db.ContactRequests.Add(new ContactRequest
        {
            Name = name,
            Email = email,
            Phone = phone,
            Subject = "Rezerwacja wizyty",
            Message = string.Join("\n", parts),
            CreatedAt = DateTime.Now,
            IsRead = false
        });
        _db.SaveChanges();
        TempData["Success"] = msgs.TryGetValue("booking.success", out var s) ? s : "Rezerwacja przyjęta! Skontaktujemy się wkrótce, aby potwierdzić termin.";
        return RedirectToAction(nameof(Book));
    }
}
