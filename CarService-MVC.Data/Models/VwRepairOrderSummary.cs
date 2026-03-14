using System;
using System.Collections.Generic;

namespace CarService_MVC.Data.Models;

public partial class VwRepairOrderSummary
{
    public int RepairOrderId { get; set; }

    public string ServiceName { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? LineTotal { get; set; }
}
