using CarService_MVC.Data.Data;
using Microsoft.AspNetCore.Mvc;

namespace CarService_MVC.Intranet.Controllers;

public class ClientController : Controller
{
    private readonly ILogger<ClientController> _logger;
    private readonly AutoSerwisContext dbAutoSerwisContext;

    public ClientController(AutoSerwisContext dbAutoSerwisContext, ILogger<ClientController> logger)
    {
        _logger = logger;
        this.dbAutoSerwisContext = dbAutoSerwisContext;
    }

    public IActionResult Index()
    {
        return View();
    }
}