using Microsoft.AspNetCore.Mvc;

namespace CarService_MVC.Intranet.Controllers;

public class ServiceController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(object model) => RedirectToAction("Index");

    public IActionResult Edit(int id) => View();

    [HttpPost]
    public IActionResult Edit(object model) => RedirectToAction("Index");

    public IActionResult Delete(int id) => View();

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id) => RedirectToAction("Index");
}
