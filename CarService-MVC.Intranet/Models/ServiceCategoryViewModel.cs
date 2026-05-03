namespace CarService_MVC.Intranet.Models;

public class ServiceCategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconCss { get; set; }
    public int ServiceCount { get; set; }
}
