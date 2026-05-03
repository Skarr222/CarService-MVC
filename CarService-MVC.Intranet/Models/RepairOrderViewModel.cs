using CarService_MVC.Data.Models;

namespace CarService_MVC.Intranet.Models;

public class RepairOrderViewModel
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int VehicleId { get; set; }
    public int? EmployeeId { get; set; }
    public string Description { get; set; } = string.Empty;
    public RepairOrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PlannedDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public decimal? TotalCost { get; set; }
    public string? Notes { get; set; }
    public string ClientFullName { get; set; } = string.Empty;
    public string VehicleInfo { get; set; } = string.Empty;
}
