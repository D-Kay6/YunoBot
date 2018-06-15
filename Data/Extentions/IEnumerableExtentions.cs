using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extentions
{
    public static class IEnumerableExtentions
    {
        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (TSource item in source) action(item);
            return source;
        }

        
        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> action)
        {
            var index = 0;
            foreach (TSource item in source) action(item, index++);
            return source;
        }
    }
}
