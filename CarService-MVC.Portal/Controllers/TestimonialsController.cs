using CarService_MVC.Data.Data;
using CarService_MVC.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarService_MVC.Portal.Controllers;

public class TestimonialsController : PortalBaseController
{
    public TestimonialsController(AutoSerwisContext db) : base(db) { }

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
            return RedirectToAction(nameof(Testimonials));
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
        return RedirectToAction(nameof(Testimonials));
    }
}
