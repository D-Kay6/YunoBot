using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Extentions
{
    public static class IEnumerableExtentions
    {
        public static IEnumerable<TSource> Foreach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (var item in source) action(item);
            return source;
        }

        public static IEnumerable<TSource> Foreach<TSource>(this IEnumerable<TSource> source,
            Action<TSource, int> action)
        {
            var index = 0;
            foreach (var item in source) action(item, index++);
            return source;
        }

        public static TSource GetRandomItem<TSource>(this IEnumerable<TSource> source)
        {
            var random = new Random();
            var index = random.Next(source.Count());
            return source.ElementAt(index);
        }

        public static IEnumerable<TSource> Add<TSource>(this IEnumerable<TSource> enumerable, TSource value)
        {
            return enumerable.Concat(new[] {value});
        }

        public static IEnumerable<TSource> Insert<TSource>(this IEnumerable<TSource> enumerable, int index,
            TSource value)
        {
            return enumerable.SelectMany((x, i) => index == i ? new[] {value, x} : new[] {x});
        }

        public static IEnumerable<TSource> Replace<TSource>(this IEnumerable<TSource> enumerable, int index,
            TSource value)
        {
            return enumerable.Select((x, i) => index == i ? value : x);
        }

        public static IEnumerable<TSource> Remove<TSource>(this IEnumerable<TSource> enumerable, int index)
        {
            return enumerable.Where((x, i) => index != i);
        }

        public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source)
        {
            var list = source.ToList();
            var random = new Random();
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                var j = random.Next(i, count);
                var item = list[i];
                list[i] = list[j];
                list[j] = item;
            }

            return list;
        }

        public static Queue<TSource> Shuffle<TSource>(this Queue<TSource> source)
        {
            var list = source.ToList();
            var random = new Random();
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                var j = random.Next(i, count);
                var item = list[i];
                list[i] = list[j];
                list[j] = item;
            }

            return new Queue<TSource>(list);
        }

        public static void Shuffle<TSource>(this IList<TSource> source)
        {
            var random = new Random();
            var count = source.Count;
            for (var i = count - 1; i >= 0; i--)
            {
                var j = random.Next(count);
                var item = source[i];
                source[i] = source[j];
                source[j] = item;
            }
        }
    }
}