using CarService_MVC.Data.Data;
using CarService_MVC.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarService_MVC.Intranet.Helpers;

public static class VehicleDropdowns
{
    public static SelectList Clients(AutoSerwisContext db, int? selectedId = null) =>
        new SelectList(
            db.Clients
                .Where(client => client.IsActive)
                .OrderBy(client => client.LastName)
                .Select(client => new { client.Id, Name = client.FirstName + " " + client.LastName }),
            "Id", "Name", selectedId);

    public static SelectList EngineTypes(EngineType? selected = null) =>
        new SelectList(
            Enum.GetValues<EngineType>()
                .Select(engineType => new { Value = engineType.ToString(), Text = engineType.ToString() }),
            "Value", "Text", selected?.ToString());
}
