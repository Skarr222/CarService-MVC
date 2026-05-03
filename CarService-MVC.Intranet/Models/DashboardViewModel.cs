using CarService_MVC.Data.Models;

namespace CarService_MVC.Intranet.Models;

public class DashboardViewModel
{
    public int TotalClients { get; set; }
    public int TotalVehicles { get; set; }
    public int OpenOrders { get; set; }
    public int PlannedToday { get; set; }
    public int UnreadMessages { get; set; }
    public List<RepairOrder> RecentOrders { get; set; } = [];
}
