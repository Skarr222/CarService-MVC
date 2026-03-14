using System;
using System.Collections.Generic;

namespace CarService_MVC.Data.Models;

public partial class VwActiveRepairOrder
{
    public int Id { get; set; }

    public string ClientName { get; set; } = null!;

    public string ClientPhone { get; set; } = null!;

    public string VehicleName { get; set; } = null!;

    public string LicensePlate { get; set; } = null!;

    public string? MechanicName { get; set; }

    public string Description { get; set; } = null!;

    public int Status { get; set; }

    public string? StatusName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? PlannedDate { get; set; }

    public decimal? TotalCost { get; set; }
}
