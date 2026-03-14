using System;
using System.Collections.Generic;

namespace CarService_MVC.Data.Models;

public partial class RepairStatusHistory
{
    public int Id { get; set; }

    public int RepairOrderId { get; set; }

    public int OldStatus { get; set; }

    public int NewStatus { get; set; }

    public string ChangedBy { get; set; } = null!;

    public DateTime ChangedAt { get; set; }

    public string? Comment { get; set; }

    public virtual RepairOrder RepairOrder { get; set; } = null!;
}
