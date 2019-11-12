using System;
using System.Globalization;
using System.Linq;

namespace Logic.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsIgnoreCase(this string item, string value)
        {
            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(item, value, CompareOptions.IgnoreCase) >= 0;
        }

        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            return input.First().ToString().ToUpper() + input.Substring(1).ToLower();
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