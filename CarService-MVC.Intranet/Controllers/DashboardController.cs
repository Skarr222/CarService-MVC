using System.Diagnostics;
using CarService_MVC.Data.Data;
using CarService_MVC.Data.Models;
using CarService_MVC.Intranet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarService_MVC.Intranet.Controllers;

public class DashboardController : Controller
{
    private readonly ILogger<DashboardController> _logger;
    private readonly AutoSerwisContext dbAutoSerwisContext;

    public DashboardController(AutoSerwisContext dbAutoSerwisContext, ILogger<DashboardController> logger)
    {
        _logger = logger;
        this.dbAutoSerwisContext = dbAutoSerwisContext;
    }

    public IActionResult Index()
    {
        var today = DateTime.Today;
        var dashboardModel = new DashboardViewModel
        {
            TotalClients = dbAutoSerwisContext.Clients.Count(),
            TotalVehicles = dbAutoSerwisContext.Vehicles.Count(),
            OpenOrders = dbAutoSerwisContext.RepairOrders.Count(r => r.Status == RepairOrderStatus.New),
            PlannedToday = dbAutoSerwisContext.RepairOrders.Count(r => r.PlannedDate.HasValue && r.PlannedDate.Value.Date == today),
            UnreadMessages = dbAutoSerwisContext.ContactRequests.Count(c => !c.IsRead),
            RecentOrders = dbAutoSerwisContext.RepairOrders
                .Include(r => r.Client)
                .Include(r => r.Vehicle)
                .OrderByDescending(r => r.CreatedAt)
                .Take(5)
                .ToList()
        };

        return View(dashboardModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        _logger.LogError("Wystąpił błąd. RequestId: {RequestId}", requestId);
        return View(new ErrorViewModel { RequestId = requestId });
    }
}