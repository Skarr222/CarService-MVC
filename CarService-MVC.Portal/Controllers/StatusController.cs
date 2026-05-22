using CarService_MVC.Data.Data;
using CarService_MVC.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarService_MVC.Portal.Controllers;

public class StatusController : PortalBaseController
{
    public StatusController(AutoSerwisContext db) : base(db) { }

    public IActionResult Status()
    {
        ViewBag.Cms = GetCms("status");

        if (TempData["VerifyPhone"] is string phone)
        {
            ViewBag.VerifyPhone = phone;
            if (TempData["VerifyError"] is string verifyError)
                ViewBag.VerifyError = verifyError;
        }

        if (TempData["StatusError"] is string statusError)
            ViewBag.StatusError = statusError;

        return View(new List<RepairOrder>());
    }

    [HttpPost]
    public IActionResult StatusRequest(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return RedirectToAction(nameof(Status));

        var recentCodeCount = _db.ContactRequests
            .Count(c => c.Phone == phone
                        && c.Subject == "SMS Verification"
                        && c.CreatedAt > DateTime.Now.AddHours(-1));

        if (recentCodeCount >= 3)
        {
            TempData["StatusError"] = "Przekroczono limit prób. Spróbuj ponownie za godzinę.";
            return RedirectToAction(nameof(Status));
        }

        if (_db.Clients.Any(c => c.Phone == phone))
        {
            var code = new Random().Next(100000, 999999).ToString();
            _db.ContactRequests.Add(new ContactRequest
            {
                Name = "SMS",
                Email = "sms@system",
                Phone = phone,
                Subject = "SMS Verification",
                Message = code,
                IsRead = false,
                CreatedAt = DateTime.Now
            });
            _db.SaveChanges();
        }

        TempData["VerifyPhone"] = phone;
        return RedirectToAction(nameof(Status));
    }

    [HttpPost]
    public IActionResult StatusVerify(string phone, string code)
    {
        var entry = _db.ContactRequests
            .Where(c => c.Phone == phone
                        && c.Subject == "SMS Verification"
                        && !c.IsRead
                        && c.CreatedAt > DateTime.Now.AddMinutes(-10))
            .OrderByDescending(c => c.CreatedAt)
            .FirstOrDefault();

        if (entry == null || entry.Message != code)
        {
            TempData["VerifyPhone"] = phone;
            TempData["VerifyError"] = "Nieprawidłowy lub wygasły kod. Sprawdź i spróbuj ponownie.";
            return RedirectToAction(nameof(Status));
        }

        entry.IsRead = true;
        _db.SaveChanges();

        var orders = _db.RepairOrders
            .Include(o => o.Vehicle)
            .Include(o => o.Client)
            .Include(o => o.RepairOrderServices).ThenInclude(orderService => orderService.Service)
            .Include(o => o.RepairStatusHistories)
            .Where(o => o.Client.Phone == phone)
            .OrderByDescending(o => o.CreatedAt)
            .ToList();

        ViewBag.Cms = GetCms("status");
        ViewBag.ShowResults = true;
        return View(nameof(Status), orders);
    }
}
