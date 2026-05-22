using System.Globalization;

namespace CarService_MVC.Portal.Helpers;

public static class AnimationHelper
{
    public static string AnimDelay(int index, double step = 0.1)
    {
        return $"animation-delay:{(index * step).ToString("0.##", CultureInfo.InvariantCulture)}s";
    }
}