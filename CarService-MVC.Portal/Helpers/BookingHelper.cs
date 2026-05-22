namespace CarService_MVC.Portal.Helpers;

public static class BookingHelper
{
    public static readonly string[] TimeSlots =
        Enumerable.Range(8, 10).Select(h => $"{h}:00").ToArray();

    public static readonly OpeningHoursEntry[] OpeningHours =
    [
        new("Pon–Pt", "Poniedziałek – Piątek", "8:00 – 18:00", false),
        new("Sob", "Sobota", "9:00 – 14:00", false),
        new("Nd", "Niedziela", "nieczynne", true)
    ];

    public record OpeningHoursEntry(string ShortLabel, string FullLabel, string Hours, bool Closed);
}