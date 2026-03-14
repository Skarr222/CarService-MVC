using System;
using System.Collections.Generic;

namespace CarService_MVC.Data.Models;

public partial class Vehicle
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public int Year { get; set; }

    public string LicensePlate { get; set; } = null!;

    public string? Vin { get; set; }

    public string EngineType { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<RepairOrder> RepairOrders { get; set; } = new List<RepairOrder>();
}
