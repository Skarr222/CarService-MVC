using System;
using System.Collections.Generic;

namespace CarService_MVC.Data.Models;

public partial class RepairOrder
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int VehicleId { get; set; }

    public int? EmployeeId { get; set; }

    public string Description { get; set; } = null!;

    public int Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? PlannedDate { get; set; }

    public DateTime? CompletedAt { get; set; }

    public decimal? TotalCost { get; set; }

    public string? Notes { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<RepairOrderService> RepairOrderServices { get; set; } = new List<RepairOrderService>();

    public virtual ICollection<RepairPhoto> RepairPhotos { get; set; } = new List<RepairPhoto>();

    public virtual ICollection<RepairStatusHistory> RepairStatusHistories { get; set; } = new List<RepairStatusHistory>();

    public virtual Vehicle Vehicle { get; set; } = null!;
}
