using System.Globalization;

namespace CarService_MVC.Intranet.Helpers;

public static class FormParser
{
    public static int Quantity(string? formValue)
    {
        return int.TryParse(formValue, out int quantity) && quantity >= 1 ? quantity : 1;
    }

    public static decimal UnitPrice(string? formValue)
    {
        var normalized = formValue?.Replace(',', '.');
        return decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price)
            ? price
            : 0m;
    }

    public static decimal? OptionalDecimal(string? formValue)
    {
        var normalized = formValue?.Replace(',', '.');
        return decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price)
            ? price
            : null;
    }
}
