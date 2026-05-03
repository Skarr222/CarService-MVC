namespace CarService_MVC.Intranet.Models;

public class VehicleViewModel
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string? Vin { get; set; }
    public string EngineType { get; set; } = string.Empty;
    public string ClientFullName { get; set; } = string.Empty;
}
