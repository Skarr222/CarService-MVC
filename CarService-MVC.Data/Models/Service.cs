using System;
using System.Collections.Generic;

namespace CarService_MVC.Data.Models;

public partial class Service
{
    public int Id { get; set; }

    public int ServiceCategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal EstimatedPrice { get; set; }

    public int EstimatedDuration { get; set; }

    public virtual ICollection<RepairOrderService> RepairOrderServices { get; set; } = new List<RepairOrderService>();

    public virtual ServiceCategory ServiceCategory { get; set; } = null!;
}
