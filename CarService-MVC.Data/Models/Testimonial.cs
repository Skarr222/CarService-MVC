using System;
using System.Collections.Generic;

namespace CarService_MVC.Data.Models;

public partial class Testimonial
{
    public int Id { get; set; }

    public string ClientName { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int Rating { get; set; }

    public bool IsApproved { get; set; }

    public DateTime CreatedAt { get; set; }
}
