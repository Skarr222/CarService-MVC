using System;
using System.Collections.Generic;

namespace CarService_MVC.Data.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Position { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public DateTime HireDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<RepairOrder> RepairOrders { get; set; } = new List<RepairOrder>();
}
