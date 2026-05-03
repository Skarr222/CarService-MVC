using CarService_MVC.Data.Data;
using CarService_MVC.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarService_MVC.Intranet.Controllers;

public class TestimonialController : Controller
{
    private readonly ILogger<TestimonialController> _logger;
    private readonly AutoSerwisContext dbAutoSerwisContext;

    public TestimonialController(AutoSerwisContext dbAutoSerwisContext, ILogger<TestimonialController> logger)
    {
        _logger = logger;
        this.dbAutoSerwisContext = dbAutoSerwisContext;
    }

    public IActionResult Index()
    {
        var testimonials = dbAutoSerwisContext.Testimonials
            .OrderByDescending(t => t.CreatedAt)
            .ToList();
        return View(testimonials);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Testimonial model)
    {
        model.CreatedAt = DateTime.Now;
        dbAutoSerwisContext.Testimonials.Add(model);
        dbAutoSerwisContext.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var testimonial = dbAutoSerwisContext.Testimonials.Find(id);
        if (testimonial == null) return NotFound();
        return View(testimonial);
    }

    [HttpPost]
    public IActionResult Edit(Testimonial model)
    {
        var testimonial = dbAutoSerwisContext.Testimonials.Find(model.Id);
        if (testimonial == null) return NotFound();
        testimonial.ClientName = model.ClientName;
        testimonial.Content = model.Content;
        testimonial.Rating = model.Rating;
        testimonial.IsApproved = model.IsApproved;
        dbAutoSerwisContext.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var testimonial = dbAutoSerwisContext.Testimonials.Find(id);
        if (testimonial == null) return NotFound();
        return View(testimonial);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        var testimonial = dbAutoSerwisContext.Testimonials.Find(id);
        if (testimonial != null)
        {
            dbAutoSerwisContext.Testimonials.Remove(testimonial);
            dbAutoSerwisContext.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}
