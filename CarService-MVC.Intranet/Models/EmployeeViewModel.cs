namespace CarService_MVC.Intranet.Models;

public class EmployeeViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; }
}
