using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Raytracer.Utility
{
    public static class StringExtensions
    {
        public static string DynamicPrefix(this decimal number)
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

    public static class ListExtensions
    {
        public static IEnumerable<List<T>> SplitList<T>(this List<T> locations, int nSize = 30)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }

        public static IEnumerable<IEnumerable<T>> ChunkTrivialBetter<T>(this IEnumerable<T> source, int chunksize)
        {
            var pos = 0;
            while (source.Skip(pos).Any())
            {
                yield return source.Skip(pos).Take(chunksize);
                pos += chunksize;
            }
        }
    }
}