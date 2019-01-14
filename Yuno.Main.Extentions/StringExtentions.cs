using System.Globalization;

namespace Yuno.Main.Extentions
{
    public static class StringExtentions
    {
        public static bool ContainsIgnoreCase(this string item, string value)
        {
            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(item, value, CompareOptions.IgnoreCase) >= 0;
        }
    }
}
