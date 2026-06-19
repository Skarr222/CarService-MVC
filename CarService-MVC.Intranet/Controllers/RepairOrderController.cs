using System.Globalization;
using System.Text;
using CarService_MVC.Data.Data;
using CarService_MVC.Data.Models;
using CarService_MVC.Intranet.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarService_MVC.Intranet.Controllers;

public class RepairOrderController : Controller
{
    private readonly ILogger<RepairOrderController> _logger;
    private readonly AutoSerwisContext dbAutoSerwisContext;

    public RepairOrderController(AutoSerwisContext dbAutoSerwisContext, ILogger<RepairOrderController> logger)
    {
        _logger = logger;
        this.dbAutoSerwisContext = dbAutoSerwisContext;
    }

    public IActionResult Index(string? search)
    {
        var query = dbAutoSerwisContext.RepairOrders
            .Include(r => r.Client)
            .Include(r => r.Vehicle)
            .Include(r => r.Employee)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(r =>
                r.Client.FirstName.Contains(search) ||
                r.Client.LastName.Contains(search) ||
                r.Vehicle.Brand.Contains(search) ||
                r.Vehicle.Model.Contains(search) ||
                (r.Vehicle.LicensePlate != null && r.Vehicle.LicensePlate.Contains(search)) ||
                (r.Description != null && r.Description.Contains(search)));

        ViewBag.Search = search;
        var orders = query
            .OrderByDescending(r => r.CreatedAt)
            .ToList();
        return View(orders);
    }

    public IActionResult Create()
    {
        ViewBag.Clients = RepairOrderDropdowns.Clients(dbAutoSerwisContext);
        ViewBag.VehicleOptions = RepairOrderDropdowns.VehicleOptions(dbAutoSerwisContext);
        ViewBag.Employees = RepairOrderDropdowns.Employees(dbAutoSerwisContext);
        ViewBag.Statuses = RepairOrderDropdowns.Statuses();
        ViewBag.Services = RepairOrderDropdowns.Services(dbAutoSerwisContext);
        return View();
    }

    [HttpPost]
    public IActionResult Create(RepairOrder model, List<int> ServiceIds)
    {
        model.CreatedAt = DateTime.Now;
        model.TotalCost = ServiceIds.Sum(serviceId =>
            FormParser.UnitPrice(Request.Form[$"ServicePrice_{serviceId}"]) *
            FormParser.Quantity(Request.Form[$"ServiceQty_{serviceId}"]));
        dbAutoSerwisContext.RepairOrders.Add(model);
        dbAutoSerwisContext.SaveChanges();

        foreach (var serviceId in ServiceIds)
            dbAutoSerwisContext.RepairOrderServices.Add(new RepairOrderService
            {
                RepairOrderId = model.Id,
                ServiceId = serviceId,
                Quantity = FormParser.Quantity(Request.Form[$"ServiceQty_{serviceId}"]),
                UnitPrice = FormParser.UnitPrice(Request.Form[$"ServicePrice_{serviceId}"])
            });

        if (ServiceIds.Any())
            dbAutoSerwisContext.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var order = dbAutoSerwisContext.RepairOrders
            .Include(r => r.RepairOrderServices)
            .FirstOrDefault(r => r.Id == id);
        if (order == null) return NotFound();
        ViewBag.Clients = RepairOrderDropdowns.Clients(dbAutoSerwisContext, order.ClientId);
        ViewBag.Vehicles = RepairOrderDropdowns.Vehicles(dbAutoSerwisContext, order.VehicleId);
        ViewBag.Employees = RepairOrderDropdowns.Employees(dbAutoSerwisContext, order.EmployeeId);
        ViewBag.Statuses = RepairOrderDropdowns.Statuses((int?)order.Status);
        ViewBag.Services = RepairOrderDropdowns.Services(dbAutoSerwisContext, order.RepairOrderServices);
        return View(order);
    }

    [HttpPost]
    public IActionResult Edit(RepairOrder model, List<int> ServiceIds)
    {
        var order = dbAutoSerwisContext.RepairOrders
            .Include(r => r.RepairOrderServices)
            .FirstOrDefault(r => r.Id == model.Id);
        if (order == null) return NotFound();

        if (order.Status != model.Status)
            dbAutoSerwisContext.RepairStatusHistories.Add(new RepairStatusHistory
            {
                RepairOrderId = order.Id,
                OldStatus = order.Status,
                NewStatus = model.Status,
                ChangedBy = User.Identity?.Name ?? "System",
                ChangedAt = DateTime.Now,
                Comment = Request.Form["StatusComment"]
            });

        order.ClientId = model.ClientId;
        order.VehicleId = model.VehicleId;
        order.EmployeeId = model.EmployeeId;
        order.Description = model.Description;
        order.Status = model.Status;
        order.PlannedDate = model.PlannedDate;
        order.CompletedAt = model.CompletedAt;
        order.TotalCost = ServiceIds.Sum(serviceId =>
            FormParser.UnitPrice(Request.Form[$"ServicePrice_{serviceId}"]) *
            FormParser.Quantity(Request.Form[$"ServiceQty_{serviceId}"]));
        order.Notes = model.Notes;

        dbAutoSerwisContext.RepairOrderServices.RemoveRange(order.RepairOrderServices);
        foreach (var serviceId in ServiceIds)
            dbAutoSerwisContext.RepairOrderServices.Add(new RepairOrderService
            {
                RepairOrderId = order.Id,
                ServiceId = serviceId,
                Quantity = FormParser.Quantity(Request.Form[$"ServiceQty_{serviceId}"]),
                UnitPrice = FormParser.UnitPrice(Request.Form[$"ServicePrice_{serviceId}"])
            });

        dbAutoSerwisContext.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult History(int id)
    {
        var order = dbAutoSerwisContext.RepairOrders
            .Include(r => r.Client)
            .Include(r => r.Vehicle)
            .Include(r => r.RepairStatusHistories)
            .FirstOrDefault(r => r.Id == id);
        if (order == null) return NotFound();

        ViewBag.OrderId = id;
        ViewBag.ClientName = order.Client.FirstName + " " + order.Client.LastName;
        return View(order.RepairStatusHistories.OrderByDescending(h => h.ChangedAt).ToList());
    }

    public IActionResult Photos(int id)
    {
        var order = dbAutoSerwisContext.RepairOrders
            .Include(r => r.Client)
            .Include(r => r.RepairPhotos)
            .FirstOrDefault(r => r.Id == id);
        if (order == null) return NotFound();

        ViewBag.OrderId = id;
        ViewBag.ClientName = order.Client.FirstName + " " + order.Client.LastName;
        return View(order.RepairPhotos.OrderByDescending(p => p.UploadedAt).ToList());
    }

    [HttpPost]
    public async Task<IActionResult> UploadPhoto(int repairOrderId, IFormFile file, string? description)
    {
        if (file != null && file.Length > 0)
        {
            var uploadsDir = Path.Combine("wwwroot", "uploads", "repairs");
            Directory.CreateDirectory(uploadsDir);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            dbAutoSerwisContext.RepairPhotos.Add(new RepairPhoto
            {
                RepairOrderId = repairOrderId,
                FilePath = $"/uploads/repairs/{fileName}",
                Description = description,
                UploadedAt = DateTime.Now
            });
            dbAutoSerwisContext.SaveChanges();
        }

        return RedirectToAction("Photos", new { id = repairOrderId });
    }

    [HttpPost]
    public IActionResult DeletePhoto(int id, int repairOrderId)
    {
        var photo = dbAutoSerwisContext.RepairPhotos.Find(id);
        if (photo != null)
        {
            var path = Path.Combine("wwwroot", photo.FilePath.TrimStart('/'));
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            dbAutoSerwisContext.RepairPhotos.Remove(photo);
            dbAutoSerwisContext.SaveChanges();
        }

        return RedirectToAction("Photos", new { id = repairOrderId });
    }

    public IActionResult ExportCsv()
    {
        var orders = dbAutoSerwisContext.RepairOrders
            .Include(r => r.Client)
            .Include(r => r.Vehicle)
            .OrderByDescending(r => r.CreatedAt)
            .ToList();

        var sb = new StringBuilder();
        sb.AppendLine("Id;Klient;Pojazd;Status;Utworzono;Koszt");
        foreach (var o in orders)
        {
            var klient = $"{o.Client.FirstName} {o.Client.LastName}";
            var pojazd = $"{o.Vehicle.Brand} {o.Vehicle.Model}";
            var koszt = o.TotalCost?.ToString("F2", CultureInfo.InvariantCulture) ?? "0.00";
            sb.AppendLine($"{o.Id};{klient};{pojazd};{o.Status};{o.CreatedAt:yyyy-MM-dd};{koszt}");
        }

        var bytes = Encoding.UTF8.GetPreamble()
            .Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
        return File(bytes, "text/csv", $"zlecenia_{DateTime.Now:yyyyMMdd}.csv");
    }

    public IActionResult Delete(int id)
    {
        var order = dbAutoSerwisContext.RepairOrders
            .Include(r => r.Client)
            .Include(r => r.Vehicle)
            .FirstOrDefault(r => r.Id == id);
        if (order == null) return NotFound();
        return View(order);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        var order = dbAutoSerwisContext.RepairOrders.Find(id);
        if (order != null)
        {
            dbAutoSerwisContext.RepairOrders.Remove(order);
            dbAutoSerwisContext.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    public IActionResult TestError()
    {
        throw new Exception("Testowy wyjątek - sprawdzenie middleware!");
    }
}