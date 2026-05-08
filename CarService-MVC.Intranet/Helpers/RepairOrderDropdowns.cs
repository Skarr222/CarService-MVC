using System.Globalization;
using CarService_MVC.Data.Data;
using CarService_MVC.Data.Models;
using CarService_MVC.Intranet.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarService_MVC.Intranet.Helpers;

public static class RepairOrderDropdowns
{
    private static readonly Dictionary<RepairOrderStatus, string> StatusLabels = new()
    {
        { RepairOrderStatus.New,        "Nowe" },
        { RepairOrderStatus.InProgress, "W trakcie" },
        { RepairOrderStatus.Completed,  "Zakończone" },
        { RepairOrderStatus.Cancelled,  "Anulowane" },
        { RepairOrderStatus.Ready,      "Gotowe" },
        { RepairOrderStatus.Released,   "Wydane" },
    };

    public static SelectList Clients(AutoSerwisContext db, int? selectedId = null) =>
        new SelectList(
            db.Clients
                .Where(client => client.IsActive)
                .OrderBy(client => client.LastName)
                .Select(client => new { client.Id, Name = client.FirstName + " " + client.LastName }),
            "Id", "Name", selectedId);

    public static SelectList Vehicles(AutoSerwisContext db, int? selectedId = null) =>
        new SelectList(
            db.Vehicles
                .OrderBy(vehicle => vehicle.Brand)
                .Select(vehicle => new { vehicle.Id, Name = vehicle.Brand + " " + vehicle.Model + " (" + vehicle.LicensePlate + ")" }),
            "Id", "Name", selectedId);

    public static SelectList Employees(AutoSerwisContext db, int? selectedId = null) =>
        new SelectList(
            db.Employees
                .Where(employee => employee.IsActive)
                .OrderBy(employee => employee.LastName)
                .Select(employee => new { employee.Id, Name = employee.FirstName + " " + employee.LastName }),
            "Id", "Name", selectedId);

    public static SelectList Statuses(int? selectedValue = null) =>
        new SelectList(
            Enum.GetValues<RepairOrderStatus>()
                .Select(status => new { Value = (int)status, Text = StatusLabels[status] }),
            "Value", "Text", selectedValue);

    public static List<ServiceSelectionItem> Services(AutoSerwisContext db, IEnumerable<RepairOrderService>? existingServices = null)
    {
        var existingMap = existingServices?.ToDictionary(ros => ros.ServiceId, ros => ros)
            ?? new Dictionary<int, RepairOrderService>();

        return db.Services
            .Include(service => service.ServiceCategory)
            .OrderBy(service => service.ServiceCategory.Name)
            .ThenBy(service => service.Name)
            .ToList()
            .Select(service => new ServiceSelectionItem
            {
                ServiceId     = service.Id,
                Name          = service.Name,
                CategoryName  = service.ServiceCategory.Name,
                EstimatedPrice = service.EstimatedPrice,
                IsSelected    = existingMap.ContainsKey(service.Id),
                Quantity      = existingMap.GetValueOrDefault(service.Id)?.Quantity ?? 1,
                UnitPrice     = existingMap.GetValueOrDefault(service.Id)?.UnitPrice ?? service.EstimatedPrice
            })
            .ToList();
    }
}
