using System;
using System.Collections.Generic;

namespace CarService_MVC.Data.Models;

public partial class RepairOrderService
{
    public int Id { get; set; }

    public int RepairOrderId { get; set; }

    public int ServiceId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public string? Notes { get; set; }

    public virtual RepairOrder RepairOrder { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
