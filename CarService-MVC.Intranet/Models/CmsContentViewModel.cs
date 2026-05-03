namespace CarService_MVC.Intranet.Models;

public class CmsContentViewModel
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Section { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime UpdatedAt { get; set; }
}
