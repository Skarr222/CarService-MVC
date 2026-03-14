using System;
using System.Collections.Generic;

namespace CarService_MVC.Data.Models;

public partial class RepairPhoto
{
    public int Id { get; set; }

    public int RepairOrderId { get; set; }

    public string FilePath { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime UploadedAt { get; set; }

    public virtual RepairOrder RepairOrder { get; set; } = null!;
}
