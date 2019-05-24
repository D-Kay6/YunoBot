using System;
using System.Globalization;

namespace Logic.Extentions
{
    public static class StringExtentions
    {
        public static bool ContainsIgnoreCase(this string item, string value)
        {
            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(item, value, CompareOptions.IgnoreCase) >= 0;
        }

        public static string ReplaceLast(this string item, string find, string replace)
        {
            var place = item.LastIndexOf(find, StringComparison.Ordinal);
            return place == -1 ? item : item.Remove(place, find.Length).Insert(place, replace);
        }

        public static string ToPossessive(this string item)
        {
            var s = item.EndsWith("s", StringComparison.CurrentCultureIgnoreCase) ? "" : "s";
            return $"{item}'{s}";
        }
    }
}