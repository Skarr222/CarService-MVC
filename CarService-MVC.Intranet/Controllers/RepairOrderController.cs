using CarService_MVC.Data.Data;
using CarService_MVC.Data.Models;
using CarService_MVC.Intranet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    public IActionResult Index()
    {
        var orders = dbAutoSerwisContext.RepairOrders
            .Include(r => r.Client)
            .Include(r => r.Vehicle)
            .Include(r => r.Employee)
            .OrderByDescending(r => r.CreatedAt)
            .ToList();
        return View(orders);
    }

    public IActionResult Create()
    {
        PopulateDropdowns();
        return View();
    }

    [HttpPost]
    public IActionResult Create(RepairOrder model, List<int> ServiceIds)
    {
        model.CreatedAt = DateTime.Now;
        dbAutoSerwisContext.RepairOrders.Add(model);
        dbAutoSerwisContext.SaveChanges();

        foreach (var serviceId in ServiceIds)
        {
            var qty = int.TryParse(Request.Form[$"ServiceQty_{serviceId}"], out var q) ? q : 1;
            var unitPrice = decimal.TryParse(Request.Form[$"ServicePrice_{serviceId}"], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var p) ? p : 0;
            dbAutoSerwisContext.RepairOrderServices.Add(new RepairOrderService
            {
                RepairOrderId = model.Id,
                ServiceId = serviceId,
                Quantity = qty < 1 ? 1 : qty,
                UnitPrice = unitPrice
            });
        }
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
        PopulateDropdowns(order.ClientId, order.VehicleId, order.EmployeeId, order.Status, order.RepairOrderServices);
        return View(order);
    }

    [HttpPost]
    public IActionResult Edit(RepairOrder model, List<int> ServiceIds)
    {
        var order = dbAutoSerwisContext.RepairOrders
            .Include(r => r.RepairOrderServices)
            .FirstOrDefault(r => r.Id == model.Id);
        if (order == null) return NotFound();
        order.ClientId = model.ClientId;
        order.VehicleId = model.VehicleId;
        order.EmployeeId = model.EmployeeId;
        order.Description = model.Description;
        order.Status = model.Status;
        order.PlannedDate = model.PlannedDate;
        order.CompletedAt = model.CompletedAt;
        order.TotalCost = model.TotalCost;
        order.Notes = model.Notes;

        dbAutoSerwisContext.RepairOrderServices.RemoveRange(order.RepairOrderServices);
        foreach (var serviceId in ServiceIds)
        {
            var qty = int.TryParse(Request.Form[$"ServiceQty_{serviceId}"], out var q) ? q : 1;
            var unitPrice = decimal.TryParse(Request.Form[$"ServicePrice_{serviceId}"], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var p) ? p : 0;
            dbAutoSerwisContext.RepairOrderServices.Add(new RepairOrderService
            {
                RepairOrderId = order.Id,
                ServiceId = serviceId,
                Quantity = qty < 1 ? 1 : qty,
                UnitPrice = unitPrice
            });
        }

        dbAutoSerwisContext.SaveChanges();
        return RedirectToAction("Index");
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

    private void PopulateDropdowns(int? selectedClientId = null, int? selectedVehicleId = null, int? selectedEmployeeId = null, RepairOrderStatus? selectedStatus = null, IEnumerable<RepairOrderService>? existingServices = null)
    {
        ViewBag.Clients = new SelectList(
            dbAutoSerwisContext.Clients.Where(c => c.IsActive).OrderBy(c => c.LastName).Select(c => new { c.Id, Name = c.FirstName + " " + c.LastName }),
            "Id", "Name", selectedClientId);

        ViewBag.Vehicles = new SelectList(
            dbAutoSerwisContext.Vehicles.OrderBy(v => v.Brand).Select(v => new { v.Id, Name = v.Brand + " " + v.Model + " (" + v.LicensePlate + ")" }),
            "Id", "Name", selectedVehicleId);

        ViewBag.Employees = new SelectList(
            dbAutoSerwisContext.Employees.Where(e => e.IsActive).OrderBy(e => e.LastName).Select(e => new { e.Id, Name = e.FirstName + " " + e.LastName }),
            "Id", "Name", selectedEmployeeId);

        var statusLabels = new Dictionary<RepairOrderStatus, string>
        {
            { RepairOrderStatus.New,       "Nowe" },
            { RepairOrderStatus.InProgress,"W trakcie" },
            { RepairOrderStatus.Completed, "Zakończone" },
            { RepairOrderStatus.Cancelled, "Anulowane" },
            { RepairOrderStatus.Ready,     "Gotowe" },
            { RepairOrderStatus.Released,  "Wydane" },
        };
        ViewBag.Statuses = new SelectList(
            Enum.GetValues<RepairOrderStatus>().Select(s => new { Value = (int)s, Text = statusLabels[s] }),
            "Value", "Text", (int?)selectedStatus);

        var existingMap = existingServices?.ToDictionary(s => s.ServiceId, s => s) ?? new Dictionary<int, RepairOrderService>();
        ViewBag.Services = dbAutoSerwisContext.Services
            .Include(s => s.ServiceCategory)
            .OrderBy(s => s.ServiceCategory.Name)
            .ThenBy(s => s.Name)
            .ToList()
            .Select(s => new ServiceSelectionItem
            {
                ServiceId = s.Id,
                Name = s.Name,
                CategoryName = s.ServiceCategory.Name,
                EstimatedPrice = s.EstimatedPrice,
                IsSelected = existingMap.ContainsKey(s.Id),
                Quantity = existingMap.TryGetValue(s.Id, out var ex) ? ex.Quantity : 1,
                UnitPrice = existingMap.TryGetValue(s.Id, out var ex2) ? ex2.UnitPrice : s.EstimatedPrice
            })
            .ToList();
    }
}
