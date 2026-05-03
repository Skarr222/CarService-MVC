namespace CarService_MVC.Intranet.Models;

public class ServiceViewModel
{
    public int Id { get; set; }
    public int ServiceCategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal EstimatedPrice { get; set; }
    public int EstimatedDuration { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}
