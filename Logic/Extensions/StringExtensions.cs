using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

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

        public static TimeSpan GetDuration(this string item)
        {
            var duration = TimeSpan.Zero;

            var match = Regex.Match(item, @"\d+s");
            if (match.Success) duration += TimeSpan.FromSeconds(int.Parse(Regex.Match(match.Value, @"\d+").Value));
            match = Regex.Match(item, @"\d+m");
            if (match.Success) duration += TimeSpan.FromMinutes(int.Parse(Regex.Match(match.Value, @"\d+").Value));
            match = Regex.Match(item, @"\d+h");
            if (match.Success) duration += TimeSpan.FromHours(int.Parse(Regex.Match(match.Value, @"\d+").Value));
            match = Regex.Match(item, @"\d+d");
            if (match.Success) duration += TimeSpan.FromDays(int.Parse(Regex.Match(match.Value, @"\d+").Value));

            if (duration != TimeSpan.Zero) return duration;

            match = Regex.Match(item, @"\d+");
            if (match.Success) duration += TimeSpan.FromDays(int.Parse(Regex.Match(match.Value, @"\d+").Value));
            return duration;
        }
    }
}