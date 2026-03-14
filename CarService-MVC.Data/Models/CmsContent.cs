using System;
using System.Collections.Generic;

namespace CarService_MVC.Data.Models;

public partial class CmsContent
{
    public int Id { get; set; }

    public string Key { get; set; } = null!;

    public string? Title { get; set; }

    public string Content { get; set; } = null!;

    public string Section { get; set; } = null!;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime UpdatedAt { get; set; }
}
