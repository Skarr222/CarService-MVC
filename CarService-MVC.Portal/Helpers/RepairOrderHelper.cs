using CarService_MVC.Data.Models;

namespace CarService_MVC.Portal.Helpers;

public static class RepairOrderHelper
{
    public static string StatusLabel(RepairOrderStatus status)
    {
        return status switch
        {
            RepairOrderStatus.New => "Nowe",
            RepairOrderStatus.InProgress => "W trakcie",
            RepairOrderStatus.Completed => "Zakończone",
            RepairOrderStatus.Cancelled => "Anulowane",
            RepairOrderStatus.Ready => "Gotowe do odbioru",
            RepairOrderStatus.Released => "Wydane",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }

    public static string StatusClass(RepairOrderStatus status)
    {
        return status switch
        {
            RepairOrderStatus.New => "status-new",
            RepairOrderStatus.InProgress => "status-inprogress",
            RepairOrderStatus.Completed => "status-completed",
            RepairOrderStatus.Cancelled => "status-cancelled",
            RepairOrderStatus.Ready => "status-ready",
            RepairOrderStatus.Released => "status-released",
            _ => ""
        };
    }

    public static string StatusIcon(RepairOrderStatus status)
    {
        return status switch
        {
            RepairOrderStatus.New => "bi-plus-circle",
            RepairOrderStatus.InProgress => "bi-wrench",
            RepairOrderStatus.Completed => "bi-check-circle",
            RepairOrderStatus.Cancelled => "bi-x-circle",
            RepairOrderStatus.Ready => "bi-bell",
            RepairOrderStatus.Released => "bi-car-front",
            _ => "bi-circle"
        };
    }
}
