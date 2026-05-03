namespace CarService_MVC.Intranet.Models;

public class TestimonialViewModel
{
    public int Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }
}
