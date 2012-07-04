using System;
using System.Collections.Generic;

namespace SystemDot
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> toEnumerate, Action<T> toPerform)
        {
            foreach (var item in toEnumerate)
            {
                toPerform(item);
            }
        }
    }
}