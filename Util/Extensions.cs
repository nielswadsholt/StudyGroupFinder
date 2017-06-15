using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public static class Extensions
    {
        /// <summary>
        /// Performs the specified action on each element of the IEnumerable<T>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (T item in collection ?? Enumerable.Empty<T>())
            {
                action(item);
            }
        }

        /// <summary>
        /// Returns a string representation of the collection with elements separated
        /// by the given separator. The separator defaults to a single space character.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToSeparatedString<T>(this IEnumerable<T> collection, string separator = " ")
        {
            return string.Join(separator, collection);
        }

        /// <summary>
        /// Shuffles the list using the Fisher-Yates shuffle algorithm
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            Random random = new Random();
            int n = list.Count();

            for (int i = 0; i < n; i++)
            {
                int r = i + (int)(random.NextDouble() * (n - i));
                T t = list[r];
                list[r] = list[i];
                list[i] = t;
            }
        }
    }
}
