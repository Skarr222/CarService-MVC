using CarService_MVC.Data.Data;
using CarService_MVC.Data.Models;
using CarService_MVC.Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CarService_MVC.Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly AutoSerwisContext _db;
        public HomeController(AutoSerwisContext db) => _db = db;

        private Dictionary<string, string> GetCms(string section) =>
            _db.CmsContents
               .Where(c => c.Section == section && c.IsActive)
               .OrderBy(c => c.SortOrder)
               .ToDictionary(c => c.Key, c => c.Content);

        public IActionResult Index()
        {
            ViewBag.Cms = GetCms("home");
            return View();
        }

        public IActionResult Services()
        {
            ViewBag.Cms = GetCms("services");
            var categories = _db.ServiceCategories
                .Include(c => c.Services)
                .OrderBy(c => c.Name)
                .ToList();
            return View(categories);
        }

        public IActionResult About()
        {
            ViewBag.Cms = GetCms("about");
            var employees = _db.Employees
                .Where(e => e.IsActive)
                .OrderBy(e => e.LastName)
                .ToList();
            return View(employees);
        }

        public IActionResult Status()
        {
            ViewBag.Cms = GetCms("status");
            if (TempData["VerifyPhone"] is string phone)
            {
                ViewBag.VerifyPhone = phone;
                if (TempData["VerifyError"] is string err)
                    ViewBag.VerifyError = err;
            }
            if (TempData["StatusError"] is string statusErr)
                ViewBag.StatusError = statusErr;
            return View(new List<RepairOrder>());
        }

        [HttpPost]
        public IActionResult StatusRequest(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return RedirectToAction("Status");

            // Rate limit: max 3 kodów z tego samego numeru w ciągu godziny
            var recentCount = _db.ContactRequests
                .Count(c => c.Phone == phone
                         && c.Subject == "SMS Verification"
                         && c.CreatedAt > DateTime.Now.AddHours(-1));
            if (recentCount >= 3)
            {
                TempData["StatusError"] = "Przekroczono limit prób. Spróbuj ponownie za godzinę.";
                return RedirectToAction("Status");
            }

            // Sprawdź cicho czy numer istnieje — odpowiedź zawsze taka sama
            var exists = _db.Clients.Any(c => c.Phone == phone);
            if (exists)
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
            return RedirectToAction("Status");
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
                return RedirectToAction("Status");
            }

            entry.IsRead = true;
            _db.SaveChanges();

            var orders = _db.RepairOrders
                .Include(o => o.Vehicle)
                .Include(o => o.Client)
                .Include(o => o.RepairOrderServices).ThenInclude(s => s.Service)
                .Include(o => o.RepairStatusHistories)
                .Where(o => o.Client.Phone == phone)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            ViewBag.Cms = GetCms("status");
            ViewBag.ShowResults = true;
            return View("Status", orders);
        }

        public IActionResult Testimonials()
        {
            ViewBag.Cms = GetCms("testimonials");
            var testimonials = _db.Testimonials
                .Where(t => t.IsApproved)
                .OrderByDescending(t => t.CreatedAt)
                .ToList();
            return View(testimonials);
        }

        [HttpPost]
        public IActionResult AddTestimonial(string clientName, string content, int rating)
        {
            if (string.IsNullOrWhiteSpace(clientName) || string.IsNullOrWhiteSpace(content) || rating < 1 || rating > 5)
            {
                TempData["TestimonialError"] = "Wypełnij wszystkie pola i wybierz ocenę.";
                return RedirectToAction("Testimonials");
            }
            _db.Testimonials.Add(new Testimonial
            {
                ClientName = clientName,
                Content = content,
                Rating = rating,
                IsApproved = false,
                CreatedAt = DateTime.Now
            });
            _db.SaveChanges();
            TempData["TestimonialSuccess"] = "Dziękujemy! Twoja opinia zostanie opublikowana po moderacji.";
            return RedirectToAction("Testimonials");
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
            return RedirectToAction("Contact");
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
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Cms = GetCms("book");
                ViewBag.ServiceCategories = _db.ServiceCategories.OrderBy(c => c.Name).ToList();
                ViewBag.Error = "Wypełnij wszystkie wymagane pola.";
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

            var request = new ContactRequest
            {
                Name = name,
                Email = email,
                Phone = phone,
                Subject = "Rezerwacja wizyty",
                Message = string.Join("\n", parts),
                CreatedAt = DateTime.Now,
                IsRead = false
            };
            _db.ContactRequests.Add(request);
            _db.SaveChanges();
            TempData["Success"] = "Rezerwacja przyjęta! Skontaktujemy się wkrótce, aby potwierdzić termin.";
            return RedirectToAction("Book");
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
