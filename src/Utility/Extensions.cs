using System.Globalization;

namespace Raytracer.Utility
{
    public static class StringExtensions
    {
        public static string DynamicPostFix(this decimal number)
        {
            if (number > 999999999)
            {
                return number.ToString("0,,,.###B", CultureInfo.InvariantCulture);
            }
            else
            if (number > 999999)
            {
                return number.ToString("0,,.##M", CultureInfo.InvariantCulture);
            }
            else
            if (number > 999)
            {
                return number.ToString("0,.#K", CultureInfo.InvariantCulture);
            }
            else
            {
                return number.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}