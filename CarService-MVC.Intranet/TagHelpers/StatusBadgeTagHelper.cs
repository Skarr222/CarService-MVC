using CarService_MVC.Data.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CarService_MVC.Intranet.TagHelpers;

[HtmlTargetElement("status-badge")]
public class StatusBadgeTagHelper : TagHelper
{
    private static readonly Dictionary<RepairOrderStatus, (string Css, string Label)> Map = new()
    {
        { RepairOrderStatus.New, ("bg-secondary", "Nowe") },
        { RepairOrderStatus.InProgress, ("bg-warning text-dark", "W trakcie") },
        { RepairOrderStatus.Completed, ("bg-success", "Zakończone") },
        { RepairOrderStatus.Cancelled, ("bg-danger", "Anulowane") },
        { RepairOrderStatus.Ready, ("bg-info text-dark", "Gotowe") },
        { RepairOrderStatus.Released, ("bg-dark", "Wydane") }
    };

    public RepairOrderStatus Status { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var (css, label) = Map[Status];
        output.TagName = "span";
        output.Attributes.SetAttribute("class", $"badge {css}");
        output.Content.SetContent(label);
    }
}