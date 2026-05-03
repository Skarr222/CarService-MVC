using CarService_MVC.Data.Data;
using CarService_MVC.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarService_MVC.Intranet.Controllers;

public class CmsContentController : Controller
{
    private readonly ILogger<CmsContentController> _logger;
    private readonly AutoSerwisContext dbAutoSerwisContext;

    public CmsContentController(AutoSerwisContext dbAutoSerwisContext, ILogger<CmsContentController> logger)
    {
        _logger = logger;
        this.dbAutoSerwisContext = dbAutoSerwisContext;
    }

    public IActionResult Index()
    {
        var contents = dbAutoSerwisContext.CmsContents
            .OrderBy(c => c.Section)
            .ThenBy(c => c.SortOrder)
            .ToList();
        return View(contents);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(CmsContent model)
    {
        model.UpdatedAt = DateTime.Now;
        dbAutoSerwisContext.CmsContents.Add(model);
        dbAutoSerwisContext.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var content = dbAutoSerwisContext.CmsContents.Find(id);
        if (content == null) return NotFound();
        return View(content);
    }

    [HttpPost]
    public IActionResult Edit(CmsContent model)
    {
        var content = dbAutoSerwisContext.CmsContents.Find(model.Id);
        if (content == null) return NotFound();
        content.Key = model.Key;
        content.Title = model.Title;
        content.Content = model.Content;
        content.Section = model.Section;
        content.SortOrder = model.SortOrder;
        content.IsActive = model.IsActive;
        content.UpdatedAt = DateTime.Now;
        dbAutoSerwisContext.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var content = dbAutoSerwisContext.CmsContents.Find(id);
        if (content == null) return NotFound();
        return View(content);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        var content = dbAutoSerwisContext.CmsContents.Find(id);
        if (content != null)
        {
            dbAutoSerwisContext.CmsContents.Remove(content);
            dbAutoSerwisContext.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}
