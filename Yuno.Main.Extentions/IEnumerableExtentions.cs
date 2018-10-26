using System;
using System.Collections.Generic;

namespace Yuno.Main.Extentions
{
    public static class IEnumerableExtentions
    {
        public static IEnumerable<TSource> Foreach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (var item in source) action(item);
            return source;
        }
        
        public static IEnumerable<TSource> Foreach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> action)
        {
            var index = 0;
            foreach (var item in source) action(item, index++);
            return source;
        }
    }
}
